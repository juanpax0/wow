using MySql.Data.MySqlClient;
using WOWHeroes.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WOWHeroes.connection
{
    public class Connection
    {
        private const string SERVER = "127.0.0.1";
        private const string DATABASE = "wow_heroes";
        private const string UID = "root";
        private const string PASSWORD = "root";

        public Connection() { }

        public MySqlConnection getConnection() {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = SERVER;
            builder.UserID = UID;
            builder.Password = PASSWORD;
            builder.Database = DATABASE;

            string connString = builder.ToString();

            builder = null;

            MySqlConnection dbConn = new MySqlConnection(connString);

            Application.ApplicationExit += (sender, args) =>
            {
                if (dbConn != null)
                {
                    dbConn.Dispose();
                    dbConn = null;
                }
            };

            return dbConn;
        } 
    }
}
