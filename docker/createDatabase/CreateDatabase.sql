
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

CREATE TABLE IF NOT EXISTS produto_categoria (
    id SERIAL PRIMARY KEY,
    nome varchar(150) NOT NULL
);

CREATE TABLE IF NOT EXISTS produto (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    preco_unitario NUMERIC(10, 2) NOT NULL,
	categoria_id INTEGER not null REFERENCES produto_categoria(id)
);

CREATE TABLE IF NOT EXISTS venda (
    id SERIAL PRIMARY KEY,
    numero_venda VARCHAR(11) UNIQUE NOT NULL,
    data_venda TIMESTAMP NOT NULL DEFAULT NOW(),
    cliente_id INTEGER NOT NULL REFERENCES cliente(id),
    filial_id INTEGER NOT NULL REFERENCES filial(id),
    valor_total NUMERIC(12, 2) NOT NULL DEFAULT 0,
    is_cancelada BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS venda_itens (
    id SERIAL PRIMARY KEY,
    venda_id INTEGER NOT NULL REFERENCES venda(id) ON DELETE CASCADE,
    produto_id INTEGER NOT NULL REFERENCES produto(id),
    quantidade INTEGER NOT NULL CHECK (quantidade > 0 AND quantidade <= 20),
    preco_unitario NUMERIC(10, 2) NOT NULL,
    desconto NUMERIC(10, 2) NOT NULL DEFAULT 0,
    valor_total NUMERIC(12, 2) NOT NULL,
    is_cancelado BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS usuario_nome (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(20) NOT NULL,
	sobre_nome VARCHAR(20) NOT NULL
);

CREATE TABLE IF NOT EXISTS usuario_endereco ( 
    id SERIAL PRIMARY KEY,
    cidade VARCHAR(50) NOT NULL,
	logradouro VARCHAR(50) NOT NULL,
	numero integer not null,
	zipcode varchar(10) null,
	latitude varchar(15) not null,
	longitude varchar(15) not null
);

CREATE TABLE IF NOT EXISTS usuario (
    id SERIAL PRIMARY KEY,
    email VARCHAR(50) UNIQUE NOT NULL,
	phone VARCHAR(15) UNIQUE NOT NULL,
    senha VARCHAR(100) NOT NULL,
    perfil VARCHAR(20) NOT NULL CHECK (perfil IN ('cliente', 'gerente', 'administrador')),
    filial_id INTEGER NULL REFERENCES filial(id),
	nome_id INTEGER NULL REFERENCES usuario_nome(id),
	endereco_id INTEGER NULL REFERENCES usuario_endereco(id),
	status VARCHAR(20) NOT NULL CHECK (status IN ('ativo', 'inativo', 'suspenso'))
);

CREATE TABLE IF NOT EXISTS carro ( 
    id SERIAL PRIMARY KEY,
	cliente_id INTEGER NOT NULL REFERENCES cliente(id),
	data_criacao TIMESTAMP NOT NULL
);

CREATE TABLE IF NOT EXISTS carro_produtos ( 
    id SERIAL PRIMARY KEY,
	carro_id INTEGER NOT NULL REFERENCES carro(id),
	produto_id INTEGER NOT NULL REFERENCES produto(id),
	quantidade INTEGER NOT NULL
);

DO $$
BEGIN
    --INDICE produto 
	IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'idx_produto_categoria_id' AND n.nspname = 'public'
    ) THEN
        CREATE INDEX idx_produto_categoria_id ON produto(categoria_id);
    END IF;

	----INDICE vendas
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

	----INDICE venda_itens
    IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'idx_itens_venda_venda_id' AND n.nspname = 'public'
    ) THEN
        CREATE INDEX idx_itens_venda_venda_id ON venda_itens(venda_id);
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'idx_itens_venda_produto_id' AND n.nspname = 'public'
    ) THEN
        CREATE INDEX idx_itens_venda_produto_id ON venda_itens(produto_id);
    END IF;
		
	----INDICE usuario
	IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'idx_usuario_filial_id' AND n.nspname = 'public'
    ) THEN
        CREATE INDEX idx_usuario_filial_id ON usuario(filial_id);
    END IF;
	
	IF NOT EXISTS (
        SELECT 1 FROM pg_class c
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE c.relname = 'idx_usuario_endereco' AND n.nspname = 'public'
    ) THEN
        CREATE INDEX idx_usuario_endereco ON usuario(endereco_id);
    END IF;
	
END
$$;

---- CRIAR filial

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM filial WHERE nome = 'Sede'
    ) THEN
        INSERT INTO filial (nome) VALUES ('Sede');
    END IF;
END
$$;

---- CRIAR categoria de produto

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM produto_categoria WHERE nome = 'eletrônicos'
    ) THEN
        INSERT INTO produto_categoria (nome) VALUES ('eletrônicos');
    END IF;
