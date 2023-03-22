using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Музыкальные_инструменты.Forms
{
    public partial class View : Form
    {
        public View()
        {
            InitializeComponent();
        }

        public static string s = "ProductCategory>0";
        public static string Filtrtitle = "ProductCost";
        public static string Filtr = "ASC";
        static double min = 0;
        static double max = 100;
        public static Dictionary<string, int> ProductO = new Dictionary<string, int>();

        private void ViewDataTable()
        {
            try
            {

                DataTable dt = Class.Requests.SelectInnerJoin("product", "category", "product.ProductCategory=category.CategoryID", s + " and ProductDiscountAmount between " + min + " and " + max + " Order by " + Filtrtitle + " " + Filtr);
                DataTable dt1 = Class.Requests.SelectDB("product");
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Данных нет");
                    label3.Text = dt.Rows.Count + " из " + dt1.Rows.Count;
                    DataTable cl = new DataTable();
                    dataGridView1.Columns.Clear();
                    return;
                }
                dt.Columns["ProductArticleNumber"].ColumnName = "Артикул";
                dt.Columns["ProductName"].ColumnName = "Имя";
                dt.Columns["CategoryName"].ColumnName = "Категория";
                dt.Columns["ProductManufacturer"].ColumnName = "Производитель";
                dt.Columns["ProductCost"].ColumnName = "Цена";
                dt.Columns["ProductDiscountAmount"].ColumnName = "Скидка";
                dataGridView1.DataSource = dt;
                for (int j = 2; j <= 12; j++) {
                    dataGridView1.Columns[j].Visible = false;
                
                }
                //dataGridView1.Columns[2].Visible = false;

                //dataGridView1.Columns[3].Visible = false;
                //dataGridView1.Columns[4].Visible = false;
                //dataGridView1.Columns[8].Visible = false;
                //dataGridView1.Columns[9].Visible = false;
                //dataGridView1.Columns[10].Visible = false;
                //dataGridView1.Columns[11].Visible = false;
                //dataGridView1.Columns[12].Visible = false;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells["Скидка"].Value) >= 15)
                    {
                        dataGridView1.Rows[i].Cells["Скидка"].Style.BackColor = Color.Chartreuse;
                    }


                }

                label3.Text = dt.Rows.Count + " из " + dt1.Rows.Count;
                dataGridView1.ClearSelection();


            }
            catch (Exception) { MessageBox.Show("Ошибка вывода бд"); }
        }

        private void Add(object sender, EventArgs e)
        {
            try
            {

                int id = dataGridView1.CurrentRow.Index;
                if (id < 0) { return; }
                string art = dataGridView1.Rows[id].Cells[0].Value.ToString();
                if (ProductO.ContainsKey(art))
                {
                    ProductO[art] = ProductO[art] + 1;
                }
                else
                {
                    ProductO.Add(art, 1);
                }
                if (ProductO.Count > 0) { button2.Visible = true; }
                dataGridView1.ClearSelection();
            }
            catch (Exception) { MessageBox.Show("Ошибка добавления продукта в корзину"); }
        }
        private void View_Load(object sender, EventArgs e)
        {
            //if (Auth.Role <= 1)
            //{
            //    button1.Text = "Выйти";
            //}
            //else
            //{
            //    button1.Text = "В меню";
            //}
            s = "ProductCategory>0";
            Filtrtitle = "ProductCost";
            Filtr = "ASC";
            min = 0;
            max = 100;
            ViewDataTable();
            string path = Directory.GetCurrentDirectory();
            path += "\\Image\\picture.png";
            Image img = new Bitmap(path);
            pictureBox1.Image = img;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            textBox1.Text = "";
            button2.Visible = false;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            toolStripMenuItem1.Click += Add;
            ProductO.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = dataGridView1.CurrentRow.Index;
                string art = dataGridView1.Rows[id].Cells[0].Value.ToString();
                DataTable dt = Class.Requests.SelectDB("product", "ProductArticleNumber='" + art + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    textBox2.Text = dr["ProductArticleNumber"].ToString();
                    textBox3.Text = dr["ProductName"].ToString();
                    textBox5.Text = dr["ProductManufacturer"].ToString();
                    textBox9.Text = dr["ProductCost"].ToString();
                    textBox7.Text = dr["ProductDiscountAmount"].ToString();
                    textBox8.Text = dr["ProductDiscount"].ToString();
                    textBox6.Text = dr["ProductQuantityInStock"].ToString();
                    textBox4.Text = dr["ProductDescription"].ToString();
                    DataTable dt1 = Class.Requests.SelectDB("category", "CategoryID=" + dr["ProductCategory"].ToString());
                    DataTable dt2 = Class.Requests.SelectDB("supplier", "SupplierID=" + dr["ProductSupplier"].ToString());
                    DataTable dt3 = Class.Requests.SelectDB("unit", "UnitID=" + dr["ProductUnit"].ToString());


                    string path = Directory.GetCurrentDirectory();
                    if (dr["ProductPhoto"].ToString() == "")
                    {
                        path += "\\Image\\picture.png";
                    }
                    else
                    {
                        path += "\\Image\\" + dr["ProductPhoto"].ToString();
                    }
                    Image img = new Bitmap(path);
                    pictureBox1.Image = img;
                    textBox10.Text = dt1.Rows[0]["CategoryName"].ToString();
                    textBox12.Text = dt2.Rows[0]["SupplierName"].ToString();
                    textBox11.Text = dt3.Rows[0]["UnitName"].ToString();
                }


            }
            catch (Exception) { MessageBox.Show("Ошибка выбора строки"); }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex) {
                case 0:
                    {
                        min = 0; 
                        max = 100;
                        break;
                    }
                case 1:
                    {
                        min = 0;
                        max = 10;
                        break;
                    }
                case 2:
                    {
                        min = 10; 
                        max = 15;
                        break;
                    }
                case 3:
                    {
                        min = 15; 
                        max = 100;
                        break;
                    }

            }
            ViewDataTable();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) { Filtr = "ASC"; }
            else { Filtr = "DESC"; }
            ViewDataTable();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { s = "ProductCategory>0"; }
            else { s = "ProductName Like '%" + textBox1.Text + "%'"; }
            ViewDataTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Auth.UserID == 3)
            {
                Menu Menu = new Menu();
                this.Visible = false;
                Menu.ShowDialog();
                
            }
            else
            {
                Auth Auth1 = new Auth();
                this.Visible = false;
                Auth1.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Order Order = new Order();
            this.Visible = false;
            Order.ShowDialog();
            if (ProductO.Count > 0) { button2.Visible = true; }
            else { button2.Visible = false; }
            this.Visible = true;
        }

        private void View_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

       
    }
}
