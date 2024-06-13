CREATE OR ALTER PROCEDURE SP_SYSO_ESTOQUE_CONF_ITEM_MOV (
    ID_MOVIMENTO INTEGER,
    ID_PRODUTO VARCHAR(20),
    ID_EXECUTOR INTEGER,
    ID_ESTOQUISTA INTEGER)
AS
DECLARE VARIABLE COUNT_OPERACAO INTEGER;
DECLARE VARIABLE SITUACAO INTEGER;
BEGIN
    SELECT SITUACAO
    FROM MOVIMENTO
    WHERE ID_MOVIMENTO = :ID_MOVIMENTO
        AND tipo_movimento IN (1,2)
    INTO :SITUACAO;

    SELECT COUNT(*)
    FROM MOVIMENTO_ITENS_USUARIOS
    WHERE id_movimento = :ID_MOVIMENTO
        AND ID_PRODUTO = :ID_PRODUTO
        AND operacao = 2
    INTO :COUNT_OPERACAO;

    if (SITUACAO = 1 or SITUACAO = 4) then
    begin

    IF (COUNT_OPERACAO = 0) THEN
    BEGIN

    INSERT INTO MOVIMENTO_ITENS_USUARIOS(
        id_empresa,
        id_filial,
        id_movimento,
        id_pacote,
        id_produto,
        id_produto_composto,
        id_usuario,
        operacao,
        pontos,
        datapagamento,
        comissaopaga,
        tempo
    )
    values(
        1,
        1,
        :ID_MOVIMENTO,
        1,
        :ID_PRODUTO,
        :ID_PRODUTO,
        :ID_EXECUTOR,
        2,
        0,
        null,
        null,
        null
    );
    END

    UPDATE MOVIMENTO_ITENS
    SET posicao_estoque = 2,
        id_estoquista = :ID_ESTOQUISTA,
        data_ult_operacao = current_timestamp
    WHERE id_movimento = :ID_MOVIMENTO
        AND id_produto = :ID_PRODUTO;
    END
END;
