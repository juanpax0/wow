using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOWHeroes.DAO
{
    class UpdateHeroDAO
    {
        private MySqlConnection dbConn;

        public UpdateHeroDAO(MySqlConnection dbConn)
        {
            this.dbConn = dbConn;
        }

        public void UpdateHero(string o_name, string name, string race, 
            string sex, string faction)
        {
            string query = string.Format("call update_hero ('{0}', '{1}', '{2}', '{3}', '{4}')", o_name, name, race, sex, faction);
            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();
            cmd.ExecuteNonQuery();
            dbConn.Close();
        }

        public void close_conn()
        {
            dbConn.Close();
        }
    }
}
