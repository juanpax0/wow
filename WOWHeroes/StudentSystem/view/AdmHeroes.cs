using MySql.Data.MySqlClient;
using WOWHeroes.DAO;
using WOWHeroes.model;
using WOWHeroes.view;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace WOWHeroes
{
    public partial class AdmHeroes : Form
    {
        private AdmHeroesDAO dbConn;
        private UpdateHero update;
        private Graphs graph;
        private static Regex regex = new Regex("^[0-9]+$");
        private static int hW, hH, nW, nH;

        public AdmHeroes(MySqlConnection con)
        {
            dbConn = new AdmHeroesDAO(con);
            update = new UpdateHero(con);
            graph = new Graphs(con);
            InitializeComponent();
            init();
        }

        private void init()
        {
            update.button1.Click += new EventHandler(this.update_method);
            pictureBox1.Image = Image.FromFile("../../view/imgs/search.png");
            pictureBox2.Image = Image.FromFile("../../view/imgs/g5.png");
            pictureBox3.Image = Image.FromFile("../../view/imgs/excel.png");

            hW = pictureBox2.Width;
            hH = pictureBox2.Height;
            nW = hW + ((hW * 5) / 100);
            nH = hH + ((hH * 5) / 100);
            get_heroes();
        }

        private void get_heroes()
        {
            listView1.Items.Clear();

            List<Hero> hrs = dbConn.GetHeroes();

            foreach (Hero h in hrs)
            {
                string nam = h.name;
                string rac = h.race;
                string sex = h.sex;
                string fac = h.faction;

                ListViewItem item = new ListViewItem(new[] { nam, rac, sex, fac });
                listView1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Object rac_ = race.SelectedItem;
            Object fac_ = faction.SelectedItem;
            bool m = checkBox1.Checked;
            bool f = checkBox2.Checked;
            string nam = Regex.Replace(name.Text.Trim().ToUpper(), @"\s+", " ");

            if ((m || f) && rac_ != null && fac_ != null && !nam.Equals(""))
            {
                DialogResult dialogResult =
                    MessageBox.Show("¿Desea agregar al heroe?",
                    "¿Seguro?", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    string rac = rac_.ToString();
                    string sex = (m ? "M" : "F");
                    string fac = fac_.ToString();

                    try
                    {
                        dbConn.InsertHero(nam, rac, sex, fac);
                        get_heroes();
                    }
                    catch (Exception ex)
                    {
                        dbConn.close_conn();
                        DialogResult dialogError =
                            MessageBox.Show("Verifique que el heroe no se encuentre previamente registrado",
                            "Ocurrio un error", MessageBoxButtons.OK);
                    }
                }
            }
            else
            {
                DialogResult dialogResult =
                    MessageBox.Show("Verifique que los datos sean correctos",
                    "Ocurrio un error",
                    MessageBoxButtons.OK);
            }
        }

        private void update_method(object sender, EventArgs e)
        {
            update.button1_Click();
            get_heroes();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            string name = textBox4.Text;
            if (name.Equals(""))
            {
                get_heroes();
            }
            else
            {
                List<Hero> hrs = dbConn.SeekHero(name);

                foreach (Hero h in hrs)
                {
                    string nam = h.name;
                    string rac = h.race;
                    string sex = h.sex;
                    string fac = h.faction;

                    ListViewItem item = new ListViewItem(new[] { nam, rac, sex, fac });
                    listView1.Items.Add(item);
                }
            }
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = listView1.SelectedItems.Count;

            if (count > 0)
            {
                if (count == 1)
                {
                    int index = listView1.SelectedItems[0].Index;
                    ListViewItem sRow = listView1.Items[index];
                    string nam = sRow.SubItems[0].Text;
                    string rac = sRow.SubItems[1].Text;
                    string sex = sRow.SubItems[2].Text;
                    string fac = sRow.SubItems[3].Text;

                    update.setComponents(nam, rac, sex, fac);
                    update.ShowDialog();
                }
                else
                {
                    DialogResult dialogResult =
                        MessageBox.Show("Debe actualizar uno a la vez",
                        "Ocurrio un error",
                        MessageBoxButtons.OK);
                }
            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = listView1.SelectedItems.Count;

            if (count > 0)
            {
                DialogResult dialogResult =
                    MessageBox.Show("¿Desea eliminar los heroes seleccionados?", "¿Seguro?",
                    MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    while (count > 0)
                    {
                        int index = listView1.SelectedItems[0].Index;
                        ListViewItem sRow = listView1.Items[index];
                        string nam = sRow.SubItems[0].Text;

                        dbConn.DeleteHero(nam);
                        listView1.Items.RemoveAt(index);

                        richTextBox1.Text += "Eliminando el heroe " + nam + "\n";
                        count--;
                    }
                    get_heroes();
                }
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

        // menu
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            graph.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog1 = new SaveFileDialog();
            saveDialog1.InitialDirectory = "c:";
            saveDialog1.Title = "Save Report File";
            saveDialog1.FileName = "";
            saveDialog1.Filter = "Excel Files|*.xlsx";
            if (saveDialog1.ShowDialog() != DialogResult.Cancel)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                for (int i = 1; i < listView1.Columns.Count + 1; i++)
                {
                    xlWorkSheet.Cells[1, i] = listView1.Columns[i - 1].Text;
                }

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    for (int j = 0; j < listView1.Columns.Count; j++)
                    {
                        xlWorkSheet.Cells[i + 2, j + 1] = listView1.Items[i].SubItems[j].Text;
                    }

                }
                xlWorkBook.SaveCopyAs(saveDialog1.FileName.ToString());
                xlWorkBook.Saved = true;

                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
            }
        }

        // Este metodo es como para liberar espacio. Se llama al GC.
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Width = hW;
            pictureBox3.Height = hH;
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Width = nW;
            pictureBox3.Height = nH;
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Width = nW;
            pictureBox2.Height = nH;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Width = hW;
            pictureBox2.Height = hH;
        }
    }
}
