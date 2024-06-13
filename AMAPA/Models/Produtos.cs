namespace AMAPA.Models
{
    public class Produtos
    {
        public int ID_EMPRESA { get; set; }
        public int ID_FILIAL { get; set; }
        public string ID_PRODUTO { get; set; }
        public string CODIGO_BARRAS { get; set; }
        public string CODIGO_FABRICANTE { get; set; }
        public int ID_DESCRICAO { get; set; }
        public string DESCRICAO { get; set; }
        public int ID_GRUPO_PRODUTOS { get; set; }
        public int ID_SUB_GRUPO_PRODUTOS { get; set; }
        public double FRACAO { get; set; }
        public double FATOR_CONV_QTDE { get; set; }
        public int SITUACAO_ESTOQUE { get; set; }
        public string UNIDADE { get; set; }
        public double PRECO_CADASTRO { get; set; }
        public int ESTOQUE_ATUAL { get; set; }
        public double ESTOQUE_INTEIRO { get; set; }
        public double ESTOQUE_FRACAO { get; set; }
        public DateTime? DT_FABRICACAO { get; set; }
        public DateTime? DT_VENCIMENTO { get; set; }
        public string LOTE { get; set; }
        public int ID_LANCAMENTO { get; set; }
        public string LOCALIZACAO { get; set; }
    }
}
