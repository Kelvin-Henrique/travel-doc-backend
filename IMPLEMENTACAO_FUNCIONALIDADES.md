# Implementação de Funcionalidades do Sistema de Viagens

Este documento descreve as funcionalidades implementadas no backend (travel-doc-backend) conforme requisitos solicitados.

## Funcionalidades Implementadas

### 1. ✅ Endpoint de Busca/Filtro de Viagens (GET /viagens)

**Status:** ✅ Implementado (já existia)

Permite buscar viagens com parâmetros flexíveis:
- `usuarioId` (obrigatório): ID do usuário
- `nome` (opcional): Filtro por nome da viagem
- `destino` (opcional): Filtro por destino
- `dataInicio` (opcional): Filtro por data de início (>=)
- `dataFim` (opcional): Filtro por data de fim (<=)

**Exemplo:** `GET /viagens?usuarioId=1&destino=Paris`

### 2. ✅ Correção de Informações Retornadas em Próximas Viagens

**Status:** ✅ Implementado

Todas as queries de viagens agora retornam o campo `participantes` populado com a lista de usuários convidados:

- `GET /viagens/proximas` - Viagens futuras
- `GET /viagens/historico` - Viagens passadas
- `GET /viagens/{id}` - Detalhes da viagem
- `GET /viagens` - Busca/filtro de viagens
- `GET /viagens/usuario/{usuarioId}` - Todas as viagens do usuário

**Estrutura de resposta:**
```json
{
  "id": 1,
  "nomeViagem": "Férias em Paris",
  "destino": "Paris, França",
  "dataInicio": "2024-12-15T00:00:00Z",
  "dataFim": "2024-12-25T00:00:00Z",
  "descricao": "Viagem de férias para conhecer Paris",
  "criadorId": 1,
  "status": 1,
  "participantes": [
    {
      "id": 2,
      "nome": "Maria Silva",
      "email": "maria@email.com"
    }
  ]
}
```

### 3. ✅ Endpoint de Edição/Atualização de Viagem (PUT /viagens/{id})

**Status:** ✅ Implementado

Permite atualizar informações gerais da viagem:
- Nome da viagem
- Destino
- Data de início
- Data de fim
- Descrição
- Status (Planejada, EmAndamento, Finalizada, Cancelada)

**Validações:**
- Verifica se a viagem existe
- Verifica se o usuário é o criador da viagem (autorização)
- Valida todos os campos conforme regras de negócio

**Endpoint:** `PUT /viagens/{id}`

**Exemplo de Request:**
```json
{
  "usuarioId": 1,
  "nomeViagem": "Férias em Paris - Atualizado",
  "destino": "Paris, França",
  "dataInicio": "2024-12-15T00:00:00Z",
  "dataFim": "2024-12-30T00:00:00Z",
  "descricao": "Viagem de férias atualizada",
  "status": 1
}
```

### 4. ✅ Adicionar Convidados à Viagem (POST /viagens/participantes)

**Status:** ✅ Implementado (já existia)

Permite adicionar participantes/convidados à viagem através do email:
- Valida se o usuário existe no sistema
- Cria convite com status Pendente
- Retorna erro se o convidado não está cadastrado

**Endpoint:** `POST /viagens/participantes`

**Exemplo de Request:**
```json
{
  "viagemId": 1,
  "emailConvidado": "convidado@email.com",
  "status": 1
}
```

### 5. ✅ Remover Convidados da Viagem (DELETE /viagens/{viagemId}/participantes/{participanteId})

**Status:** ✅ Implementado

Permite remover participantes/convidados de uma viagem:
- Verifica se a viagem existe
- Verifica se o usuário é o criador da viagem (autorização)
- Verifica se o participante está associado à viagem
- Remove o relacionamento

**Endpoint:** `DELETE /viagens/{viagemId}/participantes/{participanteId}?usuarioId={usuarioId}`

**Exemplo:** `DELETE /viagens/1/participantes/2?usuarioId=1`

### 6. ⚠️ Funcionalidades de Carro Alugado e Bagagem

