using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOWHeroes.DAO
{
    class GraphsDAO
    {
        private MySqlConnection dbConn;

        public GraphsDAO(MySqlConnection dbConn)
        {
            this.dbConn = dbConn;
        }

        public MySqlDataReader count(string procedure)
        {
            string query = string.Format("call {0}", procedure);
            MySqlCommand cmd = new MySqlCommand(query, dbConn);
            dbConn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public void close_conn()
        {
            dbConn.Close();
        }
    }
}
