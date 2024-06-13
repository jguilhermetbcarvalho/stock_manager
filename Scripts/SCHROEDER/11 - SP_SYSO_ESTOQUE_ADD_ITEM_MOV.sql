create or alter procedure SP_SYSO_ESTOQUE_ADD_ITEM_MOV (
    ID_EMPRESA integer,
    ID_FILIAL integer,
    ID_MOVIMENTO integer,
    ID_PRODUTO varchar(13),
    ID_DESCRICAO integer,
    QUANT_INTEIRA integer,
    ID_VINCULACAO integer,
    ID_EXECUTOR integer,
    IDENTIFICADOR_OS varchar(20),
    ID_ESTOQUISTA INTEGER,
    NOME_PRODUTO varchar (100),
    CODIGO_FABRICANTE VARCHAR (20)
    )
returns (RESULT_FLAG INTEGER)
as
declare variable ICOUNT integer;
declare variable ID_SOLICITANTE integer;
declare variable NOVO_ITEM integer;
DECLARE VARIABLE SEQ_USUARIO INTEGER;
BEGIN
    SELECT COUNT(*)
    FROM MOVIMENTO_USUARIOS
    WHERE ID_MOVIMENTO = :ID_MOVIMENTO
    INTO :SEQ_USUARIO;

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

    SELECT COUNT(ITEM)
    FROM MOVIMENTO_ITENS
    WHERE ID_EMPRESA = :ID_EMPRESA
        AND ID_FILIAL = :ID_FILIAL
        AND ID_MOVIMENTO = :ID_MOVIMENTO
    INTO :NOVO_ITEM;

    BEGIN
    IF (:ICOUNT = 0) THEN
        INSERT INTO MOVIMENTO_ITENS (
            ID_EMPRESA,
            ID_FILIAL,
            ID_MOVIMENTO,
            ID_PACOTE,
            ID_PRODUTO,
            ID_PRODUTO_COMPOSTO,
            ITEM,
            ID_DESCRICAO,
            COMPL_DESCRICAO_1,
            COMPL_DESCRICAO_2,
            TEXTO_COMPL_DESCRICAO,
            ID_GRUPO_PRODUTOS,
            ID_SUB_GRUPO_PRODUTOS,
            PRECO_CADASTRO,
            ESTOQUE_INTEIRO,
            ESTOQUE_FRACAO,
            COMPRA_CADASTRO,
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
            DATA_RESERVA,
            DATA_PREV_RET,
            DIARIAS_EFETIVAS,
            FATOR_CONV_QTDE,
            UNIDADE,
            PRECO_MERCADO_CADASTRO,
            IND_CANCELAMENTO,
            TOTALIZADOR_PARCIAL,
            ID_PREVENDA,
            ID_DAV_ORCAMENTO,
            ID_DAV_OS,
            ACRESCIMO,
            QTD_ENTREGA_INT,
            QTD_ENTREGA_FRA,
            CHAVE,
            NUM_PEDIDO_CLI,
            NUM_ITEM_PEDIDO_CLI,
            LIBERACAO_ESTOQUE,
            PERC_ALTM,
            VALOR_ALTM,
            MOTIVO_DEV_SNGPC,
            COD_ADICIONAL,
            TAXA_ENTREGA,
            PERC_ALT_MENOS,
            MULTIPLICADOR)
        SELECT
            :ID_EMPRESA,
            :ID_FILIAL,
            :ID_MOVIMENTO,
            1, -- ID_PACOTE
            pe.id_produto,
            pe.id_produto,
            :NOVO_ITEM + 1,
            1,
            NULL,
            NULL,
            NULL,
            pro.id_grupo_produtos,
            pro.id_sub_grupo_produtos,
            pp.preco_venda,
            pe.estoque_inteiro,
            pe.estoque_fracao,
            pp.preco_compra,
            pp.preco_custo,
            pp.custo_medio,
            pp.preco_venda,
            pro.fracao,
            :QUANT_INTEIRA,
            0,
            pp.preco_venda,
            0, -- PERCENT_DESCONTO_OPERACAO
            0, -- PERCENT_DESCONTO_PRODUTO
            0, -- VALOR_DESCONTO_COMPL
            pp.preco_venda,
            1, -- PRODUTO_SERVICO
            1, -- SITUACAO_ESTOQUE
            CURRENT_TIMESTAMP, -- DATA_MOVIMENTACAO_ESTOQUE
            0, -- VALOR_DESC_ITEM
            0,  -- VALOR_PROM_ITEM
            :ID_ESTOQUISTA,
            :ID_SOLICITANTE,
            CURRENT_TIMESTAMP, -- DATA_ULT_OPERACAO
            2, -- POSICAO_ESTOQUE
            null, -- DATA_RESERVA
            null, -- DATA_PREV_RET
            0, -- DIARIAS_EFETIVAS
            0, -- FATOR_CONV_QTDE
            pro.unidade, -- UNIDADE
            0, -- PRECO_MERCADO_CADASTRO
            'N', -- IND_CANCELAMENTO
            '', -- TOTALIZADOR_PARCIAL
            0, -- ID_PREVENDA
            0, -- ID_DAV_ORCAMENTO
            0, -- ID_DAV_OS
            null, -- ACRESCIMO
            null, -- QTD_ENTREGA_INT
            null, -- QTD_ENTREGA_FRA
            null, -- CHAVE
            '', -- NUM_PEDIDO_CLI
            0, -- NUM_ITEM_PEDIDO_CLI
            0, -- LIBERACAO_ESTOQUE
            0, -- PERC_ALTM
            0, -- VALOR_ALTM
            null, -- MOTIVO_DEV_SNGPC
            '', -- COD_ADICIONAL
            2, -- TAXA_ENTREGA
            0, -- PERC_ALT_MENOS
            0 -- MULTIPLICADOR
            from produtos pro

                             join produtos_precos pp on pp.id_empresa = pro.id_empresa
                                                    and pp.id_filial = 1
                                                    and pp.id_produto = pro.id_produto
                             join produtos_estoques pe on pe.id_empresa = pro.id_empresa
                                                      and pe.id_filial = 1
                                                      and pe.id_produto = pro.id_produto


                             where pro.id_produto = :id_produto;



        insert into MOVIMENTO_ITENS_LOTES (
            ID_EMPRESA,
            ID_FILIAL,
            ID_MOVIMENTO,
            ID_PACOTE,
            ID_PRODUTO,
            ID_PRODUTO_COMPOSTO,
            LOTE_PRODUTO,
            QUANT_INTEIRA,
            FRACAO,
            QUANT_FRACAO,
            DATA_FABRICACAO,
            DATA_VALIDADE,
            ID_GRUPO_PRODUTOS,
            ID_SUB_GRUPO_PRODUTOS)
        select
            :ID_EMPRESA, -- ID_EMPRESA
            :ID_FILIAL, -- ID_FILIAL
            :ID_MOVIMENTO, -- ID_MOVIMENTO
            1, -- ID_PACOTE
            pe.id_produto,  -- ID_PRODUTO
            pe.id_produto,  -- ID_PRODUTO_COMPOSTO
            pe.id_lote, -- LOTE_PRODUTO
            1, -- QUANT_INTEIRA
            pro.fracao, -- FRACAO
            0, -- QUANT_FRACAO
            pe.data_fabricacao, -- DATA_FABRICACAO
            pe.data_validade, -- DATA_VALIDADE
            pro.id_grupo_produtos, -- ID_GRUPO_PRODUTOS
            pro.id_sub_grupo_produtos -- ID_SUB_GRUPO_PRODUTOS
            from produtos_estoques pe
            join produtos pro on pro.id_empresa = pe.id_empresa
                        and pro.id_produto = pe.id_produto
            where pe.id_empresa = :ID_EMPRESA
                and pe.id_filial = :ID_FILIAL
                and pe.id_produto = :ID_PRODUTO;

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
            pe.id_produto,
            pe.id_produto,
            :ID_SOLICITANTE,
            1,
            0,
            null,
            2,
            null
        from produtos_estoques pe
            join produtos pro on pro.id_empresa = pe.id_empresa
                        and pro.id_produto = pe.id_produto
            where pe.id_empresa = :ID_EMPRESA
                and pe.id_filial = :ID_FILIAL
                and pe.id_produto = :ID_PRODUTO;

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
                    pe.id_produto,
                    pe.id_produto,
                    :ID_EXECUTOR,
                    2,
                    0,
                    null,
                    2,
                    null
                from produtos_estoques pe
                    join produtos pro on pro.id_empresa = pe.id_empresa
                                and pro.id_produto = pe.id_produto
                    where pe.id_empresa = :ID_EMPRESA
                        and pe.id_filial = :ID_FILIAL
                        and pe.id_produto = :ID_PRODUTO;
    
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

        UPDATE MOVIMENTO
        SET DATA_OPERACAO = current_timestamp,
        TOTAL_LIQUIDO = (SELECT SUM(TOTAL_LIQUIDO)
            FROM MOVIMENTO_ITENS 
            WHERE ID_MOVIMENTO = :ID_MOVIMENTO),
        TOTAL_BRUTO = (SELECT SUM(TOTAL_LIQUIDO)
            FROM MOVIMENTO_ITENS 
            WHERE ID_MOVIMENTO = :ID_MOVIMENTO),
        VALOR_REAL = (SELECT SUM(TOTAL_LIQUIDO)
            FROM MOVIMENTO_ITENS 
            WHERE ID_MOVIMENTO = :ID_MOVIMENTO)
        WHERE ID_MOVIMENTO = :ID_MOVIMENTO;


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
            '(GESTOR) - INSERÇÃO DO ITEM: ' || FORMATDATETIME('dd/mm/yyyy HH:MM:SS', CURRENT_TIMESTAMP) || ' ITEM: ' || :CODIGO_FABRICANTE || '-' || :NOME_PRODUTO,
            'S',
            CURRENT_TIMESTAMP
        );
     END

     RESULT_FLAG = 1;
END;
