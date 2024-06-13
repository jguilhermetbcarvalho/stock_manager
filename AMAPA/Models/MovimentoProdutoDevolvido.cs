using System.Reflection.Metadata;

namespace AMAPA.Models
{
    public class MovimentoProdutoDevolvido
    {
        public int ID_USUARIO { get; set; }
        public string ID_PRODUTO { get; set; }
        public string CODIGO_FABRICANTE { get; set; }
        public string CODIGO_BARRAS { get; set; }
        public string DESCRICAO { get; set; }
        public int QUANT_INTEIRA { get; set; }
        public int ID_EXECUTOR { get; set; }
        public int ID_MOVIMENTO { get; set; }
        public string IDENTIFICADOR_OS { get; set; }
        public string NOME_USUARIO { get; set; }
        public string NOME_EXECUTOR { get; set; }
        public string DATAHORA_ULT_OPERACAO { get; set; }
        public string DATA_MOVIMENTO { get; set; }
        public int ID_USUARIO_ULT_OPERACAO { get; set; }
        public string NOME_USUARIO_ULT_OPERACAO { get; set; }


    }

}


