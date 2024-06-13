create or alter procedure SP_SYSO_ESTOQUE_ADD_ITEM_MOV (
    ID_EMPRESA integer,
    ID_FILIAL integer,
    ID_MOVIMENTO integer,
    ID_PRODUTO varchar(13),
    ID_DESCRICAO integer,
    ID_GRUPO_PRODUTOS integer,
    ID_SUB_GRUPO_PRODUTOS integer,
    FRACAO double precision,
    QUANT_INTEIRA integer,
    QUANT_FRACAO integer,
    TOTAL_LIQUIDO double precision,
    POSICAO_ESTOQUE integer,
    ESTOQUE_INTEIRO integer,
    ESTOQUE_FRACAO integer,
    PRECO_CADASTRO double precision,
    ID_ESTOQUISTA integer,
    ID_EXECUTOR integer,
    FATOR_CONV_QTDE double precision,
    PRODUTO_SERVICO integer,
    SITUACAO_ESTOQUE smallint,
    UNIDADE varchar(6),
    IDENTIFICADOR_OS varchar(20)
    )
returns (RESULT_FLAG INTEGER)
as
declare variable ICOUNT integer;
declare variable ID_SOLICITANTE integer;
declare variable NOVO_ITEM integer;
BEGIN

    SELECT COUNT(*) FROM MOVIMENTO_ITENS MI
    WHERE (ID_EMPRESA = :ID_EMPRESA)
        AND (ID_FILIAL = :ID_FILIAL)
        AND (ID_MOVIMENTO = :ID_MOVIMENTO)
        AND (ID_PRODUTO = :ID_PRODUTO)
    INTO :ICOUNT;

    SELECT FIRST 1 ID_SOLICITANTE
    FROM MOVIMENTO_ITENS MI
    WHERE ID_MOVIMENTO = :ID_MOVIMENTO
    INTO :ID_SOLICITANTE;

    SELECT COUNT(*)
    FROM MOVIMENTO_ITENS
    WHERE ID_MOVIMENTO = :ID_MOVIMENTO
    INTO :NOVO_ITEM;

    BEGIN
    IF (:ICOUNT = 0) THEN
        INSERT INTO MOVIMENTO_ITENS(
            ID_EMPRESA,
            ID_FILIAL,
            ID_MOVIMENTO,
            ID_PACOTE,
            ID_PRODUTO,
            ID_PRODUTO_COMPOSTO,
            ITEM,
            ID_DESCRICAO,
            ID_GRUPO_PRODUTOS,
            ID_SUB_GRUPO_PRODUTOS,
            PRECO_CADASTRO,
            ESTOQUE_INTEIRO,
            ESTOQUE_FRACAO,
            CUSTO_CADASTRO,
            CUSTO_MEDIO_CADASTRO,
            PRECO_UNITARIO,
            FRACAO,
            QUANT_INTEIRA,
            QUANT_FRACAO,
            TOTAL_BRUTO,
            PERCENT_DESCONTO_OPERACAO,
            PERCENT_DESCONTO_PRODUTO,
            VALOR_DESCONTO_COMPL,
            TOTAL_LIQUIDO,
            PRODUTO_SERVICO,
            SITUACAO_ESTOQUE,
            DATA_MOVIMENTACAO_ESTOQUE,
            VALOR_DESC_ITEM,
            VALOR_PROM_ITEM,
            ID_ESTOQUISTA,
            ID_SOLICITANTE,
            DATA_ULT_OPERACAO,
            POSICAO_ESTOQUE,
            FATOR_CONV_QTDE,
            UNIDADE
        )
        VALUES(
            :ID_EMPRESA,
            :ID_FILIAL,
            :ID_MOVIMENTO,
            1, -- ID_PACOTE
            :ID_PRODUTO,
            :ID_PRODUTO,
            :NOVO_ITEM + 1,
            :ID_DESCRICAO,
            :ID_GRUPO_PRODUTOS,
            :ID_SUB_GRUPO_PRODUTOS,
            :PRECO_CADASTRO,
            :ESTOQUE_INTEIRO,
            :ESTOQUE_FRACAO,
            0,
            0,
            :PRECO_CADASTRO,
            :FRACAO,
            :QUANT_INTEIRA,
            :QUANT_FRACAO,
            :TOTAL_LIQUIDO,
            0,
            0,
            0,
            :TOTAL_LIQUIDO,
            :PRODUTO_SERVICO,
            :SITUACAO_ESTOQUE,
            CURRENT_TIMESTAMP,
            0,
            0,
            :ID_ESTOQUISTA,
            :ID_SOLICITANTE,
            CURRENT_TIMESTAMP,
            :POSICAO_ESTOQUE,
            :FATOR_CONV_QTDE,
            :UNIDADE
        );
    
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
            :ID_EMPRESA,
            :ID_FILIAL,
            :ID_MOVIMENTO,
            1,
            :ID_PRODUTO,
            :ID_PRODUTO,
            :ID_SOLICITANTE,
            1,
            0,
            null,
            2,
            null
        FROM RDB$DATABASE
        WHERE EXISTS (
            SELECT 1
            FROM MOVIMENTO_ITENS
            WHERE
                ID_MOVIMENTO = :ID_MOVIMENTO AND
                ID_PRODUTO = :ID_PRODUTO AND
                QUANT_INTEIRA = :QUANT_INTEIRA
        );
    
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
            :ID_EMPRESA,
            :ID_FILIAL,
            :ID_MOVIMENTO,
            1,
            :ID_PRODUTO,
            :ID_PRODUTO,
            :ID_EXECUTOR,
            2,
            0,
            null,
            2,
            null
        FROM RDB$DATABASE
        WHERE EXISTS (
            SELECT 1
            FROM MOVIMENTO_ITENS
            WHERE
                ID_MOVIMENTO = :ID_MOVIMENTO AND
                ID_PRODUTO = :ID_PRODUTO AND
                QUANT_INTEIRA = :QUANT_INTEIRA
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
                    QUANT_INTEIRA = :QUANT_INTEIRA
        );
    
        UPDATE MOVIMENTO_OS
        SET ID_ADICIONAL_OS = :IDENTIFICADOR_OS
        WHERE ID_MOVIMENTO = :ID_MOVIMENTO AND
        ID_ADICIONAL_OS = '' AND
        EXISTS (
                SELECT 1
                FROM MOVIMENTO_ITENS
                WHERE
                    ID_MOVIMENTO= :ID_MOVIMENTO AND
                    ID_PRODUTO = :ID_PRODUTO AND
                    QUANT_INTEIRA = :QUANT_INTEIRA
            );
    
        UPDATE MOVIMENTO
        SET DATA_OPERACAO = current_timestamp,
        TOTAL_LIQUIDO = (SELECT SUM(TOTAL_LIQUIDO)
            FROM MOVIMENTO_ITENS 
            WHERE ID_MOVIMENTO = :ID_MOVIMENTO),
        VALOR_REAL = (SELECT SUM(TOTAL_LIQUIDO)
            FROM MOVIMENTO_ITENS 
            WHERE ID_MOVIMENTO = :ID_MOVIMENTO)
        WHERE ID_MOVIMENTO = :ID_MOVIMENTO AND
        EXISTS (
                SELECT 1
                FROM MOVIMENTO_ITENS
                WHERE
                    ID_MOVIMENTO= :ID_MOVIMENTO AND
                    ID_PRODUTO = :ID_PRODUTO AND
                    QUANT_INTEIRA = :QUANT_INTEIRA
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
                    QUANT_INTEIRA = :QUANT_INTEIRA
            );
     END

     RESULT_FLAG = 1;
END;

