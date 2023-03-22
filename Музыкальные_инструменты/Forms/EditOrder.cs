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
    public partial class EditOrder : Form
    {
        public EditOrder()
        {
            InitializeComponent();
        }

        static int min = 0;
        static int max = 100;
        bool Sort = true;
        string IDorder = "";

        private void EditOrderView()
        {
            try
            {
                dataGridView1.Rows.Clear();
                DataTable Order = Class.Requests.SelectDB("Order");
                if (Order.Rows.Count == 0) { MessageBox.Show("Данных нет"); return; }
                foreach (DataRow drOrder in Order.Rows)
                {
                    DataTable dtOrderProduct = Class.Requests.SelectDB("OrderProduct", "OrderProduct.OrderID=" + drOrder["OrderID"].ToString());
                    string sostav = "";
                    DataTable dtUser = Class.Requests.SelectDB("User", "UserID=" + drOrder["OrderUser"].ToString());
                    string user = dtUser.Rows[0]["UserSurname"].ToString() + " " + dtUser.Rows[0]["UserName"].ToString() + " " + dtUser.Rows[0]["UserPatronymic"].ToString();
                    double cost = 0, costD = 0;
                    string Article = "";
                    foreach (DataRow drOrderProduct in dtOrderProduct.Rows)
                    {
                        Article += drOrderProduct["ProductArticleNumber"].ToString() + ";";
                        DataTable dtProduct = Class.Requests.SelectDB("Product", "Product.ProductArticleNumber='" + drOrderProduct["ProductArticleNumber"].ToString() + "'", "ProductName,ProductCost,ProductDiscountAmount");
                        sostav += dtProduct.Rows[0]["ProductName"].ToString() + ":" + drOrderProduct["OrderProductCount"].ToString() + "; ";
                        cost += Convert.ToInt32(drOrderProduct["OrderProductCount"]) * Convert.ToDouble(dtProduct.Rows[0]["ProductCost"]);
                        costD += Convert.ToInt32(drOrderProduct["OrderProductCount"]) * (Convert.ToDouble(dtProduct.Rows[0]["ProductCost"]) - ((Convert.ToDouble(dtProduct.Rows[0]["ProductCost"]) / 100) * Convert.ToDouble(dtProduct.Rows[0]["ProductDiscountAmount"])));
                    }
                    DateTime OrderDate = Convert.ToDateTime(drOrder["OrderDate"]);
                    DateTime OrderDeliveryDate = Convert.ToDateTime(drOrder["OrderDeliveryDate"]);

                    DataTable dtStatus = Class.Requests.SelectDB("Status", "StatusID=" + drOrder["OrderStatus"].ToString());


                    dataGridView1.Rows.Add(drOrder["OrderID"].ToString(), Article, dtStatus.Rows[0]["StatusName"].ToString(),
                        OrderDate.ToString("yyyy-MM-dd"), OrderDeliveryDate.ToString("yyyy-MM-dd"),
                        user, sostav, cost, Math.Round(((cost - costD) / cost) * 100, 2), costD);
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToDouble(dataGridView1.Rows[i].Cells["Discount"].Value) >= min && Convert.ToDouble(dataGridView1.Rows[i].Cells["Discount"].Value) < max)
                    {
                        dataGridView1.Rows[i].Visible = true;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Visible = false;
                    }

                    string Article = dataGridView1.Rows[i].Cells["art"].Value.ToString();
                    string[] ListArticle = Article.Split(';');
                    string Sostav = dataGridView1.Rows[i].Cells["OrderProduct"].Value.ToString();
                    string[] ListSostav = Sostav.Split(';');
                    bool F = true;
                    for (int j = 0; j < ListArticle.Length - 1; j++)
                    {
                        string[] Art_Count = ListSostav[j].Split(':');
                        DataTable dtProduct = Class.Requests.SelectDB("Product", "Product.ProductArticleNumber='" + ListArticle[j] + "'", "ProductQuantityInStock");
                        if (Convert.ToInt32(dtProduct.Rows[0][0]) < Convert.ToInt32(Art_Count[1]))
                        {
                            F = false;
                            break;
                        }
                    }

                    if (F)
                    {
                        if (ListSostav.Length >= 4)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.YellowGreen;
                        }
                    }
                    else
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Purple;
                    }


                }
                if (Sort)
                {
                    dataGridView1.Sort(dataGridView1.Columns["Price"], ListSortDirection.Ascending);
                }
                else
                {
                    dataGridView1.Sort(dataGridView1.Columns["Price"], ListSortDirection.Descending);
                }
                dataGridView1.ClearSelection();

            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void EditOrder_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            //dateTimePicker1.MinDate = DateTime.Now;
            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;

            Sort = true;
            EditOrderView();
            DataTable dtStatus = Class.Requests.SelectDB("Status");
            comboBox3.Items.Clear();
            foreach (DataRow dr in dtStatus.Rows)
            {
                comboBox3.Items.Add(dr["StatusName"].ToString());
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0) { min = 0; max = 100; }
            if (comboBox2.SelectedIndex == 1) { min = 0; max = 10; }
            if (comboBox2.SelectedIndex == 2) { min = 10; max = 15; }
            if (comboBox2.SelectedIndex == 3) { min = 15; max = 100; }
            EditOrderView();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) { Sort = true; }
            else { Sort = false; }
            EditOrderView();
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
                if (comboBox3.SelectedIndex == -1) { MessageBox.Show("Статууус"); return; }
                DataTable dtStatus = Class.Requests.SelectDB("Status", "StatusName='" + comboBox3.Text + "'");

                bool EditOrderF = Class.Requests.UpdateBD("Order",
                    "OrderStatus=" + dtStatus.Rows[0][0].ToString() + " , OrderDeliveryDate=\"" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "\"",
                    "OrderID=" + IDorder + "");

                if (EditOrderF) { MessageBox.Show("Успешно"); }
                else { MessageBox.Show("Неудачно"); }
                comboBox3.SelectedIndex = -1;
                EditOrderView();
            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = dataGridView1.CurrentRow.Index;
            comboBox3.Text = dataGridView1.Rows[id].Cells["OrderStatus"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.Rows[id].Cells["OrderDeliveryDate"].Value);
            IDorder = dataGridView1.Rows[id].Cells["OrderID"].Value.ToString();
        }

        private void EditOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
