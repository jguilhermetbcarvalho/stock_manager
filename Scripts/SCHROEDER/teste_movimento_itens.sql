/*

Inserir item no movimetno

*/

insert into MOVIMENTO_ITENS (ID_EMPRESA,
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
                      select 1, /*ID_EMPRESA,*/
                             1, /*ID_FILIAL,*/
                             132857, /*ID_MOVIMENTO,*/
                             1, /*ID_PACOTE,*/
                             pe.id_produto, /*ID_PRODUTO,*/
                             pe.id_produto, /*ID_PRODUTO_COMPOSTO,*/
                             (select max(mi.item) from movimento_itens mi
                             where mi.id_empresa = 1
                               and mi.id_filial = 1
                               and mi.id_movimento = 132856), /*ITEM,*/
                             1, /*ID_DESCRICAO,*/
                             null, /*COMPL_DESCRICAO_1,*/
                             null, /*COMPL_DESCRICAO_2,*/
                             null, /*TEXTO_COMPL_DESCRICAO,*/
                             pro.id_grupo_produtos, /* ID_GRUPO_PRODUTOS,*/
                             pro.id_sub_grupo_produtos, /* ID_SUB_GRUPO_PRODUTOS,*/
                             pp.preco_venda, /* PRECO_CADASTRO,*/
                             pe.estoque_inteiro, /* ESTOQUE_INTEIRO,*/
                             pe.estoque_fracao, /* ESTOQUE_FRACAO,*/
                             pp.preco_compra, /* COMPRA_CADASTRO,*/
                             pp.preco_custo, /* CUSTO_CADASTRO,*/
                             pp.custo_medio, /* CUSTO_MEDIO_CADASTRO,*/
                             pp.preco_venda,  /* PRECO_UNITARIO,*/
                             pro.fracao, /* FRACAO,*/
                             1, /* QUANT_INTEIRA,*/
                             0, /* QUANT_FRACAO,*/
                             pp.preco_venda, /* TOTAL_BRUTO,*/
                             0, /*PERCENT_DESCONTO_OPERACAO,*/
                             0, /*PERCENT_DESCONTO_PRODUTO, */
                             0, /*VALOR_DESCONTO_COMPL,*/
                             pp.preco_venda, /* TOTAL_LIQUIDO,*/
                             1, /* PRODUTO_SERVICO,*/
                             1, /*SITUACAO_ESTOQUE,*/
                             current_timestamp, /* DATA_MOVIMENTACAO_ESTOQUE,*/
                             0, /*VALOR_DESC_ITEM,*/
                             0,  /*VALOR_PROM_ITEM,*/
                             0, /*ID_ESTOQUISTA,*/
                             0, /*ID_SOLICITANTE,*/
                             null, /*DATA_ULT_OPERACAO,*/
                             1, /*POSICAO_ESTOQUE,*/
                             null, /*DATA_RESERVA,*/
                             null, /*DATA_PREV_RET,*/
                             0, /* DIARIAS_EFETIVAS,*/
                             0, /*FATOR_CONV_QTDE, */
                             pro.unidade, /* UNIDADE,*/
                             0, /*PRECO_MERCADO_CADASTRO,*/
                             'N', /*IND_CANCELAMENTO,*/
                             '', /*TOTALIZADOR_PARCIAL,*/
                             0, /*ID_PREVENDA,*/
                             0, /*ID_DAV_ORCAMENTO,*/
                             0, /*ID_DAV_OS,*/
                             null, /*ACRESCIMO,*/
                             null, /*QTD_ENTREGA_INT,*/
                             null, /*QTD_ENTREGA_FRA,*/
                             null, /*CHAVE,*/
                             '', /*NUM_PEDIDO_CLI,*/
                             0, /*NUM_ITEM_PEDIDO_CLI,*/
                             0, /*LIBERACAO_ESTOQUE,*/
                             0, /*PERC_ALTM,*/
                             0, /*VALOR_ALTM,*/
                             null, /*MOTIVO_DEV_SNGPC,*/
                             '', /*COD_ADICIONAL,*/
                             2, /*TAXA_ENTREGA,*/
                             0, /*PERC_ALT_MENOS,*/
                             0 /*MULTIPLICADOR*/
                             from produtos pro

                             join produtos_precos pp on pp.id_empresa = pro.id_empresa
                                                    and pp.id_filial = 1
                                                    and pp.id_produto = pro.id_produto
                             join produtos_estoques pe on pe.id_empresa = pro.id_empresa
                                                      and pe.id_filial = 1
                                                      and pe.id_produto = pro.id_produto


                             where pro.id_produto = '0000000005616';


insert into MOVIMENTO_ITENS_LOTES (ID_EMPRESA,
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
                            select 1, /*ID_EMPRESA,*/
                                   1, /*ID_FILIAL,*/
                                   132857, /*ID_MOVIMENTO,*/
                                   1, /*ID_PACOTE,*/
                                   pe.id_produto,  /*ID_PRODUTO, */
                                   pe.id_produto,  /*ID_PRODUTO_COMPOSTO,*/
                                   pe.id_lote, /* LOTE_PRODUTO,*/
                                   1, /*QUANT_INTEIRA,*/
                                   pro.fracao, /* FRACAO,*/
                                   0, /*QUANT_FRACAO,*/
                                   pe.data_fabricacao, /* DATA_FABRICACAO,*/
                                   pe.data_validade, /* DATA_VALIDADE,*/
                                   pro.id_grupo_produtos, /* ID_GRUPO_PRODUTOS,*/
                                   pro.id_sub_grupo_produtos /* ID_SUB_GRUPO_PRODUTOS*/
                                   from produtos_estoques pe
                                   join produtos pro on pro.id_empresa = pe.id_empresa
                                                    and pro.id_produto = pe.id_produto
                                   where pe.id_empresa = 1
                                     and pe.id_filial = 1
                                     and pe.id_produto = '0000000005616';

insert into PRODUTOS_ESTOQUES_LOG (ID_EMPRESA,
                                   ID_FILIAL,
                                   ID_AJUSTE,
                                   ID_PRODUTO,
                                   ID_LOTE,
                                   ID_PRODUTO_SERIE,
                                   DATA,
                                   ID_USUARIO,
                                   ID_MOVIMENTO,
                                   FRACAO,
                                   ESTOQUE_ANTERIOR,
                                   QUANTIDADE,
                                   ESTOQUE_POSTERIOR,
                                   SITUACAO_ESTOQUE,
                                   OPERACAO,
                                   ACAO_BD,
                                   SITUACAO_ESTOQUE_OLD)
                            select 1, /*ID_EMPRESA,*/
                                   1, /*ID_FILIAL,*/
                                   gen_id(gen_produtos_estoques_log,1), /* ID_AJUSTE,*/
                                   pe.id_produto,/*ID_PRODUTO,*/
                                   '', /*ID_LOTE,*/
                                   null, /*ID_PRODUTO_SERIE,*/
                                   current_timestamp, /*data*/
                                   1, /*ID_USUARIO,*/
                                   132857, /*ID_MOVIMENTO,*/
                                   1, /*FRACAO,*/
                                   pe.estoque_inteiro, /* ESTOQUE_ANTERIOR,*/
                                   1, /*QUANTIDADE,*/
                                   pe.estoque_inteiro-1,/* ESTOQUE_POSTERIOR,*/
                                   1, /*SITUACAO_ESTOQUE,*/
                                   '-', /*OPERACAO,*/
                                   'I', /*ACAO_BD,*/
                                   0 /*SITUACAO_ESTOQUE_OLD*/
                                   from produtos pro

                                   join produtos_estoques pe on pe.id_empresa = pro.id_empresa
                                                      and pe.id_filial = 1
                                                      and pe.id_produto = pro.id_produto

                                   where pro.id_produto = '0000000005616';


