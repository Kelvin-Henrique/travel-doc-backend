# Travel Doc API - Endpoints de Viagens

## Novos Endpoints Implementados

### 1. GET /viagens/proximas
Lista viagens futuras (data de início >= data atual) do usuário autenticado.

**Parâmetros de Query:**
- `usuarioId` (int, obrigatório): ID do usuário

**Resposta de Sucesso (200):**
```json
[
  {
    "id": 1,
    "nomeViagem": "Férias em Paris",
    "destino": "Paris, França",
    "dataInicio": "2024-12-15T00:00:00Z",
    "dataFim": "2024-12-25T00:00:00Z",
    "descricao": "Viagem de férias para conhecer Paris",
    "criadorId": 1,
    "status": 1
  }
]
```

### 2. GET /viagens/historico
Lista viagens já realizadas (data de término < data atual) do usuário autenticado.

**Parâmetros de Query:**
- `usuarioId` (int, obrigatório): ID do usuário

**Resposta de Sucesso (200):**
```json
[
  {
    "id": 2,
    "nomeViagem": "Viagem a Lisboa",
    "destino": "Lisboa, Portugal",
    "dataInicio": "2024-01-10T00:00:00Z",
    "dataFim": "2024-01-20T00:00:00Z",
    "descricao": "Viagem de trabalho",
    "criadorId": 1,
    "status": 3
  }
]
```

### 3. GET /viagens
Busca e filtra viagens do usuário.

**Parâmetros de Query:**
- `usuarioId` (int, obrigatório): ID do usuário
- `nome` (string, opcional): Filtro por nome da viagem
- `destino` (string, opcional): Filtro por destino
- `dataInicio` (datetime, opcional): Filtro por data de início (>=)
- `dataFim` (datetime, opcional): Filtro por data de fim (<=)

**Exemplo:**
```
GET /viagens?usuarioId=1&destino=Paris
```

**Resposta de Sucesso (200):**
```json
[
  {
    "id": 1,
    "nomeViagem": "Férias em Paris",
    "destino": "Paris, França",
    "dataInicio": "2024-12-15T00:00:00Z",
    "dataFim": "2024-12-25T00:00:00Z",
    "descricao": "Viagem de férias para conhecer Paris",
    "criadorId": 1,
    "status": 1
  }
]
```

### 4. GET /viagens/{id}
Retorna detalhes de uma viagem específica.

**Parâmetros de Rota:**
- `id` (int, obrigatório): ID da viagem

**Parâmetros de Query:**
- `usuarioId` (int, obrigatório): ID do usuário

**Resposta de Sucesso (200):**
```json
{
  "id": 1,
  "nomeViagem": "Férias em Paris",
  "destino": "Paris, França",
  "dataInicio": "2024-12-15T00:00:00Z",
  "dataFim": "2024-12-25T00:00:00Z",
  "descricao": "Viagem de férias para conhecer Paris",
  "criadorId": 1,
  "status": 1
}
```

**Resposta de Erro (404):**
```json
{
  "mensagem": "Viagem não encontrada ou você não tem permissão para visualizá-la."
}
```

## Status de Viagem (StatusViagemEnum)

- `1` - Planejada
- `2` - EmAndamento
- `3` - Finalizada
- `4` - Cancelada

## Dados de Teste

Para testar os endpoints, você pode executar os seguintes comandos SQL após as migrações:

```sql
-- Inserir usuário de teste (se não existir)
INSERT INTO usuario_tb (nome, email, cpf, telefone, tipo, data_inclusao)
VALUES ('João Silva', 'joao.silva@email.com', '12345678901', '11999999999', 1, NOW())
ON CONFLICT DO NOTHING;

-- Inserir viagens de teste
-- Viagem futura (próximas viagens)
INSERT INTO viagem_tb (nome_viagem, destino, data_inicio, data_fim, descricao, criador_id, status, data_inclusao)
VALUES 
('Férias em Paris', 'Paris, França', '2024-12-15', '2024-12-25', 'Viagem de férias para conhecer Paris', 1, 1, NOW()),
('Viagem de Ano Novo', 'Rio de Janeiro, Brasil', '2024-12-30', '2025-01-05', 'Passar o ano novo no Rio', 1, 1, NOW());

-- Viagem passada (histórico)
INSERT INTO viagem_tb (nome_viagem, destino, data_inicio, data_fim, descricao, criador_id, status, data_inclusao)
VALUES 
('Viagem a Lisboa', 'Lisboa, Portugal', '2024-01-10', '2024-01-20', 'Viagem de trabalho', 1, 3, NOW()),
('Conferência em São Paulo', 'São Paulo, Brasil', '2024-03-05', '2024-03-08', 'Participação em conferência de tecnologia', 1, 3, NOW());
```

## Endpoint Existente (Mantido)

### GET /viagens/usuario/{usuarioId}
Lista todas as viagens de um usuário (sem filtro por data).

**Parâmetros de Rota:**
- `usuarioId` (int, obrigatório): ID do usuário

**Resposta de Sucesso (200):**
```json
[
  {
    "id": 1,
    "nomeViagem": "Férias em Paris",
    "destino": "Paris, França",
    "dataInicio": "2024-12-15T00:00:00Z",
    "dataFim": "2024-12-25T00:00:00Z",
    "descricao": "Viagem de férias para conhecer Paris",
    "criadorId": 1,
    "status": 1
  },
  {
    "id": 2,
    "nomeViagem": "Viagem a Lisboa",
    "destino": "Lisboa, Portugal",
    "dataInicio": "2024-01-10T00:00:00Z",
    "dataFim": "2024-01-20T00:00:00Z",
    "descricao": "Viagem de trabalho",
    "criadorId": 1,
    "status": 3
  }
]
```

## Endpoint de Cadastro (Existente)

### POST /viagens
Cria uma nova viagem.

**Request Body:**
```json
{
  "usuarioId": 1,
  "nomeViagem": "Férias em Paris",
  "destino": "Paris, França",
  "dataInicio": "2024-12-15T00:00:00Z",
  "dataFim": "2024-12-25T00:00:00Z",
  "descricao": "Viagem de férias para conhecer Paris"
}
```

**Resposta de Sucesso (200):**
```json
{}
```

**Resposta de Erro (400):**
```json
{
  "mensagem": "Mensagem de erro descrevendo o problema"
}
```

## Observações

- Todos os endpoints retornam as viagens filtradas por `usuarioId`, garantindo que cada usuário veja apenas suas próprias viagens
- As datas são armazenadas em UTC no banco de dados
- O campo `status` é retornado como um número inteiro (enum)
- Os endpoints de `proximas` e `historico` ordenam as viagens por data (próximas em ordem ascendente, histórico em ordem descendente)
