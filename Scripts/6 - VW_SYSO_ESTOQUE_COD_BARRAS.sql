CREATE OR ALTER VIEW VW_SYSO_ESTOQUE_COD_BARRAS AS
SELECT id_produto, codigo_barras
FROM (
    SELECT id_produto, codigo_barras
    FROM PRODUTOS
    WHERE codigo_barras IS NOT NULL
    UNION
    SELECT id_produto, codigo_barras
    FROM PRODUTOS_CODIGOS
    WHERE codigo_barras IS NOT NULL
) AS codigos_barras_combinados

