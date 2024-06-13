using System.Reflection.Metadata;

namespace AMAPA.Models
{
    public class EntregarProdutos
    {
        public int idMovimento { get; set; }
        public string idProduto { get; set; }
        public string identificaOs { get; set; }
        public int qtdeInteira { get; set; }
        public string nomeProduto { get; set; }
        public string fabricProduto { get; set; }
        public int idExecutor { get; set; }
        public int idEstoquista { get; set; }
    }

}