END
$$;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM produto_categoria WHERE nome = 'vestuário'
    ) THEN
        INSERT INTO produto_categoria (nome) VALUES ('vestuário');
    END IF;
END
$$;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM produto_categoria WHERE nome = 'alimentos e bebidas'
    ) THEN
        INSERT INTO produto_categoria (nome) VALUES ('alimentos e bebidas');
    END IF;
END
$$;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM produto_categoria WHERE nome = 'casa e decoração'
    ) THEN
        INSERT INTO produto_categoria (nome) VALUES ('casa e decoração');
    END IF;
END
$$;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM produto_categoria WHERE nome = 'beleza e cuidados pessoais'
    ) THEN
        INSERT INTO produto_categoria (nome) VALUES ('beleza e cuidados pessoais');
    END IF;
END
$$;

---- CRIAR produto

DO $$
DECLARE
	cat_id INT;
BEGIN
    SELECT id INTO cat_id FROM produto_categoria WHERE nome = 'beleza e cuidados pessoais' LIMIT 1;

    IF NOT EXISTS (
        SELECT 1 FROM produto WHERE nome = 'batom avon'
    ) THEN
        INSERT INTO produto (nome, preco_unitario, categoria_id) VALUES ('batom avon', 35.00, cat_id);
    END IF;
END
$$;

DO $$
DECLARE
	cat_id INT;
BEGIN
    SELECT id INTO cat_id FROM produto_categoria WHERE nome = 'casa e decoração' LIMIT 1;

    IF NOT EXISTS (
        SELECT 1 FROM produto WHERE nome = 'lâmpada 120w'
    ) THEN
        INSERT INTO produto (nome, preco_unitario, categoria_id) VALUES ('lâmpada 120w', 10.00, cat_id);
    END IF;
END
$$;

DO $$
DECLARE
	cat_id INT;
BEGIN
    SELECT id INTO cat_id FROM produto_categoria WHERE nome = 'alimentos e bebidas' LIMIT 1;

    IF NOT EXISTS (
        SELECT 1 FROM produto WHERE nome = 'maçã'
    ) THEN
        INSERT INTO produto (nome, preco_unitario, categoria_id) VALUES ('maçã', 1.00, cat_id);
    END IF;
END
$$;

DO $$
DECLARE
	cat_id INT;
BEGIN
    SELECT id INTO cat_id FROM produto_categoria WHERE nome = 'vestuário' LIMIT 1;

    IF NOT EXISTS (
        SELECT 1 FROM produto WHERE nome = 'vestido'
    ) THEN
        INSERT INTO produto (nome, preco_unitario, categoria_id) VALUES ('vestido', 50.00, cat_id);
    END IF;
END
$$;

DO $$
DECLARE
	cat_id INT;
BEGIN
    SELECT id INTO cat_id FROM produto_categoria WHERE nome = 'eletrônicos' LIMIT 1;

    IF NOT EXISTS (
        SELECT 1 FROM produto WHERE nome = 'ventilador'
    ) THEN
        INSERT INTO produto (nome, preco_unitario, categoria_id) VALUES ('ventilador', 100.00, cat_id);
    END IF;
END
$$;

---- CRIAR clientes

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM cliente WHERE cpf = '19830713075'
    ) THEN
        INSERT INTO cliente (nome, cpf, telefone, email)
        VALUES ('Cliente Teste 1', '19830713075', '(11) 91234-5678', 'clienteteste1@teste.com');
    END IF;
END
$$;

---- CRIAR nome usuario

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM usuario_nome WHERE nome = 'twig'
    ) THEN
        INSERT INTO usuario_nome (nome, sobre_nome) VALUES ('twig', 'mesquita');
    END IF;
END
$$;

---- CRIAR endereco usuario

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM usuario_endereco WHERE zipcode = '69043790'
    ) THEN
        INSERT INTO usuario_endereco (cidade, logradouro, numero, zipcode, latitude, longitude) VALUES ('Manaus', 'Alameda São José', '26', '69043790', '-3.0746604', '-60.0415794');
    END IF;
END
$$;

---- CRIAR usuario

DO $$
DECLARE
	endereco_id INT;
    filial_id INT;
BEGIN
    SELECT id INTO endereco_id FROM usuario_endereco WHERE zipcode = '69043790' LIMIT 1;
    SELECT id INTO filial_id FROM filial WHERE nome = 'Sede' LIMIT 1;

    IF NOT EXISTS (
        SELECT 1 FROM usuario WHERE email = 'administrador@empresa.com'
    ) THEN
        INSERT INTO usuario (email, phone, senha, perfil, filial_id, endereco_id, status)
        VALUES ('administrador@empresa.com', '(92) 99999-9999', 'MTIzNDU2', 'administrador', filial_id, endereco_id, 'ativo'); --senha é 123456
    END IF;
END
$$;

