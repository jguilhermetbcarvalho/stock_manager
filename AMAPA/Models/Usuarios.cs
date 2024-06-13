namespace AMAPA.Models
{
    public class Usuarios
    {
        public int ID_USUARIO { get; set; }
        public string? NOME_USUARIO { get; set; }
        public string? TELEFONE { get; set; }
        public string? EMAIL { get; set; }
        public string? CPF { get; set; }
        public string? LOGIN { get; set; }
        public int TIPO_USUARIO { get; set; }
        public string? DESCRICAO_TIPO_USUARIO { get; set; }
        public int? ID_EMPRESA { get; set; }
        public int? ID_FILIAL { get; set; }

    }

    public class Empresa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }


}
