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
using Word = Microsoft.Office.Interop.Word;

namespace Музыкальные_инструменты.Forms
{
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
        }


        private readonly string FileName = Directory.GetCurrentDirectory() + @"\template\template.docx";
        public static string orderID = "";


        private void ViewDataTable()
        {
            try
            {
                dataGridView1.Rows.Clear();
                if (View.ProductO.Count == 0) { button2.Enabled = false; return; }
                foreach (var p in View.ProductO)
                {
                    DataTable dt = Class.Requests.SelectDB("product", "ProductArticleNumber='" + p.Key + "'");
                    if (dt.Rows.Count == 0) { continue; }
                    string path = Directory.GetCurrentDirectory();
                    if (dt.Rows[0]["ProductPhoto"].ToString() == "")
                    {
                        path += "\\Image\\picture.png";
                    }
                    else
                    {
                        path += "\\Image\\" + dt.Rows[0]["ProductPhoto"].ToString();
                    }
                    Image img = new Bitmap(path);
                    double cost = Convert.ToDouble(dt.Rows[0]["ProductCost"]) - (Convert.ToDouble(dt.Rows[0]["ProductCost"]) / 100 * Convert.ToInt32(dt.Rows[0]["ProductDiscountAmount"]));

                    dataGridView1.Rows.Add(p.Key, img, dt.Rows[0]["ProductName"].ToString(), dt.Rows[0]["ProductCost"].ToString(), dt.Rows[0]["ProductDiscountAmount"].ToString(), (cost * p.Value), p.Value);

                }
                DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn();
                dataGridView1.Columns.Add(buttonColumn1);
                buttonColumn1.HeaderText = "Добавить";
                buttonColumn1.UseColumnTextForButtonValue = true;
                buttonColumn1.Text = "+";


                DataGridViewButtonColumn buttonColumn2 = new DataGridViewButtonColumn();
                dataGridView1.Columns.Add(buttonColumn2);
                buttonColumn2.HeaderText = "Удалить";
                buttonColumn2.UseColumnTextForButtonValue = true;
                
                //buttonColumn2.DefaultCellStyle. = FlatStyle;
                buttonColumn2.Text = "-";

                dataGridView1.Columns[7].DisplayIndex = 6;
                DateTime d = DateTime.Now;
                DateTime d1 = new DateTime();
                if (View.ProductO.Count >= 3)
                {
                    bool f = true;
                    foreach (var p in View.ProductO)
                    {
                        DataTable dt = Class.Requests.SelectDB("product", "ProductArticleNumber='" + p.Key + "'");
                        if (dt.Rows.Count == 0) { continue; }
                        if (!(Convert.ToInt32(dt.Rows[0]["ProductQuantityInStock"]) >= p.Value))
                        {
                            f = false; break;
                        }
                    }
                    if (f)
                    {
                        d1 = d.AddDays(3);
                    }
                    else
                    {
                        d1 = d.AddDays(6);
                    }
                }
                else
                {
                    d1 = d.AddDays(6);
                }

                label8.Text = d1.ToString("yyyy-MM-dd");
                dataGridView1.ClearSelection();
            }
            catch (Exception) { MessageBox.Show("Ошибка вывода списка корзины"); }


        }

        private void Order_Load(object sender, EventArgs e)
        {
            try
            {
                if (Auth.User == "")
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                }
                else
                {
                    string[] SUser = Auth.User.Split(' ');
                    textBox1.Text = SUser[0];
                    textBox2.Text = SUser[1];
                    textBox3.Text = SUser[2];
                }
                DataTable pickuppoint = Class.Requests.SelectDB("pickuppoint");
                comboBox1.Items.Clear();
                DateTime d = DateTime.Now;
                label6.Text = d.ToString("yyyy-MM-dd");
                orderID = "";
                button3.Enabled = false;
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                textBox3.ReadOnly = false;
                comboBox1.Enabled = true;
                foreach (DataRow dr in pickuppoint.Rows)
                {
                    comboBox1.Items.Add(dr["PickupPointName"].ToString());
                }
                ViewDataTable();
            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int r = e.RowIndex, c = e.ColumnIndex;
                string id = dataGridView1.Rows[r].Cells[0].Value.ToString();
                if (c == 7)
                {
                    View.ProductO[id] = View.ProductO[id] + 1;
                }
                if (c == 8)
                {
                    View.ProductO[id] = View.ProductO[id] - 1;
                    if (View.ProductO[id] == 0) {
                        var quest = MessageBox.Show("Уверены, что хотите удалить товар из корзины?", "", MessageBoxButtons.YesNo);
                        if (quest == DialogResult.Yes)
                        {
                            View.ProductO.Remove(id);
                        }
                        else {
                            View.ProductO[id] = 1;
                        }
                        
                    }
                }
                dataGridView1.Columns.RemoveAt(7);
                dataGridView1.Columns.RemoveAt(7);
                ViewDataTable();
            }
            catch (Exception) {
                //MessageBox.Show("Ошибка"); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            View View = new View();
            this.Visible = false;
            View.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Все поля обязательны для ввода"); return;
                }
                DataTable pickuppoint = Class.Requests.SelectDB("pickuppoint", "PickupPointName='" + comboBox1.Text + "'");

                string InsertTransactionOrder = Class.Requests.InsertTransactionOrder(textBox1.Text, textBox2.Text, textBox3.Text, pickuppoint.Rows[0][0].ToString(), label8.Text, label6.Text, View.ProductO);

                if (InsertTransactionOrder != "")
                {
                    orderID = InsertTransactionOrder;
                    button3.Enabled = true;
                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    textBox3.ReadOnly = true;
                    comboBox1.Enabled = false;
                    MessageBox.Show("Успешно ");
                    View.ProductO.Clear();
                    dataGridView1.Columns.RemoveAt(7);
                    dataGridView1.Columns.RemoveAt(7);
                    ViewDataTable();
                }
                else
                {
                    MessageBox.Show("Неудалось оформить заказ");
                    orderID = "";
                    button3.Enabled = false;
                    textBox1.ReadOnly = false;
                    textBox2.ReadOnly = false;
                    textBox3.ReadOnly = false;
                    comboBox1.Enabled = true;
                }


            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (orderID == "") { return; }

                //DataTable user = Class.Requests.SelectDB("user", "UserSurname='" + textBox1.Text + "' and UserName='" + textBox2.Text + "' and UserPatronymic='" + textBox3.Text + "'");
                DataTable dtorder = Class.Requests.SelectDB("order", "OrderID=" + orderID);
                DataTable dtorderproduct = Class.Requests.SelectDB("orderproduct", "OrderID=" + orderID);

                string text1 = "";
                string text2 = "";
                double price = 0;
                double cost = 0;

                foreach (DataRow dr in dtorderproduct.Rows)
                {
                    DataTable dt = Class.Requests.SelectDB("product", "ProductArticleNumber='" + dr["ProductArticleNumber"].ToString() + "'");
                    price += Convert.ToInt32(dr["OrderProductCount"]) * Convert.ToDouble(dt.Rows[0]["ProductCost"]);
                    cost += Convert.ToInt32(dr["OrderProductCount"]) * (Convert.ToDouble(dt.Rows[0]["ProductCost"]) - (Convert.ToDouble(dt.Rows[0]["ProductCost"]) / 100 * Convert.ToInt32(dt.Rows[0]["ProductDiscountAmount"])));
                    text1 += dt.Rows[0]["ProductName"].ToString() + "\v";
                    text2 += dr["OrderProductCount"].ToString() + "*" + dt.Rows[0]["ProductCost"].ToString() + "\v";
                }



                var wordApp = new Word.Application();
                wordApp.Visible = false;
                try
                {
                    var wordDocument = wordApp.Documents.Open(FileName);
                    DateTime OrderDate = Convert.ToDateTime(dtorder.Rows[0]["OrderDate"]);
                    DateTime OrderDeliveryDate = Convert.ToDateTime(dtorder.Rows[0]["OrderDeliveryDate"]);
                    ReplaceWordStub("{OrderID}", orderID, wordDocument);
                    ReplaceWordStub("{User}", textBox1.Text + " " + textBox2.Text + " " + textBox3.Text, wordDocument);
                    ReplaceWordStub("{DateO}", OrderDate.ToString("yyyy-MM-dd"), wordDocument);
                    ReplaceWordStub("{DateD}", OrderDeliveryDate.ToString("yyyy-MM-dd"), wordDocument);
                    ReplaceWordStub("{Pickuppoint}", comboBox1.Text, wordDocument);
                    ReplaceWordStub("{OrderCode}", dtorder.Rows[0]["OrderCode"].ToString(), wordDocument);
                    ReplaceWordStub("{p1}", text1, wordDocument);
                    ReplaceWordStub("{p2}", text2, wordDocument);
                    ReplaceWordStub("{Price}", price.ToString(), wordDocument);
                    ReplaceWordStub("{Discount}", Math.Round(((price - cost) / price) * 100, 2).ToString(), wordDocument);
                    ReplaceWordStub("{Cost}", cost.ToString(), wordDocument);
                    wordApp.Visible = true;
                }
                catch
                {
                    MessageBox.Show("Ошибка создания отчёта");
                }
            }
            catch (Exception) { MessageBox.Show("Ошибка формирования чека"); }
        }

        private void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        private void Order_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
