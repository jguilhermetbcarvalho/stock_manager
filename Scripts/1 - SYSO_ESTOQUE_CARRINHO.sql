-- Criação do Gerador (Generator)
CREATE SEQUENCE SEQ_SYSESTOQUECARRINHO_ID_PK;

-- Criação da Tabela
CREATE TABLE SYSO_ESTOQUE_CARRINHO (
    id_pk INTEGER NOT NULL PRIMARY KEY,
    datahora_criacao TIMESTAMP,
    id_movimento INTEGER,
    id_produto VARCHAR(15),
    codigo_fabricante VARCHAR(20),
    codigo_barras VARCHAR(15),
    descricao VARCHAR(40),
    quant_inteira INTEGER,
    quant_fracao INTEGER,
    fracao INTEGER,
    identificador_os VARCHAR(20),
    situacao INTEGER,
    id_usuario INTEGER,
    id_executor INTEGER,
    id_usuario_ult_operacao INTEGER,
    datahora_ult_operacao TIMESTAMP,
    localizacao VARCHAR(20)
);

-- Criação do Trigger
CREATE TRIGGER BI_SYSESTOQUECARRINHO_ID_PK
    FOR SYSO_ESTOQUE_CARRINHO
    ACTIVE BEFORE INSERT
AS
BEGIN
    IF (NEW.id_pk IS NULL) THEN
    BEGIN
        NEW.id_pk = GEN_ID(SEQ_SYSESTOQUECARRINHO_ID_PK, 1);
    END
END;

