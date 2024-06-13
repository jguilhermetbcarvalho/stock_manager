using AMAPA.Controllers;
using AMAPA.Models;
using AMAPA.Repositoryusing;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration; // Adicione este namespace para usar IConfiguration
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using System.Collections;
using System.Collections.Generic;


namespace AMAPA.Repository
{
    public class UsuarioRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _conexao;

        public UsuarioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conexao = _configuration["ConectString"];
        }

        public IList<Usuarios> BuscarUsuarios()
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"SELECT u.ID_USUARIO, u.NOME_USUARIO, gu.DESCRICAO as NOME_GRUPO " +
                                  @"FROM USUARIOS u " +
                                  @"JOIN GRUPO_USUARIOS gu ON u.ID_GRUPO_REFERENCIA = gu.ID_GRUPO_USUARIOS;";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        var lista = new List<Usuarios>();
                        while (dr.Read())
                        {
                            try
                            {
                                Usuarios p = new Usuarios();
                                p.ID_USUARIO = Convert.ToInt32(dr["ID_USUARIO"]);
                                p.NOME_USUARIO = Convert.ToString(dr["NOME_USUARIO"]);
                                p.DESCRICAO_TIPO_USUARIO = Convert.ToString(dr["NOME_GRUPO"]);

                                lista.Add(p);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        return lista;
                    }
                }

                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }

        internal IList<Empresa> BuscarEmpresa(string quuery)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    FbCommand cmd1 = new FbCommand(quuery, conexaoFireBird);
                    FbDataReader dr = cmd1.ExecuteReader();
                    IList<Empresa> lista = new List<Empresa>();
                    while (dr.Read())
                    {
                        Empresa c = new Empresa();
                        c.Id = (int)dr["Id_Empresa"];
                        c.Nome = (string)dr["NOME_EMPRESA"];
                        lista.Add(c);
                    }

                    return lista;
                }

                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }

        internal IList<Empresa> BuscarFilial(string quuery)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    FbCommand cmd1 = new FbCommand(quuery, conexaoFireBird);
                    FbDataReader dr = cmd1.ExecuteReader();
                    IList<Empresa> lista = new List<Empresa>();
                    while (dr.Read())
                    {
                        Empresa c = new Empresa();
                        c.Id = (int)dr["Id_filial"];
                        c.Nome = (string)dr["NOME_FANTASIA"];
                        lista.Add(c);
                    }

                    return lista;
                }

                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }

        internal async Task<Login> BuscarUsuarioAsync(string user, string senha)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    string mSQL = @"SELECT * FROM USUARIOS WHERE ID_USUARIO = @user AND (SENHA_ACESSO_GESTOR IS NULL OR SENHA_ACESSO_GESTOR = @senha) AND ID_GRUPO_REFERENCIA = 1;";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@senha", senha);

                    FbDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        MItem1 usuario = new MItem1();
                        usuario.ID_USUARIO = Convert.ToInt32(dr["ID_USUARIO"]);
                        usuario.NOME_USUARIO = Convert.ToString(dr["NOME_USUARIO"]);
                        usuario.TELEFONE = Convert.ToString(dr["TELEFONE"]);
                        usuario.EMAIL = Convert.ToString(dr["EMAIL"]);
                        usuario.CPF = Convert.ToString(dr["CPF"]);
                        usuario.ID_GRUPO_REFERENCIA = Convert.ToString(dr["ID_GRUPO_REFERENCIA"]);
                        usuario.SENHA_ACESSO_GESTOR = Convert.ToString(dr["SENHA_ACESSO_GESTOR"]);

                        // Preencher o objeto Login
                        Login login = new Login();
                        login.m_Item1 = usuario;
                        login.m_Item2 = true; // Indicar que o usuário foi encontrado com sucesso
                        login.m_Item3 = "Usuário encontrado."; // Mensagem opcional
                        return login;
                    }
                    else
                    {
                        // Usuário não encontrado
                        return null;
                    }
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }


        public bool BuscarUsuarioLiberacao(string user, string senha)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    string mSQL = @"SELECT * FROM USUARIOS WHERE ID_USUARIO = @user AND (SENHA_LIBERACAO_GESTOR = @senha OR SENHA_LIBERACAO_GESTOR IS NULL);";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@senha", senha);

                    FbDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        return true;
                    }
                    else
                    {
                        // Usuário não encontrado
                        return false;
                    }
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }

        public void AlterarSenhaUsuario(int idUsuario, string senhaLiberacao, string senhaAcesso, int naoValidarSenha)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    string mSQL = "";
                    conexaoFireBird.Open();

                    if (naoValidarSenha == 0)
                    {
                        if (senhaAcesso == null)
                        {
                            mSQL = @"UPDATE USUARIOS SET SENHA_LIBERACAO_GESTOR = @senhaLiberacao WHERE ID_USUARIO = @idUsuario";
                        }
                        else if (senhaLiberacao == null)
                        {
                            mSQL = @"UPDATE USUARIOS SET SENHA_ACESSO_GESTOR = @senhaAcesso WHERE ID_USUARIO = @idUsuario";
                        }
                        else
                        {
                            mSQL = @"UPDATE USUARIOS SET SENHA_ACESSO_GESTOR = @senhaAcesso, ENHA_LIBERACAO_GESTOR = @senhaLiberacao WHERE ID_USUARIO = @idUsuario";
                        }
                    }
                    else if (naoValidarSenha == 1)
                    {
                        mSQL = @"UPDATE USUARIOS SET NAO_VALIDAR_SENHA = @naoValidarSenha WHERE ID_USUARIO = @idUsuario";
                    }            

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@senhaLiberacao", senhaLiberacao);
                    cmd.Parameters.AddWithValue("@senhaAcesso", senhaAcesso);
                    cmd.Parameters.AddWithValue("@naoValidarSenha", naoValidarSenha);

                    cmd.ExecuteNonQuery();
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
        }

    }
}
