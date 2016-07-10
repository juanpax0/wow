using MySql.Data.MySqlClient;
using WOWHeroes.connection;
using WOWHeroes.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WOWHeroes.view
{
    public partial class Graphs : Form
    {
        private GraphsDAO dbConn;

        public Graphs(MySqlConnection dbConn)
        {
            InitializeComponent();
            this.dbConn = new GraphsDAO(dbConn);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Object ob = comboBox1.SelectedItem;

            if (ob != null)
            {
                var si = ob.ToString();
                chart1.Series[0].Points.Clear();

                if (si.Equals("RAZA"))
                {
                    chart1.Series[0].ChartType = 
                        System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

                    race();
                }
                else
                {
                    chart1.Series[0].ChartType = 
                        System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

                    if (si.Equals("SEXO"))
                        sex();
                    else if (si.Equals("FACCION"))
                        faction();
                }

                dbConn.close_conn();
            }
        }

        private void race()
        {
            MySqlDataReader reader = dbConn.count("count_raz");
      
            while (reader.Read())
            {
                string race = reader["RAZA"].ToString();
                int count = Int32.Parse(reader["counter"].ToString());

                chart1.Series[0].Points.AddXY(race, count);
            }
        }

        private void sex()
        {
            MySqlDataReader reader = dbConn.count("count_sex");

            while (reader.Read())
            {
                string sex = reader["SEXO"].ToString();
                int count = Int32.Parse(reader["counter"].ToString());

                chart1.Series[0].Points.AddXY(sex, count);
            }
        }

        private void faction()
        {
            MySqlDataReader reader = dbConn.count("count_fac");

            while (reader.Read())
            {
                string sex = reader["FACCION"].ToString();
                int count = Int32.Parse(reader["counter"].ToString());

                chart1.Series[0].Points.AddXY(sex, count);
            }
        }
    }
}

