using AMAPA.Controllers;
using AMAPA.Models;
using AMAPA.Repositoryusing;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AMAPA.Repository
{
    public class ProdutoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _conexao;

        public ProdutoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conexao = _configuration["ConectString"];
        }


        public List<Produtos> BuscarProdutoCodigoProdutoLista(string codigoProduto)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"SELECT ID_PRODUTO, CODIGO_BARRAS, CODIGO_FABRICANTE, DESCRICAO FROM VW_SYSO_ESTOQUE_PRODUTOS " +
                        @"WHERE ID_PRODUTO LIKE @codigoProduto";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@codigoProduto", "%" + codigoProduto + "%");

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        var lista = new List<Produtos>();
                        while (dr.Read())
                        {
                            try
                            {
                                Produtos p = new Produtos();
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.CODIGO_FABRICANTE = Convert.ToString(dr["CODIGO_FABRICANTE"]);
                                p.DESCRICAO = Convert.ToString(dr["DESCRICAO"]);

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
        public List<Produtos> BuscarProdutoCodigoBarrasLista(string codigoBarras)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"SELECT ID_PRODUTO, CODIGO_BARRAS, CODIGO_FABRICANTE, DESCRICAO FROM VW_SYSO_ESTOQUE_PRODUTOS " +
                        @"WHERE CODIGO_BARRAS LIKE @codigoBarras OR CODIGO_BARRAS_2 LIKE @codigoBarras";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@codigoBarras", "%" + codigoBarras + "%");

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        var lista = new List<Produtos>();
                        while (dr.Read())
                        {
                            try
                            {
                                Produtos p = new Produtos();
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.CODIGO_FABRICANTE = Convert.ToString(dr["CODIGO_FABRICANTE"]);
                                p.DESCRICAO = Convert.ToString(dr["DESCRICAO"]);

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
        public List<Produtos> BuscarProdutoCodigoFabricanteLista(string codigoFabricante)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"SELECT ID_PRODUTO, CODIGO_BARRAS, CODIGO_FABRICANTE, DESCRICAO FROM VW_SYSO_ESTOQUE_PRODUTOS " +
                        @"WHERE CODIGO_FABRICANTE LIKE @codigoFabricante";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@codigoFabricante", "%" + codigoFabricante + "%");

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        var lista = new List<Produtos>();
                        while (dr.Read())
                        {
                            try
                            {
                                Produtos p = new Produtos();
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.CODIGO_FABRICANTE = Convert.ToString(dr["CODIGO_FABRICANTE"]);
                                p.DESCRICAO = Convert.ToString(dr["DESCRICAO"]);

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




        public Produtos BuscarProdutoCodigoProdutoIndividual(string codigoProduto)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"select * from " +
                        @"VW_SYSO_ESTOQUE_PRODUTOS where ID_PRODUTO = @codigoProduto";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@codigoProduto", codigoProduto);

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        Produtos p = new Produtos();
                        if (dr.Read())
                        {
                            try
                            {
                                p.ID_EMPRESA = Convert.ToInt32(dr["ID_EMPRESA"]);
                                p.ID_FILIAL = Convert.ToInt32(dr["ID_FILIAL"]);
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.CODIGO_FABRICANTE = Convert.ToString(dr["CODIGO_FABRICANTE"]);
                                p.ID_DESCRICAO = Convert.ToInt32(dr["ID_DESCRICAO"]);
                                p.DESCRICAO = Convert.ToString(dr["DESCRICAO"]);
                                p.LOCALIZACAO = Convert.ToString(dr["LOCALIZACAO"]);
                                p.ESTOQUE_ATUAL = Convert.ToInt32(dr["ESTOQUE_INTEIRO"]);

                            }
                            catch (Exception)
                            {
                            }
                        }
                        return p;
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

        public Produtos BuscarProdutoCodigoBarrasIndividual(string codigoBarras)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"select ID_PRODUTO, CODIGO_BARRAS, CODIGO_FABRICANTE, DESCRICAO, LOCALIZACAO, ESTOQUE_INTEIRO from VW_SYSO_ESTOQUE_PRODUTOS " +
                        @"where CODIGO_BARRAS = @codigoBarras OR CODIGO_BARRAS_2 LIKE @codigoBarras";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@codigoBarras", codigoBarras);

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        Produtos p = new Produtos();
                        if (dr.Read())
                        {
                            try
                            {
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.CODIGO_FABRICANTE = Convert.ToString(dr["CODIGO_FABRICANTE"]);
                                p.DESCRICAO = Convert.ToString(dr["DESCRICAO"]);
                                p.LOCALIZACAO = Convert.ToString(dr["LOCALIZACAO"]);
                                p.ESTOQUE_ATUAL = Convert.ToInt32(dr["ESTOQUE_INTEIRO"]);

                            }
                            catch (Exception)
                            {
                            }
                        }
                        return p;
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

        public Produtos BuscarProdutoCodigoFabricanteIndividual(string codigoFabricante)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"SELECT ID_PRODUTO, CODIGO_BARRAS, CODIGO_FABRICANTE, DESCRICAO, LOCALIZACAO, ESTOQUE_INTEIRO  FROM VW_SYSO_ESTOQUE_PRODUTOS " +
                        @"WHERE CODIGO_FABRICANTE = @codigoFabricante";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@codigoFabricante", codigoFabricante);

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        Produtos p = new Produtos();
                        if (dr.Read())
                        {
                            try
                            {
                                p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                p.CODIGO_BARRAS = Convert.ToString(dr["CODIGO_BARRAS"]);
                                p.CODIGO_FABRICANTE = Convert.ToString(dr["CODIGO_FABRICANTE"]);
                                p.DESCRICAO = Convert.ToString(dr["DESCRICAO"]);
                                p.LOCALIZACAO = Convert.ToString(dr["LOCALIZACAO"]);
                                p.ESTOQUE_ATUAL = Convert.ToInt32(dr["ESTOQUE_INTEIRO"]);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        return p;
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



        public IList<CodBarrasDivergente> BuscarProdutosCodBarrasDivergente() {
            {
                using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
                {
                    try
                    {
                        conexaoFireBird.Open();

                        string mSQL = @"SELECT formatdatetime('dd/mm/yyyy', DATAHORA) AS DATA, ID_PRODUTO, " +
                            @"COD_BARRAS_GESTOR_ESTOQUE, LOCALIZACAO " +
                            @"FROM SYSO_ESTOQUE_COD_BARRAS WHERE ALTERADO = 0";

                        FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                        using (FbDataReader dr = cmd.ExecuteReader())
                        {
                            var lista = new List<CodBarrasDivergente>();
                            while (dr.Read())
                            {
                                try
                                {
                                    CodBarrasDivergente p = new CodBarrasDivergente();
                                    p.ID_PRODUTO = Convert.ToString(dr["ID_PRODUTO"]);
                                    p.COD_BARRAS_GESTOR = Convert.ToString(dr["COD_BARRAS_GESTOR_ESTOQUE"]);
                                    p.DATAHORA = Convert.ToString(dr["DATA"]);
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
        }

        public void ConfirmarAlteracaoProduto(string idProduto)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"UPDATE SYSO_ESTOQUE_COD_BARRAS SET ALTERADO = 1 WHERE ID_PRODUTO = @idProduto";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);

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

        public List<string> BuscarCodigoBarrasProduto(string idProduto)
        {
            List<string> codigosDeBarras = new List<string>(); // Lista para armazenar os códigos de barras

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"SELECT CODIGO_BARRAS FROM VW_SYSO_ESTOQUE_COD_BARRAS WHERE ID_PRODUTO = @idProduto;";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);

                    FbDataReader reader = cmd.ExecuteReader(); // Executar a consulta e obter um leitor de dados

                    while (reader.Read()) // Ler os resultados da consulta
                    {
                        string codigoBarras = reader["CODIGO_BARRAS"].ToString(); // Ler o valor do código de barras
                        codigosDeBarras.Add(codigoBarras); // Adicionar o código de barras à lista
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

            return codigosDeBarras; // Retorna a lista de códigos de barras
        }


        public void VerificarCodBarras(string idProduto, string loc, string codBarrasGestor)
        {
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = @"EXECUTE PROCEDURE SP_SYSO_ESTOQUE_COD_BARRAS(@idProduto, @codBarrasGestor, @loc);";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.Parameters.AddWithValue("@idProduto", idProduto);
                    cmd.Parameters.AddWithValue("@loc", loc);
                    cmd.Parameters.AddWithValue("@codBarrasGestor", codBarrasGestor);
                    
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
