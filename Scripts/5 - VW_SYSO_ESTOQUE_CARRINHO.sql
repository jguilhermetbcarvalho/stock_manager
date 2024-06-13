CREATE OR ALTER VIEW VW_SYSO_ESTOQUE_CARRINHO AS
SELECT
    site.id_pk,
    site.datahora_criacao,
    site.id_movimento,
    site.id_produto,
    site.codigo_fabricante,
    site.codigo_barras,
    site.descricao,
    site.quant_inteira,
    site.quant_fracao,
    site.fracao,
    site.identificador_os,
    CASE 
        WHEN site.situacao = 1 THEN '1 - LANCADO'
        WHEN site.situacao = 2 THEN '2 - VINCULADO'
        WHEN site.situacao = 3 THEN '3 - DEVOLVIDO'
        ELSE '4 - DESCONHECIDO'
    END AS situacao,
    site.id_usuario,
    site.id_executor,
    u1.nome_usuario AS nome_usuario,
    u2.nome_usuario AS nome_executor,
    datahora_ult_operacao,
    u3.id_usuario AS id_usuario_ult_operacao,
    u3.nome_usuario AS nome_usuario_ult_operacao,
    site.localizacao,
    formatdatetime('dd/mm/yyyy', site.datahora_criacao) as data_criacao,
    formatdatetime('dd/mm/yyyy', datahora_ult_operacao) as data_movimento
FROM 
    SYSO_ESTOQUE_CARRINHO site
LEFT JOIN 
    USUARIOS u1 ON site.id_usuario = u1.id_usuario
LEFT JOIN 
    USUARIOS u2 ON site.id_executor = u2.id_usuario
LEFT JOIN 
    USUARIOS u3 ON site.id_usuario_ult_operacao = u3.id_usuario;
