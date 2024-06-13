CREATE OR ALTER VIEW VW_SYSO_ESTOQUE_PRODUTOS(
    ID_EMPRESA,
    ID_FILIAL,
    ID_PRODUTO,
    CODIGO_BARRAS,
    CODIGO_FABRICANTE,
    ID_DESCRICAO,
    DESCRICAO,
    ID_GRUPO_PRODUTOS,
    ID_SUB_GRUPO_PRODUTOS,
    FRACAO,
    FATOR_CONV_QTDE,
    SITUACAO_ESTOQUE,
    UNIDADE,
    PRECO_CADASTRO,
    PRECO_UNITARIO,
    ESTOQUE_INTEIRO,
    ESTOQUE_FRACAO,
    DATA_FABRICACAO,
    DATA_VENCIMENTO,
    LOTE,
    LOCALIZACAO,
    CODIGO_BARRAS_2)
AS
select distinct
       pro.id_empresa,
       pro.id_filial,
       pro.id_produto,
       pro.codigo_barras,
       pro.codigo_fabricante,
       pd.id_descricao,
       pd.descricao,
       pro.id_grupo_produtos,
       pro.id_sub_grupo_produtos,
       pro.fracao,
       pro.fator_conv_qtde,
       pro.situacao,
       pro.unidade,
       pp.preco_venda,
       pp.preco_venda,
       PE.estoque_inteiro,
       PE.estoque_fracao,
       pe.data_fabricacao,
       pe.data_validade,
       pe.id_lote,
       prol.localizacao,
       pco.codigo_barras
from produtos pro

join produtos_descricoes pd on pd.id_empresa = pro.id_empresa
                           and pd.id_produto = pro.id_produto

join produtos_estoques pe on pe.id_empresa = pro.id_empresa
                         and pe.id_filial = 1
                         and pe.id_produto = pro.id_produto

join produtos_precos pp on pp.id_empresa = pro.id_empresa
                       and pp.id_filial = pe.id_filial
                       and pp.id_produto = pro.id_produto

left join produtos_localizacao prol on prol.id_empresa = pro.id_empresa
                       and prol.id_filial = pe.id_filial
                       and prol.id_produto = pro.id_produto

left join produtos_codigos pco on pco.id_empresa = pro.id_empresa
                              and pco.id_produto = pro.id_produto

where pro.id_empresa = 1
  and pro.situacao = 1
;

