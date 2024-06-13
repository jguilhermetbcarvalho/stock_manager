using AMAPA.Controllers;
using AMAPA.Models;
using AMAPA.Repositoryusing;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration; // Adicione este namespace para usar IConfiguration
using System;
using System.Collections.Generic;

namespace AMAPA.Repository
{
    public class DashboardRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _conexao;

        public DashboardRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _conexao = _configuration["ConectString"];
        }


        public int ContarRegistrosCarrinhoLancado()
        {
            int quantidadeRegistros = 0;

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    string mSQL = @"SELECT COUNT(*) FROM VW_SYSO_ESTOQUE_CARRINHO WHERE SITUACAO = '1 - LANCADO'";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    quantidadeRegistros = Convert.ToInt32(cmd.ExecuteScalar());
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

            return quantidadeRegistros;
        }

        public int ContarRegistrosCarrinhoDevolvido()
        {
            int quantidadeRegistros = 0;

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    string mSQL = @"SELECT COUNT(*) FROM VW_SYSO_ESTOQUE_CARRINHO WHERE datahora_ult_operacao BETWEEN " +
                        @"EXTRACT(MONTH FROM CURRENT_TIMESTAMP)||'/'|| " +
                        @"EXTRACT(DAY FROM CURRENT_TIMESTAMP)||'/'|| " +
                        @"EXTRACT(YEAR FROM CURRENT_TIMESTAMP)|| ' 00-00' " +
                        @"AND  EXTRACT(MONTH FROM CURRENT_TIMESTAMP)||'/' || " +
                        @"EXTRACT(DAY FROM CURRENT_TIMESTAMP)||'/' || " +
                        @"EXTRACT(YEAR FROM CURRENT_TIMESTAMP)|| ' 23-59' " +
                        @"AND SITUACAO = '3 - DEVOLVIDO' AND ID_MOVIMENTO is null";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    quantidadeRegistros = Convert.ToInt32(cmd.ExecuteScalar());
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

            return quantidadeRegistros;
        }

        public int ContarRegistrosEntregue()
        {
            int quantidadeRegistros = 0;

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    string mSQL = @"SELECT COUNT(*) FROM VW_SYSO_ESTOQUE_PROD_ENTREGUE WHERE DATA_ULT_OPERACAO BETWEEN " +
                        @"EXTRACT(MONTH FROM CURRENT_TIMESTAMP)||'/'|| " +
                        @"EXTRACT(DAY FROM CURRENT_TIMESTAMP)||'/'|| " +
                        @"EXTRACT(YEAR FROM CURRENT_TIMESTAMP)|| ' 00-00' " +
                        @"AND EXTRACT(MONTH FROM CURRENT_TIMESTAMP)||'/' || " +
                        @"EXTRACT(DAY FROM CURRENT_TIMESTAMP)||'/' || " +
                        @"EXTRACT(YEAR FROM CURRENT_TIMESTAMP)|| ' 23-59' ";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    quantidadeRegistros = Convert.ToInt32(cmd.ExecuteScalar());
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

            return quantidadeRegistros;
        }

        public int ContarRegistrosPendente()
        {
            int quantidadeRegistros = 0;

            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(_conexao))
            {
                try
                {
                    conexaoFireBird.Open();

                    string mSQL = @"SELECT COUNT(*) FROM VW_SYSO_ESTOQUE_PROD_PENDENTE";

                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);

                    quantidadeRegistros = Convert.ToInt32(cmd.ExecuteScalar());
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

            return quantidadeRegistros;
        }


    }
}
