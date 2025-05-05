
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT FROM pg_database WHERE datname = 'sistema_vendas'
    ) THEN
        EXECUTE 'CREATE DATABASE sistema_vendas';
    END IF;
END
$$;

CREATE TABLE IF NOT EXISTS cliente (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    cpf VARCHAR(11) UNIQUE NOT NULL CHECK (LENGTH(cpf) = 11),
    telefone VARCHAR(15) NOT NULL,
    email VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE IF NOT EXISTS filial (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS categoria_produto (
    id SERIAL PRIMARY KEY,
    nome varchar(150) NOT NULL
);

CREATE TABLE IF NOT EXISTS produto (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    preco_unitario NUMERIC(10, 2) NOT NULL,
	categoria_id INTEGER not null REFERENCES categoria_produto(id)
);

CREATE TABLE IF NOT EXISTS venda (
    id SERIAL PRIMARY KEY,
    numero_venda VARCHAR(50) UNIQUE NOT NULL,
    data_venda TIMESTAMP NOT NULL DEFAULT NOW(),
    cliente_id INTEGER NOT NULL REFERENCES clientes(id),
    filial_id INTEGER NOT NULL REFERENCES filiais(id),
    valor_total NUMERIC(12, 2) NOT NULL DEFAULT 0,
    cancelada BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS itens_venda (
    id SERIAL PRIMARY KEY,
    venda_id INTEGER NOT NULL REFERENCES vendas(id) ON DELETE CASCADE,
    produto_id INTEGER NOT NULL REFERENCES produtos(id),
    quantidade INTEGER NOT NULL CHECK (quantidade > 0 AND quantidade <= 20),
    preco_unitario NUMERIC(10, 2) NOT NULL,
    desconto NUMERIC(10, 2) NOT NULL DEFAULT 0,
    valor_total NUMERIC(12, 2) NOT NULL,
    cancelado BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS usuario (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(50) UNIQUE NOT NULL,
    senha VARCHAR(100) NOT NULL,
    perfil VARCHAR(20) NOT NULL CHECK (perfil IN ('admin', 'vendedor')),
    filial_id INTEGER NULL REFERENCES filiais(id),
    ativo BOOLEAN NOT NULL DEFAULT TRUE
);

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'idx_vendas_cliente_id' AND n.nspname = 'public'
    ) THEN
        CREATE INDEX idx_vendas_cliente_id ON venda(cliente_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'idx_vendas_filial_id' AND n.nspname = 'public'
    ) THEN
        CREATE INDEX idx_vendas_filial_id ON venda(filial_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'idx_itens_venda_venda_id' AND n.nspname = 'public'
    ) THEN
        CREATE INDEX idx_itens_venda_venda_id ON itens_venda(venda_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'idx_itens_venda_produto_id' AND n.nspname = 'public'
    ) THEN
        CREATE INDEX idx_itens_venda_produto_id ON itens_venda(produto_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM filiais WHERE nome = 'Sede'
    ) THEN
        INSERT INTO filiais (nome) VALUES ('Sede');
    END IF;
END
$$;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM filiais WHERE nome = 'Filial Central'
    ) THEN
        INSERT INTO filiais (nome) VALUES ('Filial Central');
    END IF;
END
$$;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM clientes WHERE cpf = '12345678901'
    ) THEN
        INSERT INTO clientes (nome, cpf, telefone, email)
        VALUES ('Cliente Exemplo', '12345678901', '(11)91234-5678', 'cliente@exemplo.com');
    END IF;
END
$$;

DO $$
DECLARE
    filial_id INT;
BEGIN
    SELECT id INTO filial_id FROM filiais WHERE nome = 'Filial Central' LIMIT 1;

    IF NOT EXISTS (
        SELECT 1 FROM usuario WHERE email = 'admin@empresa.com'
    ) THEN
        INSERT INTO usuario (nome, email, senha, perfil, filial_id)
        VALUES ('Administrador', 'admin@empresa.com', 'senhaadmin123', 'admin', filial_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM usuario WHERE email = 'vendedor@empresa.com'
    ) THEN
        INSERT INTO usuario (nome, email, senha, perfil, filial_id)
        VALUES ('Vendedor Exemplo', 'vendedor@empresa.com', 'senhavend123', 'vendedor', filial_id);
    END IF;
END
$$;

DO $$
DECLARE
    i INT := 1;
    produto_nome TEXT;
    preco NUMERIC(10,2);
BEGIN
    WHILE i <= 20 LOOP
        produto_nome := 'Produto ' || i;
        preco := ROUND((10 + random() * 90)::NUMERIC, 2);
        IF NOT EXISTS (
            SELECT 1 FROM produtos WHERE nome = produto_nome
        ) THEN
            INSERT INTO produtos (nome, preco_unitario)
            VALUES (produto_nome, preco);
        END IF;
        i := i + 1;
    END LOOP;
END
$$;
