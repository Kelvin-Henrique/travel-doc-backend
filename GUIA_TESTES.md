# Guia de Testes - Endpoints de Viagens

Este documento fornece exemplos de como testar os endpoints implementados.

## Pré-requisitos

1. Banco de dados PostgreSQL rodando (conforme `appsettings.Development.json`)
2. Aplicação rodando: `dotnet run` na pasta `TravelDoc.Api`
3. Dados de seed carregados (executar `seed_data.sql`)

## Endpoints de Consulta

### 1. Listar Próximas Viagens

```bash
curl -X GET "http://localhost:5005/viagens/proximas?usuarioId=1"
```

**Resposta Esperada:**
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
    "status": 1,
    "participantes": []
  }
]
```

### 2. Listar Histórico de Viagens

```bash
curl -X GET "http://localhost:5005/viagens/historico?usuarioId=1"
```

### 3. Buscar Viagens com Filtros

```bash
# Filtrar por destino
curl -X GET "http://localhost:5005/viagens?usuarioId=1&destino=Paris"

# Filtrar por nome
curl -X GET "http://localhost:5005/viagens?usuarioId=1&nome=Férias"

# Filtrar por data
curl -X GET "http://localhost:5005/viagens?usuarioId=1&dataInicio=2024-12-01&dataFim=2024-12-31"
```

### 4. Obter Detalhes de uma Viagem

```bash
curl -X GET "http://localhost:5005/viagens/1?usuarioId=1"
```

## Endpoints de Modificação

### 5. Criar Nova Viagem

```bash
curl -X POST "http://localhost:5005/viagens" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": 1,
    "nomeViagem": "Viagem de Teste",
    "destino": "São Paulo, Brasil",
    "dataInicio": "2024-12-20T00:00:00Z",
    "dataFim": "2024-12-25T00:00:00Z",
    "descricao": "Viagem criada para teste"
  }'
```

**Resposta de Sucesso:** `200 OK` com body vazio `{}`

**Resposta de Erro:** `400 Bad Request`
```json
{
  "mensagem": "O nome da viagem deve ser informado!"
}
```

### 6. Atualizar Viagem Existente

```bash
curl -X PUT "http://localhost:5005/viagens/1" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": 1,
    "nomeViagem": "Férias em Paris - Atualizado",
    "destino": "Paris, França",
    "dataInicio": "2024-12-15T00:00:00Z",
    "dataFim": "2024-12-30T00:00:00Z",
    "descricao": "Viagem de férias atualizada",
    "status": 1
  }'
```

**Resposta de Sucesso:** `200 OK` com body vazio `{}`

**Resposta de Erro (sem permissão):** `400 Bad Request`
```json
{
  "mensagem": "Você não tem permissão para editar esta viagem!"
}
```

### 7. Adicionar Participante à Viagem

```bash
curl -X POST "http://localhost:5005/viagens/participantes" \
  -H "Content-Type: application/json" \
  -d '{
    "viagemId": 1,
    "emailConvidado": "maria@email.com",
    "status": 1
  }'
```

**Resposta de Sucesso:** `200 OK` com body vazio `{}`

**Resposta de Erro:** `400 Bad Request`
```json
{
  "mensagem": "Convidado não possui no cadastro no TravelDoc"
}
```

### 8. Remover Participante da Viagem

```bash
curl -X DELETE "http://localhost:5005/viagens/1/participantes/2?usuarioId=1"
```

**Resposta de Sucesso:** `200 OK` com body vazio `{}`

**Resposta de Erro:** `400 Bad Request`
```json
{
  "mensagem": "Participante não encontrado nesta viagem!"
}
```

## Testes com Swagger

A aplicação está configurada com Swagger em ambiente de desenvolvimento.

1. Acesse: `http://localhost:5005/swagger`
2. Teste os endpoints diretamente pela interface

## Casos de Teste Recomendados

### Teste de Autorização

