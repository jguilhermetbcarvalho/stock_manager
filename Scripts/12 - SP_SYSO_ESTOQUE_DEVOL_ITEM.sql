create or alter procedure SP_SYSO_ESTOQUE_DEVOL_ITEM (
    ID_PRODUTO varchar(13),
    IDENTIFICADOR_OS varchar(20),
    ID_ESTOQUISTA integer,
    QUANTIDADE integer)
as
DECLARE VARIABLE COUNT_OPERACAO INTEGER;
BEGIN
    UPDATE SYSO_ESTOQUE_CARRINHO
    SET SITUACAO = 3,
        DATAHORA_ULT_OPERACAO = CURRENT_TIMESTAMP, 
        ID_USUARIO_ULT_OPERACAO = :ID_ESTOQUISTA
    WHERE ID_PRODUTO = :ID_PRODUTO
        AND IDENTIFICADOR_OS = :IDENTIFICADOR_OS
        AND QUANT_INTEIRA = :QUANTIDADE;
END;
