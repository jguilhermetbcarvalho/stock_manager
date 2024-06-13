using AMAPA.Controllers;
using AMAPA.Models;
using AMAPA.Repositoryusing;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration; // Adicione este namespace para usar IConfiguration
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using System.Collections.Generic;
using System.Data;

namespace AMAPA.Repository
{
    public class MovimentoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly string _conexao;

        public MovimentoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _usuarioRepository = new UsuarioRepository(_configuration);
            _conexao = _configuration["ConectString"];
        }

        #region Produtos_Pendentes
        public IList<MovimentoProdutoPendente> BuscarMovimentoProdutoPendente()
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"SELECT * FROM VW_SYSO_ESTOQUE_PROD_PENDENTE";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        var lista = new List<MovimentoProdutoPendente>();
                        while (dr.Read())
                        {
                            try
                            {
                                MovimentoProdutoPendente p = new MovimentoProdutoPendente();
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_FABRICA = Convert.ToString(dr["CODIGO_FABRICA"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.PRODUTO = Convert.ToString(dr["PRODUTO"]);
                                p.SITUACAO = Convert.ToString(dr["SITUACAO"]);
                                p.QTDE_INTEIRA = Convert.ToInt32(dr["QTDE_INTEIRA"]);
                                p.IDENTIFICA_OS = Convert.ToString(dr["IDENTIFICA_OS"]);
                                p.ID_MOVIMENTO = Convert.ToInt32(dr["ID_MOVIMENTO"]);
                                p.TIPO_OPERACAO = Convert.ToString(dr["TIPO_OPERACAO"]);
                                p.LOCALIZACAO = Convert.ToString(dr["LOCALIZACAO"]);
                                p.DATA_MOVIMENTO = Convert.ToString(dr["DATA_MOVIMENTO"]);

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

        public void ConfirmarEntregaProdutoPendente(int idMovimento, string idProduto, int idExecutor, int idEstoquista)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"EXECUTE PROCEDURE SP_SYSO_ESTOQUE_CONF_ITEM_MOV(@idMovimento, @idProduto, @idExecutor, @idEstoquista)";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@idMovimento", idMovimento);
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);
                    cmd.Parameters.AddWithValue("@idExecutor", idExecutor);
                    cmd.Parameters.AddWithValue("@idEstoquista", idEstoquista);

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
        #endregion

        #region Produtos Carrinho
        public IList<MovimentoProdutoCarrinho> BuscarMovimentoProdutoCarrinho()
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"select * from VW_SYSO_ESTOQUE_CARRINHO " +
                    @"where SITUACAO = '1 - LANCADO'";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        var lista = new List<MovimentoProdutoCarrinho>();
                        while (dr.Read())
                        {
                            try
                            {
                                MovimentoProdutoCarrinho p = new MovimentoProdutoCarrinho();
                                p.ID_PK = Convert.ToInt32(dr["ID_PK"]);
                                p.ID_USUARIO = Convert.ToInt32(dr["ID_USUARIO"]);
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_FABRICANTE = Convert.ToString(dr["CODIGO_FABRICANTE"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.DESCRICAO = Convert.ToString(dr["DESCRICAO"]);
                                p.QUANT_INTEIRA = Convert.ToInt32(dr["QUANT_INTEIRA"]);
                                p.ID_EXECUTOR = Convert.ToInt32(dr["ID_EXECUTOR"]);
                                p.IDENTIFICADOR_OS = Convert.ToString(dr["IDENTIFICADOR_OS"]);
                                p.NOME_EXECUTOR = Convert.ToString(dr["NOME_EXECUTOR"]);
                                p.NOME_USUARIO = Convert.ToString(dr["NOME_USUARIO"]);
                                p.DATAHORA_CRIACAO = Convert.ToDateTime(dr["DATAHORA_CRIACAO"]);
                                p.DATA_CRIACAO = Convert.ToString(dr["DATA_CRIACAO"]);
                                p.LOCALIZACAO = Convert.ToString(dr["LOCALIZACAO"]);

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

        public int  EditarQuantidadeProdutoCarrinho(int idPk, string idProduto, int novaQuantInteira, int quantInteira)
        {
            int resultFlag = 0;

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    // Query SQL para chamar a stored procedure
                    string mSQL = @"EXECUTE PROCEDURE SP_SYSO_ESTOQUE_EDIT_ITEM_CAR(@idPk, @idProduto, @novaQuantInteira, @quantInteira)";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    // Parâmetros para a query SQL
                    cmd.Parameters.AddWithValue("@idPk", idPk);
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);
                    cmd.Parameters.AddWithValue("@novaQuantInteira", novaQuantInteira);
                    cmd.Parameters.AddWithValue("@quantInteira", quantInteira);

                    cmd.Parameters.Add("@resultFlag", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    resultFlag = Convert.ToInt32(cmd.Parameters["@resultFlag"].Value);
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
            return resultFlag;
        }

        public int AdicionarProdutoCarrinho(string idProduto, string descricao, string codigoBarras, string codigoFabricante, int quantidade, string veiculo, int idExecutor, int idEstoquista, string localizacao)
        {
            int resultFlag = 0;

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    // Query SQL para chamar a stored procedure
                    string mSQL = @"EXECUTE PROCEDURE SP_SYSO_ESTOQUE_ADD_ITEM_CAR(@idProduto, @descricao, @codigoBarras, @codigoFabricante, @quantidade, @veiculo, @idExecutor, @idEstoquista, @localizacao);";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    // Parâmetros para a query SQL
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.Parameters.AddWithValue("@codigoBarras", codigoBarras);
                    cmd.Parameters.AddWithValue("@codigoFabricante", codigoFabricante);
                    cmd.Parameters.AddWithValue("@quantidade", quantidade);
                    cmd.Parameters.AddWithValue("@veiculo", veiculo);
                    cmd.Parameters.AddWithValue("@idExecutor", idExecutor);
                    cmd.Parameters.AddWithValue("@idEstoquista", idEstoquista);
                    cmd.Parameters.AddWithValue("@localizacao", localizacao);

                    // Parâmetro de saída
                    cmd.Parameters.Add("@resultFlag", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    resultFlag = Convert.ToInt32(cmd.Parameters["@resultFlag"].Value);
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
            return resultFlag;
        }

        public void RemoverProdutoCarrinho(string idProduto, string veiculo, int idEstoquista, int quantInteira)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    // Query SQL para inserir um novo produto na tabela de produtos
                    string mSQL = @"EXECUTE PROCEDURE SP_SYSO_ESTOQUE_DEVOL_ITEM(@idProduto, @veiculo, @idEstoquista, @quantInteira);";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    // Parâmetros para a query SQL
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);
                    cmd.Parameters.AddWithValue("@veiculo", veiculo);
                    cmd.Parameters.AddWithValue("@idEstoquista", idEstoquista);
                    cmd.Parameters.AddWithValue("@quantInteira", quantInteira);

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

        public int VincularProdutoCarrinho(Produtos produto, int idMovimento, string veiculo, string idProduto, int quantInteira, int idExecutor, int idVinculacao, int idEstoquista, string nomeProduto, string fabricProduto)
        {
            int resultFlag = 0;

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    string mSQL = @"EXECUTE PROCEDURE SP_SYSO_ESTOQUE_VINC_ITEM_MOV(@idEmpresa, @idFilial, @idMovimento, @idProduto, @idDescricao, @quantInteira, @idVinculacao, @idExecutor, @veiculo, @idEstoquista, @nomeProduto, @fabricProduto);";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    cmd.Parameters.AddWithValue("@idEmpresa", produto.ID_EMPRESA);
                    cmd.Parameters.AddWithValue("@idFilial", produto.ID_FILIAL);
                    cmd.Parameters.AddWithValue("@idMovimento", idMovimento);
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);
                    cmd.Parameters.AddWithValue("@idDescricao", produto.ID_DESCRICAO);
                    cmd.Parameters.AddWithValue("@quantInteira", quantInteira);
                    cmd.Parameters.AddWithValue("@idVinculacao", idVinculacao);
                    cmd.Parameters.AddWithValue("@idExecutor", idExecutor);
                    cmd.Parameters.AddWithValue("@veiculo", veiculo);
                    cmd.Parameters.AddWithValue("@idEstoquista", idEstoquista);
                    cmd.Parameters.AddWithValue("@nomeProduto", nomeProduto);
                    cmd.Parameters.AddWithValue("@fabricProduto", fabricProduto);

                    cmd.Parameters.Add("@resultFlag", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    resultFlag = Convert.ToInt32(cmd.Parameters["@resultFlag"].Value);
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
            return resultFlag;
        }
        #endregion

        #region Produtos Entregues
        public IList<MovimentoProdutoEntregue> BuscarMovimentoEntregue()
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"SELECT * FROM VW_SYSO_ESTOQUE_PROD_ENTREGUE";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        var lista = new List<MovimentoProdutoEntregue>();
                        while (dr.Read())
                        {
                            try
                            {
                                MovimentoProdutoEntregue p = new MovimentoProdutoEntregue();
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_FABRICA = Convert.ToString(dr["CODIGO_FABRICA"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.PRODUTO = Convert.ToString(dr["PRODUTO"]);
                                p.SITUACAO = Convert.ToString(dr["SITUACAO"]);
                                p.QTDE_INTEIRA = Convert.ToInt32(dr["QTDE_INTEIRA"]);
                                p.IDENTIFICA_OS = Convert.ToString(dr["IDENTIFICA_OS"]);
                                p.ID_MOVIMENTO = Convert.ToInt32(dr["ID_MOVIMENTO"]);
                                p.TIPO_OPERACAO = Convert.ToString(dr["TIPO_OPERACAO"]);
                                p.NOME_EXECUTOR = Convert.ToString(dr["NOME_EXECUTOR"]);
                                p.NOME_ESTOQUISTA = Convert.ToString(dr["NOME_ESTOQUISTA"]);
                                p.ID_USUARIO = Convert.ToInt32(dr["ID_USUARIO"]);
                                p.ID_EXECUTOR = Convert.ToInt32(dr["ID_EXECUTOR"]);
                                p.DATA_ULT_OPERACAO = Convert.ToString(dr["DATA_ULT_OPERACAO"]);
                                p.DATA_MOVIMENTO = Convert.ToString(dr["DATA_MOVIMENTO"]);

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

        public int RemoverProdutoEntregue(int idMovimento, string idProduto, int quantInteira, int idUsuario, string nomeProduto, string fabricProduto)
        {
            int resultFlag = 0;
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    // Query SQL para inserir um novo produto na tabela de produtos
                    
                    string mSQL = "";
                    mSQL = @"EXECUTE PROCEDURE SP_SYSO_ESTOQUE_REMOV_ITEM_MOV(@idMovimento, @idProduto, @quantInteira, @idEmpresa, @idFilial, @idUsuario, @nomeProduto, @fabricProduto);";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    // Parâmetros para a query SQL
                    cmd.Parameters.AddWithValue("@idMovimento", idMovimento);
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);
                    cmd.Parameters.AddWithValue("@quantInteira", quantInteira);
                    cmd.Parameters.AddWithValue("@idEmpresa", 1);
                    cmd.Parameters.AddWithValue("@idFilial", 1);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@nomeProduto", nomeProduto);
                    cmd.Parameters.AddWithValue("@fabricProduto", fabricProduto);

                    cmd.Parameters.Add("@resultFlag", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    resultFlag = Convert.ToInt32(cmd.Parameters["@resultFlag"].Value);
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
            return resultFlag;
        }
        #endregion

        #region Produtos Devolvidos
        public IList<MovimentoProdutoDevolvido> BuscarMovimentoDevolvido()
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"select * from VW_SYSO_ESTOQUE_CARRINHO " +
                    @"where SITUACAO = '3 - DEVOLVIDO' AND ID_MOVIMENTO is null";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        var lista = new List<MovimentoProdutoDevolvido>();
                        while (dr.Read())
                        {
                            try
                            {
                                MovimentoProdutoDevolvido p = new MovimentoProdutoDevolvido();
                                p.ID_USUARIO = Convert.ToInt32(dr["ID_USUARIO"]);
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_FABRICANTE = Convert.ToString(dr["CODIGO_FABRICANTE"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.DESCRICAO = Convert.ToString(dr["DESCRICAO"]);
                                p.QUANT_INTEIRA = Convert.ToInt32(dr["QUANT_INTEIRA"]);
                                p.ID_EXECUTOR = Convert.ToInt32(dr["ID_EXECUTOR"]);
                                p.IDENTIFICADOR_OS = Convert.ToString(dr["IDENTIFICADOR_OS"]);
                                p.NOME_EXECUTOR = Convert.ToString(dr["NOME_EXECUTOR"]);
                                p.NOME_USUARIO = Convert.ToString(dr["NOME_USUARIO"]);
                                p.DATAHORA_ULT_OPERACAO = Convert.ToString(dr["DATAHORA_ULT_OPERACAO"]);
                                p.DATA_MOVIMENTO = Convert.ToString(dr["DATA_MOVIMENTO"]);
                                p.ID_USUARIO_ULT_OPERACAO = Convert.ToInt32(dr["ID_USUARIO_ULT_OPERACAO"]);
                                p.NOME_USUARIO_ULT_OPERACAO = Convert.ToString(dr["NOME_USUARIO_ULT_OPERACAO"]);

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
        #endregion

        public (bool movimentoEncontrado, bool produtoEncontrado, bool quantidadeIgual) BuscarMovimento(int idMovimento, string idProduto, int quantInteira)
        {
            bool movimentoEncontrado = false;
            bool produtoEncontrado = false;
            bool quantidadeIgual = false;

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    string mSQL = @"
                SELECT
                    CASE WHEN EXISTS (SELECT 1 FROM MOVIMENTO WHERE ID_MOVIMENTO = @idMovimento AND tipo_movimento IN (1,2)) THEN 1 ELSE 0 END AS MovimentoEncontrado,
                    CASE WHEN EXISTS (SELECT 1 FROM MOVIMENTO_ITENS WHERE ID_MOVIMENTO = @idMovimento AND ID_PRODUTO = @idProduto AND POSICAO_ESTOQUE = 1) THEN 1 ELSE 0 END AS ProdutoEncontrado,
                    CASE WHEN EXISTS (SELECT 1 FROM MOVIMENTO_ITENS WHERE ID_MOVIMENTO = @idMovimento AND ID_PRODUTO = @idProduto AND QUANT_INTEIRA = @quantInteira AND POSICAO_ESTOQUE = 1) THEN 1 ELSE 0 END AS QuantidadeIgual
                FROM RDB$DATABASE";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@idMovimento", idMovimento);
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);
                    cmd.Parameters.AddWithValue("@quantInteira", quantInteira);

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            movimentoEncontrado = Convert.ToBoolean(dr["MovimentoEncontrado"]);
                            produtoEncontrado = Convert.ToBoolean(dr["ProdutoEncontrado"]);
                            quantidadeIgual = Convert.ToBoolean(dr["QuantidadeIgual"]);
                        }
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
            return (movimentoEncontrado, produtoEncontrado, quantidadeIgual);
        }

        public int BuscarSituacaoMovimento(int idMovimento)
        {
            int situacaoMovimento = 0;

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    string mSQL = @"SELECT SITUACAO FROM MOVIMENTO WHERE ID_MOVIMENTO = @idMovimento AND tipo_movimento IN (1,2)";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@idMovimento", idMovimento);

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            situacaoMovimento = Convert.ToInt32(dr["SITUACAO"]);
                        }
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
            return situacaoMovimento;
        }

    }
}
