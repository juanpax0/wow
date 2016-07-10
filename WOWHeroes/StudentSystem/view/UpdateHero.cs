using MySql.Data.MySqlClient;
using WOWHeroes.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WOWHeroes.view
{
    public partial class UpdateHero : Form
    {
        private string name;
        private UpdateHeroDAO dbConn;
        private static Regex regex = new Regex("^[0-9]+$");

        public UpdateHero(MySqlConnection conn)
        {
            InitializeComponent();
            dbConn = new UpdateHeroDAO(conn);
        }

        public void setComponents(string name, string race, string sex, string faction)
        {
            this.name = name;

            textBox1.Text = name;
            comboBox1.SelectedItem = race;
            comboBox2.SelectedItem = faction;

            if (checkBox1.Text.Equals(sex))
                checkBox1.Checked = true;
            else
                checkBox2.Checked = true;
        }

        public void button1_Click()
        {
            Object rac_ = comboBox1.SelectedItem;
            Object fac_ = comboBox2.SelectedItem;
            bool m = checkBox1.Checked;
            bool f = checkBox2.Checked;
            string nam = Regex.Replace(textBox1.Text.Trim().ToUpper(), @"\s+", " ");

            if ((m || f) && rac_ != null && fac_ != null && !nam.Equals(""))
            {
                DialogResult dialogResult =
                    MessageBox.Show("¿Desea actualizar los datos?",
                    "¿Seguro?", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    string rac = rac_.ToString();
                    string sex = (m ? "M" : "F");
                    string fac = fac_.ToString();

                    try
                    {
                        dbConn.UpdateHero(name, nam, rac, sex, fac);
                        Hide();
                    }
                    catch (Exception e)
                    {
                        dbConn.close_conn();
                        error_message();
                    }

                }
            }
            else
            {
                error_message();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                checkBox2.Checked = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                checkBox1.Checked = false;
        }

        private void error_message()
        {
            DialogResult dialogResult =
                MessageBox.Show("Verifique que los datos sean correctos", "Ocurrio un error",
                MessageBoxButtons.OK);
        }
    }
}
