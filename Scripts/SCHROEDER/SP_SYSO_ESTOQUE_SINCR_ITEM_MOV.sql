CREATE OR ALTER PROCEDURE SP_SYSO_ESTOQUE_SINCR_ITEM_MOV (
    ID_MOVIMENTO INTEGER,
    ID_PRODUTO VARCHAR(15),
    QUANTIDADE INTEGER,
    IDENTIFICADOR_OS VARCHAR(40),
    ID_EXECUTOR INTEGER,
    ID_VINCULACAO INTEGER,
    ID_ESTOQUISTA INTEGER,
    NOME_PRODUTO varchar (100),
    CODIGO_FABRICANTE VARCHAR (20)
) 
AS
DECLARE VARIABLE SEQ_USUARIO INTEGER;
BEGIN
    SELECT COUNT(*)
    FROM MOVIMENTO_USUARIOS
    WHERE ID_MOVIMENTO = :ID_MOVIMENTO
    INTO :SEQ_USUARIO;

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
    SELECT
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
    FROM RDB$DATABASE
    WHERE EXISTS (
        SELECT 1
        FROM MOVIMENTO_ITENS
        WHERE
            ID_MOVIMENTO = :ID_MOVIMENTO AND
            ID_PRODUTO = :ID_PRODUTO AND
            QUANT_INTEIRA = :QUANTIDADE
    );

    UPDATE MOVIMENTO_ITENS
    SET posicao_estoque = 2, id_estoquista = :ID_ESTOQUISTA
    WHERE id_movimento = :ID_MOVIMENTO AND
    id_produto = :ID_PRODUTO AND
    EXISTS (
            SELECT 1
            FROM MOVIMENTO_ITENS
            WHERE
                ID_MOVIMENTO= :ID_MOVIMENTO AND
                ID_PRODUTO = :ID_PRODUTO AND
                QUANT_INTEIRA = :QUANTIDADE
        );

    UPDATE MOVIMENTO_OS
    SET ID_ADICIONAL_OS= :IDENTIFICADOR_OS
    WHERE ID_MOVIMENTO = :ID_MOVIMENTO AND
    ID_ADICIONAL_OS IS NULL AND
    EXISTS (
            SELECT 1
            FROM MOVIMENTO_ITENS
            WHERE
                ID_MOVIMENTO= :ID_MOVIMENTO AND
                ID_PRODUTO = :ID_PRODUTO AND
                QUANT_INTEIRA = :QUANTIDADE
        );

    UPDATE SYSO_ESTOQUE_CARRINHO
    SET
        ID_MOVIMENTO = :ID_MOVIMENTO,
        SITUACAO = 2,
        DATAHORA_ULT_OPERACAO = CURRENT_TIMESTAMP,
        ID_USUARIO_ULT_OPERACAO = :ID_ESTOQUISTA
    WHERE
        ID_PRODUTO = :ID_PRODUTO AND
        IDENTIFICADOR_OS = :IDENTIFICADOR_OS AND
        EXISTS (
            SELECT 1
            FROM MOVIMENTO_ITENS
            WHERE
                ID_MOVIMENTO= :ID_MOVIMENTO AND
                ID_PRODUTO = :ID_PRODUTO AND
                QUANT_INTEIRA = :QUANTIDADE
        );

    INSERT INTO MOVIMENTO_USUARIOS (
            ID_EMPRESA,
            ID_FILIAL,
            ID_MOVIMENTO,
            SEQUENCIA_USUARIO,
            ID_USUARIO,
            OPERACAO,
            DETALHES,
            INTERNO,
            DATA_OPERACAO
        )
        VALUES (
            1,
            1,
            :ID_MOVIMENTO,
            :SEQ_USUARIO + 1,
            :ID_VINCULACAO,
            999,
            '(GESTOR) - SINCRONIZAÇÃO DO ITEM: ' || FORMATDATETIME('dd/mm/yyyy HH:MM:SS', CURRENT_TIMESTAMP) || ' ITEM: ' || :CODIGO_FABRICANTE || '-' || :NOME_PRODUTO,
            'S',
            CURRENT_TIMESTAMP
        );
END;

