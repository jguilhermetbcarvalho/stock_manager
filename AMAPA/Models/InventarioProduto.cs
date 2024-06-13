namespace AMAPA.Models
{
    public class InventarioProduto
    {
        public int Id_Lancamento { get; set; }
        public string ID_PRODUTO { get; set; }
        public int ID_EMPRESA { get; set; }
        public int ID_FILIAL { get; set; }
        public string LOTE { get; set; }
        public int ESTOQUE_ATUAL { get; set; }
        public double ESTOQUE_INTEIRO { get; set; }
        public double ESTOQUE_FRACAO { get; set; }
        public DateTime? DT_FABRICACAO { get; set; }
        public DateTime? DT_VENCIMENTO { get; set; }
    }
}
