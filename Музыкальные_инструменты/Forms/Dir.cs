using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Музыкальные_инструменты.Forms
{
    public partial class Dir : Form
    {
        public Dir()
        {
            InitializeComponent();
        }

        private void ClickMenu(object sender, EventArgs e)
        {
            Menu Menu = new Menu();
            this.Visible = false;
            Menu.ShowDialog();
        }

        private void ViewT()
        {
            try
            {

                DataTable dt1 = Class.Requests.SelectDB("category");
                dataGridView1.DataSource = dt1;

                DataTable dt2 = Class.Requests.SelectDB("unit");
                dataGridView2.DataSource = dt2;

                DataTable dt3 = Class.Requests.SelectDB("status");
                dataGridView3.DataSource = dt3;

                DataTable dt4 = Class.Requests.SelectDB("pickuppoint");
                dataGridView4.DataSource = dt4;

                DataTable dt5 = Class.Requests.SelectDB("Supplier");
                dataGridView5.DataSource = dt5;

            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            ViewT();
        }

        private void Dir_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            ViewT();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox6.Text == "") { return; }
                DataTable dt = Class.Requests.SelectDB("category", "CategoryName='" + textBox6.Text + "'");
                if (dt.Rows.Count == 0)
                {
                    bool InsertCategoryFlag = Class.Requests.InsertDB("category", "CategoryName", "'" + textBox6.Text + "'");
                    if (InsertCategoryFlag) { MessageBox.Show("Успешно добавлено"); }
                    else { MessageBox.Show("Неудалось добавить запись"); }
                }
                textBox6.Text = "";
                ViewT();
            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

     
     
    

        private void Dir_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "") { return; }
                DataTable dt = Class.Requests.SelectDB("unit", "UnitName='" + textBox1.Text + "'");
                if (dt.Rows.Count == 0)
                {
                    bool InsertCategoryFlag = Class.Requests.InsertDB("unit", "UnitName", "'" + textBox1.Text + "'");
                    if (InsertCategoryFlag) { MessageBox.Show("Успешно добавлено"); }
                    else { MessageBox.Show("Неудалось добавить запись"); }
                }
                textBox1.Text = "";
                ViewT();
            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text == "") { return; }
                DataTable dt = Class.Requests.SelectDB("status", "StatusName='" + textBox2.Text + "'");
                if (dt.Rows.Count == 0)
                {
                    bool InsertCategoryFlag = Class.Requests.InsertDB("status", "StatusName", "'" + textBox2.Text + "'");
                    if (InsertCategoryFlag) { MessageBox.Show("Успешно добавлено"); }
                    else { MessageBox.Show("Неудалось добавить запись"); }
                }
                textBox2.Text = "";
                ViewT();
            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text == "") { return; }
                DataTable dt = Class.Requests.SelectDB("pickuppoint", "PickupPointName='" + textBox3.Text + "'");
                if (dt.Rows.Count == 0)
                {
                    bool InsertCategoryFlag = Class.Requests.InsertDB("pickuppoint", "PickupPointName", "'" + textBox3.Text + "'");
                    if (InsertCategoryFlag) { MessageBox.Show("Успешно добавлено"); }
                    else { MessageBox.Show("Неудалось добавить запись"); }
                }
                textBox3.Text = "";
                ViewT();
            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text == "") { return; }
                DataTable dt = Class.Requests.SelectDB("Supplier", "SupplierName='" + textBox4.Text + "'");
                if (dt.Rows.Count == 0)
                {
                    bool InsertCategoryFlag = Class.Requests.InsertDB("Supplier", "SupplierName", "'" + textBox4.Text + "'");
                    if (InsertCategoryFlag) { MessageBox.Show("Успешно добавлено"); }
                    else { MessageBox.Show("Неудалось добавить запись"); }
                }
                textBox4.Text = "";
                ViewT();
            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

   
     
    }
}