**Status:** ⚠️ Não Implementado

**Motivo:** As entidades de Carro Alugado (CarroAlugado) e Bagagem não existem no schema do banco de dados atual. Para implementar estas funcionalidades, seria necessário:

1. Criar as entidades de domínio:
   - `CarroAlugado` (com campos como modelo, placa, empresa, fotos, etc.)
   - `Bagagem` (com campos como tipo, peso, quantidade, etc.)

2. Criar migrations do Entity Framework para adicionar as tabelas

3. Implementar os endpoints CRUD para estas entidades

4. Associar estas entidades à Viagem

**Recomendação:** Criar uma issue separada para implementação completa destas funcionalidades, incluindo modelagem de dados e migração do banco.

## Melhorias Implementadas

### Repositórios Atualizados

1. **IViagemParticipanteRepository / ViagemParticipanteRepository:**
   - Adicionado método `ObterPorViagemAsync(int viagemId)` - busca participantes de uma viagem
   - Adicionado método `ObterPorParticipanteAsync(int participanteId)` - busca viagens de um participante
   - Adicionado método `ObterPorViagemEParticipanteAsync(int viagemId, int participanteId)` - busca relacionamento específico

2. **ViagemRepository:**
   - Todas as queries agora incluem a lista de participantes via JOIN
   - Melhoria de performance utilizando `AsNoTracking()`

### Validações

Todas as requests possuem validações usando FluentValidation:
- Validação de campos obrigatórios
- Validação de tamanhos máximo e mínimo
- Validação de datas
- Validação de IDs positivos

### Segurança

Todas as operações de edição/exclusão verificam:
- Se o recurso existe
- Se o usuário tem permissão para realizar a operação (é o criador da viagem)

## Endpoints Disponíveis

### Consulta de Viagens
- `GET /viagens/proximas?usuarioId={id}` - Lista viagens futuras
- `GET /viagens/historico?usuarioId={id}` - Lista viagens passadas
- `GET /viagens?usuarioId={id}&nome={nome}&destino={destino}&dataInicio={data}&dataFim={data}` - Busca/filtro
- `GET /viagens/{id}?usuarioId={id}` - Detalhes de uma viagem
- `GET /viagens/usuario/{usuarioId}` - Todas as viagens do usuário

### Gerenciamento de Viagens
- `POST /viagens` - Criar nova viagem
- `PUT /viagens/{id}` - Atualizar viagem existente

### Gerenciamento de Participantes
- `POST /viagens/participantes` - Adicionar participante
- `DELETE /viagens/{viagemId}/participantes/{participanteId}?usuarioId={id}` - Remover participante

## Documentação

Consulte o arquivo `ENDPOINTS_VIAGENS.md` para documentação detalhada de todos os endpoints, incluindo:
- Parâmetros de request
- Exemplos de request/response
- Códigos de status HTTP
- Mensagens de erro

## Status dos Enums

### StatusViagemEnum
- `1` - Planejada
- `2` - EmAndamento
- `3` - Finalizada
- `4` - Cancelada

### StatusViagemParticipanteEnum
- `1` - Pendente
- `2` - Aceito
- `3` - Recusado

## Testes

Para testar os endpoints:
1. Execute o projeto com `dotnet run`
2. Acesse o Swagger em `http://localhost:5005/swagger`
3. Use os dados de seed fornecidos em `seed_data.sql`

## Próximos Passos (Recomendações)

1. **Implementar Carro Alugado e Bagagem:**
   - Criar modelagem de dados
   - Criar migrations
   - Implementar endpoints CRUD
   - Adicionar upload de fotos

2. **Testes Automatizados:**
   - Criar testes unitários para handlers
   - Criar testes de integração para endpoints
   - Criar testes para validações

3. **Melhorias de Segurança:**
   - Implementar autenticação JWT
   - Implementar autorização baseada em roles
   - Rate limiting

4. **Frontend (travel_doc_app):**
   - Integrar com os novos endpoints
   - Implementar tela de edição de viagem
   - Implementar gerenciamento de participantes
   - Implementar upload de fotos
