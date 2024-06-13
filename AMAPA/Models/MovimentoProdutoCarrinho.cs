using System.Reflection.Metadata;

namespace AMAPA.Models
{
    public class MovimentoProdutoCarrinho
    {
        public int ID_PK { get; set; }
        public int ID_USUARIO { get; set; }
        public string ID_PRODUTO { get; set; }
        public string CODIGO_FABRICANTE { get; set; }
        public string CODIGO_BARRAS { get; set; }
        public string DESCRICAO { get; set; }
        public int FRACAO { get; set; }
        public int QUANT_INTEIRA { get; set; }
        public int QUANT_FRACAO { get; set; }
        public int ID_EXECUTOR { get; set; }
        public int ID_MOVIMENTO { get; set; }
        public string IDENTIFICADOR_OS { get; set; }
        public string NOME_USUARIO { get; set; }
        public string NOME_EXECUTOR { get; set; }
        public DateTime DATAHORA_CRIACAO { get; set; }
        public string DATA_CRIACAO { get; set; }
        public string LOCALIZACAO { get; set; }

    }

}
