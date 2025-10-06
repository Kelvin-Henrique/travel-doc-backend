-- Seed data for testing Travel Doc API endpoints
-- Execute este script após rodar as migrações do banco de dados

-- Limpar dados de teste existentes (opcional)
-- DELETE FROM viagem_tb WHERE criador_id IN (SELECT id FROM usuario_tb WHERE email = 'joao.silva@email.com');
-- DELETE FROM usuario_tb WHERE email = 'joao.silva@email.com';

-- Inserir usuário de teste
-- Nota: O ID será gerado automaticamente pelo PostgreSQL
DO $$
DECLARE
    v_usuario_id INT;
    v_usuario_count INT;
BEGIN
    -- Verificar se usuário já existe
    SELECT COUNT(*) INTO v_usuario_count FROM usuario_tb WHERE email = 'joao.silva@email.com';
    
    -- Inserir usuário se não existir
    IF v_usuario_count = 0 THEN
        INSERT INTO usuario_tb (nome, email, cpf, telefone, tipo, data_inclusao)
        VALUES ('João Silva', 'joao.silva@email.com', '12345678901', '11999999999', 1, NOW() AT TIME ZONE 'UTC');
    END IF;

    -- Obter o ID do usuário
    SELECT id INTO v_usuario_id FROM usuario_tb WHERE email = 'joao.silva@email.com';

    -- Inserir viagens futuras (próximas viagens)
    INSERT INTO viagem_tb (nome_viagem, destino, data_inicio, data_fim, descricao, criador_id, status)
    VALUES 
    (
        'Férias em Paris', 
        'Paris, França', 
        (CURRENT_DATE + INTERVAL '30 days') AT TIME ZONE 'UTC', 
        (CURRENT_DATE + INTERVAL '40 days') AT TIME ZONE 'UTC', 
        'Viagem de férias para conhecer a Torre Eiffel e o Louvre', 
        v_usuario_id, 
        1
    ),
    (
        'Viagem de Ano Novo', 
        'Rio de Janeiro, Brasil', 
        (CURRENT_DATE + INTERVAL '60 days') AT TIME ZONE 'UTC', 
        (CURRENT_DATE + INTERVAL '67 days') AT TIME ZONE 'UTC', 
        'Passar o reveillon na praia de Copacabana', 
        v_usuario_id, 
        1
    ),
    (
        'Conferência em Barcelona', 
        'Barcelona, Espanha', 
        (CURRENT_DATE + INTERVAL '15 days') AT TIME ZONE 'UTC', 
        (CURRENT_DATE + INTERVAL '18 days') AT TIME ZONE 'UTC', 
        'Participar da conferência internacional de tecnologia', 
        v_usuario_id, 
        1
    );

    -- Inserir viagens passadas (histórico)
    INSERT INTO viagem_tb (nome_viagem, destino, data_inicio, data_fim, descricao, criador_id, status)
    VALUES 
    (
        'Viagem a Lisboa', 
        'Lisboa, Portugal', 
        (CURRENT_DATE - INTERVAL '90 days') AT TIME ZONE 'UTC', 
        (CURRENT_DATE - INTERVAL '80 days') AT TIME ZONE 'UTC', 
        'Viagem de trabalho e conhecer a cidade histórica', 
        v_usuario_id, 
        3
    ),
    (
        'Conferência em São Paulo', 
        'São Paulo, Brasil', 
        (CURRENT_DATE - INTERVAL '60 days') AT TIME ZONE 'UTC', 
        (CURRENT_DATE - INTERVAL '57 days') AT TIME ZONE 'UTC', 
        'Participação em conferência de desenvolvimento de software', 
        v_usuario_id, 
        3
    ),
    (
        'Férias em Salvador', 
        'Salvador, Bahia', 
        (CURRENT_DATE - INTERVAL '120 days') AT TIME ZONE 'UTC', 
        (CURRENT_DATE - INTERVAL '110 days') AT TIME ZONE 'UTC', 
        'Viagem de férias para aproveitar as praias baianas', 
        v_usuario_id, 
        3
    );

    RAISE NOTICE 'Dados de teste inseridos com sucesso para o usuário ID: %', v_usuario_id;
END $$;
