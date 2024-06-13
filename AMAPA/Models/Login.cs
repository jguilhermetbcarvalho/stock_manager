namespace AMAPA.Models
{
    public class MItem1
    {
        public int ID_USUARIO { get; set; }
        public string NOME_USUARIO { get; set; }
        public string TELEFONE { get; set; }
        public string EMAIL { get; set; }
        public string CPF { get; set; }
        public string ID_GRUPO_REFERENCIA { get; set; }
        public string SENHA_ACESSO_GESTOR { get; set; }
    }

    public class Login
    {
        public MItem1 m_Item1 { get; set; }
        public bool m_Item2 { get; set; }
        public string m_Item3 { get; set; }
    }
}

