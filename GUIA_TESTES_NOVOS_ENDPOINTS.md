# Guia de Testes Manuais - Novos Endpoints

Este documento descreve como testar manualmente os novos endpoints implementados.

## Pré-requisitos

1. Banco de dados PostgreSQL configurado e rodando
2. Executar migrations: `dotnet ef database update`
3. Popular banco com dados de teste usando `seed_data.sql`
4. Executar a aplicação: `dotnet run` na pasta TravelDoc.Api

## Endpoints Implementados

### 1. DELETE /viagens/{id} - Excluir Viagem

**Descrição:** Exclui uma viagem existente (exclusão física).

**Teste 1: Exclusão bem-sucedida**
```bash
# Requisição
DELETE http://localhost:5005/viagens/1?usuarioId=1

# Resposta esperada: 200 OK
{}
```

**Teste 2: Viagem não encontrada**
```bash
# Requisição
DELETE http://localhost:5005/viagens/999?usuarioId=1

# Resposta esperada: 400 Bad Request
{
  "mensagem": "Viagem não encontrada!"
}
```

**Teste 3: Usuário sem permissão**
```bash
# Requisição (tentando deletar viagem de outro usuário)
DELETE http://localhost:5005/viagens/1?usuarioId=2

# Resposta esperada: 400 Bad Request
{
  "mensagem": "Você não tem permissão para excluir esta viagem!"
}
```

**Teste 4: Validação - ID inválido**
```bash
# Requisição
DELETE http://localhost:5005/viagens/0?usuarioId=1

# Resposta esperada: 400 Bad Request
# Mensagem de validação sobre ID obrigatório
```

---

### 2. POST /viagens/{tripId}/invite - Convidar Participante

**Descrição:** Convida um participante para a viagem através do email.

**Teste 1: Convite bem-sucedido**
```bash
# Requisição
POST http://localhost:5005/viagens/1/invite
Content-Type: application/json

{
  "email": "convidado@email.com"
}

# Resposta esperada: 200 OK
{}

# Verificar no banco:
# SELECT * FROM viagem_participante_tb WHERE viagem_id = 1;
# Deve ter um novo registro com status = 1 (Pendente)
```

**Teste 2: Usuário não cadastrado**
```bash
# Requisição
POST http://localhost:5005/viagens/1/invite
Content-Type: application/json

{
  "email": "naoexiste@email.com"
}

# Resposta esperada: 400 Bad Request
{
  "mensagem": "Convidado não possui cadastro no TravelDoc"
}
```

**Teste 3: Viagem não encontrada**
```bash
# Requisição
POST http://localhost:5005/viagens/999/invite
Content-Type: application/json

{
  "email": "convidado@email.com"
}

# Resposta esperada: 400 Bad Request
{
  "mensagem": "Viagem não encontrada!"
}
```

**Teste 4: Validação - Email inválido**
```bash
# Requisição
POST http://localhost:5005/viagens/1/invite
Content-Type: application/json

{
  "email": "email-invalido"
}

# Resposta esperada: 400 Bad Request
# Mensagem de validação sobre email inválido
```

**Teste 5: Validação - Email vazio**
```bash
# Requisição
POST http://localhost:5005/viagens/1/invite
Content-Type: application/json

{
  "email": ""
}

# Resposta esperada: 400 Bad Request
# Mensagem de validação sobre email obrigatório
```

---

## Verificação via Swagger

1. Acesse: `http://localhost:5005/swagger`
2. Localize os endpoints:
   - `DELETE /viagens/{id}`
   - `POST /viagens/{tripId}/invite`
3. Use a interface do Swagger para testar os endpoints

---

## Testes de Integração com Endpoints Existentes

### Fluxo Completo de Gerenciamento de Viagem

**Cenário:** Criar, atualizar, convidar participante, e excluir uma viagem

```bash
# 1. Criar viagem
POST http://localhost:5005/viagens
Content-Type: application/json

{
  "usuarioId": 1,
  "nomeViagem": "Teste de Viagem",
  "destino": "São Paulo",
  "dataInicio": "2024-12-01",
  "dataFim": "2024-12-10",
  "descricao": "Viagem de teste"
}

# 2. Atualizar viagem (use o ID retornado no passo 1)
PUT http://localhost:5005/viagens/{id}
Content-Type: application/json

{
  "usuarioId": 1,
  "nomeViagem": "Teste de Viagem - Atualizado",
  "destino": "São Paulo",
  "dataInicio": "2024-12-01",
  "dataFim": "2024-12-15",
  "descricao": "Viagem de teste atualizada"
}

# 3. Convidar participante (novo endpoint)
POST http://localhost:5005/viagens/{id}/invite
Content-Type: application/json

{
  "email": "participante@email.com"
}

# 4. Verificar detalhes da viagem
GET http://localhost:5005/viagens/{id}?usuarioId=1

# 5. Excluir viagem (novo endpoint)
DELETE http://localhost:5005/viagens/{id}?usuarioId=1

# 6. Verificar que viagem foi excluída
GET http://localhost:5005/viagens/{id}?usuarioId=1
# Deve retornar 404 ou null
```

---

## Queries SQL para Verificação Manual

```sql
-- Verificar viagens
SELECT * FROM viagem_tb ORDER BY id DESC LIMIT 10;

-- Verificar participantes de uma viagem
SELECT vp.*, u.nome, u.email 
FROM viagem_participante_tb vp
JOIN usuario_tb u ON u.id = vp.participante_id
WHERE vp.viagem_id = 1;

-- Verificar se viagem foi excluída
SELECT * FROM viagem_tb WHERE id = 1;
```

---

## Checklist de Testes

### DELETE /viagens/{id}
- [ ] Exclusão bem-sucedida
- [ ] Viagem não encontrada
- [ ] Usuário sem permissão
- [ ] Validação de ID inválido
- [ ] Validação de usuarioId inválido

### POST /viagens/{tripId}/invite
- [ ] Convite bem-sucedido
- [ ] Usuário não cadastrado
- [ ] Viagem não encontrada
- [ ] Validação de email inválido
- [ ] Validação de email vazio
- [ ] Validação de tripId inválido

### Testes de Integração
- [ ] Fluxo completo: criar → atualizar → convidar → excluir
- [ ] Verificar persistência no banco de dados
- [ ] Verificar que apenas o criador pode executar ações

---

## Notas Importantes

1. **Mock de Notificação/Email:** O endpoint POST /viagens/{tripId}/invite contém um comentário TODO para implementar o serviço de notificação/email real. Atualmente, apenas cria o convite no banco.

2. **Exclusão Física:** O endpoint DELETE /viagens/{id} realiza exclusão física (remove do banco). Se houver necessidade de exclusão lógica, será necessário adicionar um campo `deleted_at` ou `active` na tabela.

3. **Autorização:** Todos os endpoints verificam se o usuário é o criador da viagem antes de permitir a operação.

4. **Status do Convite:** O convite criado via POST /viagens/{tripId}/invite sempre tem status Pendente (1).

---

## Ferramentas Recomendadas

- **Postman:** Para testes de API com coleções organizadas
- **curl:** Para testes rápidos via linha de comando
- **Swagger UI:** Interface já incluída na aplicação
- **pgAdmin/DBeaver:** Para verificação manual no banco de dados
