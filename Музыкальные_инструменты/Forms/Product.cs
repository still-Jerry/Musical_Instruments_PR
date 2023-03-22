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
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
        }

        public static string img = "picture.png";




        private void ViewDataTable()
        {
            try
            {

                DataTable dt = Class.Requests.SelectInnerJoin("product", "category", "product.ProductCategory=category.CategoryID");
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Данных нет");
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
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.ClearSelection();
            }
            catch (Exception) { MessageBox.Show("Ошибка вывода бд"); }
        }

        private void ClearForm()
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                path += "\\Image\\picture.png";
                Image img = new Bitmap(path);
                pictureBox1.Image = img;
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
            }
            catch (Exception) { MessageBox.Show("Ошибка очистки формы"); }
        }

        private void Product_Load(object sender, EventArgs e)
        {
            ViewDataTable();
            ClearForm();

            DataTable cat = Class.Requests.SelectDB("Category");
            DataTable unit = Class.Requests.SelectDB("Unit");
            DataTable supplier = Class.Requests.SelectDB("Supplier");
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            foreach (DataRow dr in cat.Rows)
            {
                comboBox1.Items.Add(dr["CategoryName"].ToString());
            }

            foreach (DataRow dr in unit.Rows)
            {
                comboBox2.Items.Add(dr["UnitName"].ToString());
            }

            foreach (DataRow dr in supplier.Rows)
            {
                comboBox3.Items.Add(dr["SupplierName"].ToString());
            }
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
                    comboBox1.Text = dt1.Rows[0]["CategoryName"].ToString();
                    comboBox3.Text = dt2.Rows[0]["SupplierName"].ToString();
                    comboBox2.Text = dt3.Rows[0]["UnitName"].ToString();
                }


            }
            catch (Exception) { MessageBox.Show("Ошибка выбора строки"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menu Menu = new Menu();
            this.Visible = false;
            Menu.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int id = dataGridView1.CurrentRow.Index;
                string art = dataGridView1.Rows[id].Cells[0].Value.ToString();
                DialogResult dr = MessageBox.Show("Удалить запись с артикулом " + art + "?", "Музыкальные_инструменты", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    bool deleteFlag = Class.Requests.DeleteBD("Product", "ProductArticleNumber='" + art + "'");
                    if (deleteFlag) { MessageBox.Show("Успешно удалено"); }
                    else { MessageBox.Show("Неудалось удалить запись"); }
                }
                ViewDataTable();
                ClearForm();
            }
            catch (Exception) { MessageBox.Show("Ошибка удаления"); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox9.Text == "" || textBox6.Text == "")
                {
                    MessageBox.Show("Почти все текстовые поля обязательны для заполнения");
                    return;
                }

                bool u = true;
                DataTable ProductArticleNumber = Class.Requests.SelectDB("Product", "", "ProductArticleNumber");
                foreach (DataRow dr in ProductArticleNumber.Rows)
                {
                    if (textBox2.Text == dr[0].ToString()) { u = false; break; }
                }
                if (!u) { MessageBox.Show("Артикул должен быть уникальным"); return; }

                if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
                {
                    MessageBox.Show("Все выпадающие списки обязательны для заполнения");
                    return;
                }
                DataTable cat = Class.Requests.SelectDB("Category", "CategoryName='" + comboBox1.Text + "'");
                DataTable unit = Class.Requests.SelectDB("Unit", "UnitName='" + comboBox2.Text + "'");
                DataTable supplier = Class.Requests.SelectDB("Supplier", "SupplierName='" + comboBox3.Text + "'");

                string VALUES = "'" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "'," + cat.Rows[0][0].ToString() + ",'" + img + "','" + textBox5.Text + "'," + textBox9.Text.Replace(',', '.') + ",";
                if (textBox7.Text == "")
                {
                    VALUES += "0,";
                }
                else
                {
                    VALUES += textBox7.Text + ",";
                }
                VALUES += textBox6.Text + ",";
                if (textBox8.Text == "")
                {
                    VALUES += "0,";
                }
                else
                {
                    VALUES += textBox8.Text + ",";
                }
                VALUES += supplier.Rows[0][0].ToString() + "," + unit.Rows[0][0].ToString();
                string Attributes = "ProductArticleNumber, ProductName, ProductDescription, ProductCategory, ProductPhoto, ProductManufacturer, ProductCost, ProductDiscountAmount, ProductQuantityInStock, ProductDiscount, ProductSupplier, ProductUnit";
                bool InsertFlag = Class.Requests.InsertDB("Product", Attributes, VALUES);
                if (InsertFlag) { MessageBox.Show("Успешно добавлено"); }
                else { MessageBox.Show("Неудалось добавить запись"); }
                ViewDataTable();
                ClearForm();
            }
            catch (Exception) { MessageBox.Show("Ошибка добавления"); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { return; }
                string filename = openFileDialog1.FileName;
                string[] Comfile = filename.Split('\\');
                img = Comfile[Comfile.Length - 1];

                string path = Directory.GetCurrentDirectory();
                FileInfo fileInfo1 = new FileInfo(filename);
                FileInfo fileInfo2 = new FileInfo(path + "\\Image\\" + img);
                if (fileInfo1.Exists && !fileInfo2.Exists)
                {
                    fileInfo1.CopyTo(path + "\\Image\\" + img);
                }
                else
                {
                    MessageBox.Show("Такое фото уже есть");
                }
                pictureBox1.Image = new Bitmap(path + "\\Image\\" + img);
            }
            catch (Exception) { MessageBox.Show("Ошибка добавления фото"); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int id = dataGridView1.CurrentRow.Index;
                string art = dataGridView1.Rows[id].Cells[0].Value.ToString();

                if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox9.Text == "" || textBox6.Text == "")
                {
                    MessageBox.Show("Почти все текстовые поля обязательны для заполнения");
                    return;
                }

                bool u = true;
                DataTable ProductArticleNumber = Class.Requests.SelectDB("Product", "", "ProductArticleNumber");
                foreach (DataRow dr in ProductArticleNumber.Rows)
                {
                    if (textBox2.Text == dr[0].ToString() && textBox2.Text != art) { u = false; break; }
                }
                if (!u) { MessageBox.Show("Артикул должен быть уникальным"); return; }

                if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
                {
                    MessageBox.Show("Все выпадающие списки обязательны для заполнения");
                    return;
                }

                DataTable cat = Class.Requests.SelectDB("Category", "CategoryName='" + comboBox1.Text + "'");
                DataTable unit = Class.Requests.SelectDB("Unit", "UnitName='" + comboBox2.Text + "'");
                DataTable supplier = Class.Requests.SelectDB("Supplier", "SupplierName='" + comboBox3.Text + "'");

                string VALUES = "ProductArticleNumber='" + textBox2.Text + "', ProductName='" + textBox3.Text + "', ProductDescription='" + textBox4.Text + "', ProductCategory=" + cat.Rows[0][0].ToString() + ", ProductPhoto='" + img + "', ProductManufacturer='" + textBox5.Text + "', ProductCost=" + textBox9.Text.Replace(',', '.') + ",";
                if (textBox7.Text == "")
                {
                    VALUES += " ProductDiscountAmount=0,";
                }
                else
                {
                    VALUES += " ProductDiscountAmount=" + textBox7.Text + ",";
                }
                VALUES += " ProductQuantityInStock=" + textBox6.Text + ",";
                if (textBox8.Text == "")
                {
                    VALUES += " ProductDiscount=0,";
                }
                else
                {
                    VALUES += " ProductDiscount=" + textBox8.Text + ",";
                }
                VALUES += " ProductSupplier=" + supplier.Rows[0][0].ToString() + ", ProductUnit=" + unit.Rows[0][0].ToString();


                bool updata = Class.Requests.UpdateBD("Product", VALUES, "ProductArticleNumber='" + art + "'");
                if (updata) { MessageBox.Show("Успешно"); }
                else { MessageBox.Show("Не удачно"); }
                ViewDataTable();
                ClearForm();
            }
            catch (Exception) { MessageBox.Show("Ошибка редактирования"); }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'z') || e.KeyChar == 8 || (e.KeyChar >= '1' && e.KeyChar <= '9') || e.KeyChar == '0')
            {
                e.Handled = false;
            }
            else { e.Handled = true; }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'А' && e.KeyChar <= 'я') || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else { e.Handled = true; }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '1' && e.KeyChar <= '9') || e.KeyChar == '0' || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else { e.Handled = true; }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '1' && e.KeyChar <= '9') || e.KeyChar == '0' || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else { e.Handled = true; }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '1' && e.KeyChar <= '9') || e.KeyChar == '0' || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else { e.Handled = true; }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '1' && e.KeyChar <= '9') || e.KeyChar == '0' || e.KeyChar == ',' || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else { e.Handled = true; }
        }

        private void Product_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }



    }
}