1. **Criar viagem como Usuário 1**
2. **Tentar atualizar como Usuário 2** → Deve retornar erro de permissão
3. **Tentar remover participante como Usuário 2** → Deve retornar erro de permissão

```bash
# Criar viagem como usuário 1
curl -X POST "http://localhost:5005/viagens" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": 1,
    "nomeViagem": "Teste Autorização",
    "destino": "São Paulo",
    "dataInicio": "2024-12-20T00:00:00Z",
    "dataFim": "2024-12-25T00:00:00Z"
  }'

# Tentar atualizar como usuário 2 (deve falhar)
curl -X PUT "http://localhost:5005/viagens/1" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": 2,
    "nomeViagem": "Tentando Hackear",
    "destino": "São Paulo",
    "dataInicio": "2024-12-20T00:00:00Z",
    "dataFim": "2024-12-25T00:00:00Z"
  }'
```

### Teste de Validação

```bash
# Nome vazio - deve falhar
curl -X POST "http://localhost:5005/viagens" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": 1,
    "nomeViagem": "",
    "destino": "São Paulo",
    "dataInicio": "2024-12-20T00:00:00Z",
    "dataFim": "2024-12-25T00:00:00Z"
  }'

# Data de início posterior à data de fim - deve falhar
curl -X POST "http://localhost:5005/viagens" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": 1,
    "nomeViagem": "Teste Data Inválida",
    "destino": "São Paulo",
    "dataInicio": "2024-12-25T00:00:00Z",
    "dataFim": "2024-12-20T00:00:00Z"
  }'
```

### Teste de Participantes

```bash
# 1. Criar viagem
curl -X POST "http://localhost:5005/viagens" \
  -H "Content-Type: application/json" \
  -d '{
    "usuarioId": 1,
    "nomeViagem": "Teste Participantes",
    "destino": "São Paulo",
    "dataInicio": "2024-12-20T00:00:00Z",
    "dataFim": "2024-12-25T00:00:00Z"
  }'

# 2. Adicionar participante (assumindo ID da viagem = 10)
curl -X POST "http://localhost:5005/viagens/participantes" \
  -H "Content-Type: application/json" \
  -d '{
    "viagemId": 10,
    "emailConvidado": "maria@email.com",
    "status": 1
  }'

# 3. Verificar que o participante aparece na consulta
curl -X GET "http://localhost:5005/viagens/10?usuarioId=1"

# 4. Remover participante (assumindo ID do participante = 2)
curl -X DELETE "http://localhost:5005/viagens/10/participantes/2?usuarioId=1"

# 5. Verificar que o participante foi removido
curl -X GET "http://localhost:5005/viagens/10?usuarioId=1"
```

## Resultados Esperados

### Sucesso (200 OK)
- Body vazio `{}` para POST, PUT, DELETE
- Array de objetos para GET (pode ser vazio `[]`)
- Objeto único para GET de detalhes

### Erro de Validação (400 Bad Request)
```json
{
  "mensagem": "Descrição do erro"
}
```

### Não Encontrado (404 Not Found)
```json
{
  "mensagem": "Viagem não encontrada ou você não tem permissão para visualizá-la."
}
```

## Troubleshooting

### Erro de Conexão com Banco de Dados
- Verifique se o PostgreSQL está rodando
- Verifique as configurações em `appsettings.Development.json`
- Execute as migrations: `dotnet ef database update`

### Erro 404 - Endpoint não encontrado
- Verifique se a aplicação está rodando
- Verifique se a porta está correta (5005)
- Verifique se os endpoints foram registrados no Program.cs

### Participante não encontrado
- Certifique-se de que o usuário com o email existe no banco
- Use o endpoint de cadastro de usuário primeiro se necessário

## Monitoramento

### Ver logs da aplicação
```bash
dotnet run --verbosity detailed
```

### Verificar se endpoints foram registrados
Ao iniciar a aplicação, verifique os logs para confirmar que os endpoints foram mapeados:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://0.0.0.0:5005
```
