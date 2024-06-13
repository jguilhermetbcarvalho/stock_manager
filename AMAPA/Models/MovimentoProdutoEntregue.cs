using System.Reflection.Metadata;

namespace AMAPA.Models
{
    public class MovimentoProdutoEntregue
    {
        public string ID_PRODUTO { get; set; }
        public int ID_USUARIO { get; set; }
        public int ID_EXECUTOR { get; set; }
        public string CODIGO_FABRICA { get; set; }
        public string CODIGO_BARRAS { get; set; }
        public string PRODUTO { get; set; }
        public string SITUACAO { get; set; }
        public int QTDE_INTEIRA { get; set; }
        public string IDENTIFICA_OS { get; set; }
        public int ID_MOVIMENTO { get; set; }
        public string TIPO_OPERACAO { get; set; }
        public string NOME_EXECUTOR { get; set; }
        public string NOME_ESTOQUISTA { get; set; }
        public string DATA_ULT_OPERACAO { get; set; }
        public string DATA_MOVIMENTO { get; set; }

    }

}
