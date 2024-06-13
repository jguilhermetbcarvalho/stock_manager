CREATE OR ALTER PROCEDURE SP_SYSO_ESTOQUE_ADD_ITEM_CAR (
    ID_PRODUTO VARCHAR(15),
    DESCRICAO VARCHAR(40),
    CODIGO_BARRAS VARCHAR(15),
    CODIGO_FABRICANTE VARCHAR(20),
    QUANT_INTEIRA INTEGER,
    IDENTIFICADOR_OS VARCHAR(20),
    ID_EXECUTOR INTEGER,
    ID_USUARIO INTEGER,
    LOCALIZACAO VARCHAR(20)
)
returns (RESULT_FLAG INTEGER)
AS
declare variable ESTOQUE INTEGER;
BEGIN
    SELECT ESTOQUE_INTEIRO
    FROM PRODUTOS_ESTOQUES
    WHERE ID_PRODUTO = :ID_PRODUTO
    INTO :ESTOQUE;

    if (ESTOQUE >= QUANT_INTEIRA) then
    BEGIN
        INSERT INTO SYSO_ESTOQUE_CARRINHO(
            ID_PRODUTO,
            DESCRICAO,
            CODIGO_BARRAS,
            CODIGO_FABRICANTE,
            QUANT_INTEIRA,
            IDENTIFICADOR_OS,
            ID_EXECUTOR,
            ID_USUARIO,
            SITUACAO,
            DATAHORA_ULT_OPERACAO,
            DATAHORA_CRIACAO,
            LOCALIZACAO
        )
        VALUES (
            :ID_PRODUTO,
            :DESCRICAO,
            :CODIGO_BARRAS,
            :CODIGO_FABRICANTE,
            :QUANT_INTEIRA,
            :IDENTIFICADOR_OS,
            :ID_EXECUTOR,
            :ID_USUARIO,
            1,
            CURRENT_TIMESTAMP,
            CURRENT_TIMESTAMP,
            :LOCALIZACAO
        );
        RESULT_FLAG = 1;
    END
    else
    BEGIN
        RESULT_FLAG = 0; -- Ou qualquer valor que você queira atribuir para indicar que o item não foi adicionado
    END
END;

