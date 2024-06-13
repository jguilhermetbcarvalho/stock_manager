using FirebirdSql.Data.FirebirdClient;
using System.Configuration;

namespace AMAPA.Repository﻿using
{
    public class AcessoFB
    {
        private static readonly AcessoFB instanciaFireBird = new AcessoFB();

        #region Conexao
        public static AcessoFB GetInstancia()
        {
            return instanciaFireBird;
        }


        public FbConnection GetConexao(string conect)
        {
            var senha = "User=SYSDBA;Password=vectordba;";
            string conn = senha + conect;
            var conexao = conn;
            return new FbConnection(conexao);
        }


        public bool TestarConexao(string conect)
        {
            bool boll = false;
            using (FbConnection conexaoFireBird = AcessoFB.GetInstancia().GetConexao(conect))
            {
                try
                {
                    conexaoFireBird.Open();
                    boll = true;
                    conexaoFireBird.Close();
                }
                catch (Exception)
                {
                    return boll = false;
                }
            }
            return boll;
        }
        #endregion
    }
}
