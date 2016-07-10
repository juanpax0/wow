using MySql.Data.MySqlClient;
using WOWHeroes.connection;
using WOWHeroes.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WOWHeroes.DAO
{
    class AdmHeroesDAO
    {
        public MySqlConnection dbConn;

        public AdmHeroesDAO(MySqlConnection dbConn)
        {
            this.dbConn = dbConn;
        }

        public List<Hero> GetHeroes()
        {
            List<Hero> heroes = new List<Hero>();
            string query = "call get_heroes";
            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string nam = reader["nombre"].ToString();
                string rac = reader["raza"].ToString();
                string sex = reader["sexo"].ToString();
                string fac = reader["faccion"].ToString();

                Hero h = new Hero(nam, rac, sex, fac);
                heroes.Add(h);
            }

            reader.Close();
            dbConn.Close();

            return heroes;
        }

        public void InsertHero(string name, string race, string sex, string faction)
        {
            string query = string.Format("call insert_hero ('{0}', '{1}', '{2}', '{3}')", name, race, sex, faction);
            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();
            cmd.ExecuteNonQuery();
            dbConn.Close();
        }

        public void DeleteHero(string name)
        {
            string query = string.Format("call delete_hero ('{0}')", name);
            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();
            cmd.ExecuteNonQuery();
            dbConn.Close();
        }

        public List<Hero> SeekHero(string name)
        {
            List<Hero> heroes = new List<Hero>();
            string query = string.Format("call seek_hero ('%{0}%')", name);
            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string nam = reader["nombre"].ToString();
                string rac = reader["raza"].ToString();
                string sex = reader["sexo"].ToString();
                string fac = reader["faccion"].ToString();

                Hero h = new Hero(nam, rac, sex, fac);
                heroes.Add(h);
            }
            reader.Close();
            dbConn.Close();

            return heroes;
        }
        public void close_conn()
        {
            dbConn.Close();
        }
    }
}
