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
using Excel = Microsoft.Office.Interop.Excel;

namespace Музыкальные_инструменты.Forms
{
    public partial class SpecialFunc : Form
    {


        public SpecialFunc()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedIndex == -1) { MessageBox.Show("Выберите таблицу"); return; }
                bool ex = Class.Requests.ExportTable(comboBox2.Text);
                if (ex) { MessageBox.Show("Успешно"); }
                else { MessageBox.Show("Неудалось"); }
            }
            catch (Exception) { MessageBox.Show("Ошибка экспорта"); }
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
                if (comboBox2.SelectedIndex == -1) { MessageBox.Show("Выберите таблицу"); return; }
                openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string pathToFile = openFileDialog1.FileName;

                    string[] readText = File.ReadAllLines(pathToFile);
                    string[] valField;
                    string[] titleField = readText[0].Split(';');

                    string VALUES = "";

                    foreach (string str in readText.Skip(1).ToArray())
                    {
                        valField = str.Split(';');
                        if (comboBox2.SelectedIndex == 0)
                        {
                            //OrderID, OrderStatus, OrderDeliveryDate, OrderPickupPoint, OrderCode, OrderUser, OrderDate
                            DateTime d1 = Convert.ToDateTime(valField[2]);
                            DateTime d2 = Convert.ToDateTime(valField[2]);
                            string s1 = d1.ToString("yyyy-MM-dd");
                            string s2 = d2.ToString("yyyy-MM-dd");
                            VALUES += "(" + valField[0] + "," + valField[1] + ",\"" + s1 + "\"," + valField[3] + "," + valField[4] + "," + valField[5] + ",\"" + s2 + "\"),";


                        }
                        if (comboBox2.SelectedIndex == 1)
                        {
                            //OrderProductID, OrderID, ProductArticleNumber, OrderProductCount
                            VALUES += "(" + valField[0] + "," + valField[1] + "," + "'" + valField[2] + "'," + valField[3] + "),";
                        }
                        if (comboBox2.SelectedIndex == 2)
                        {
                            //ProductArticleNumber, ProductName, ProductDescription, ProductCategory, ProductPhoto, ProductManufacturer,
                            //ProductCost, ProductDiscountAmount, ProductQuantityInStock, ProductDiscount, ProductSupplier, ProductUnit
                            VALUES += "('" + valField[0] + "','" + valField[1] + "','" + valField[2] + "'," + valField[3] + "," +
                           (valField[4] == String.Empty ? "NULL" : ("'" + valField[4] + "'")) + ",'" + valField[5] + "'," +
                           valField[6].Replace(',', '.') + "," + (valField[7] == String.Empty ? "NULL" : valField[7]) + "," +
                           valField[8] + "," + (valField[9] == String.Empty ? "NULL" : valField[9]) + "," + valField[10] + "," + valField[11] + "),";
                        }
                        if (comboBox2.SelectedIndex == 3)
                        {
                            //UserID, UserSurname, UserName, UserPatronymic, UserLogin, UserPassword, UserRole
                            VALUES += "(" + valField[0] + ",'" + valField[1] + "','" + valField[2] + "','" +
                           valField[3] + "','" + valField[4] + "','" + valField[5] + "','" + valField[6] + "'),";
                        }
                    }
                    VALUES = VALUES.Substring(0, VALUES.Length - 1);

                    bool FInsertDB = Class.Requests.ImportTable(comboBox2.Text, String.Join(",", titleField), VALUES);
                    if (FInsertDB) { MessageBox.Show("Успешно"); }
                    else { MessageBox.Show("Неудалось"); }


                }
            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void SpecialFunc_Load(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            DateTime d1 = d.AddDays(7);
            DateTime d2 = d - (d1 - d);
            dateTimePicker2.Value = d1;
            dateTimePicker1.Value = d2;
            comboBox2.SelectedIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {


                Excel.Application excelApp = new Excel.Application();
                // Добавление листа
                var workBook = excelApp.Workbooks.Add(Type.Missing);

                // Получение листа
                var mechanic = workBook.ActiveSheet;
                // устанавливаем имя рабочего листа
                mechanic.Name = "Отчет";

                // Объект таблицы
                var mechanicCells = mechanic.Cells;

                mechanic.Cells[1, 1] = "Номер заказа";
                mechanic.Cells[1, 2] = "Дата заказа";
                mechanic.Cells[1, 3] = "Дата доставки";
                mechanic.Cells[1, 4] = "Сумма заказа с учетом скидки";
                mechanic.Cells[1, 5] = "Статус заказа";
                mechanic.Cells[1, 6] = "Пользователь";

                // Получение данных
                int temp = 2;
                double VV = 0;
                DataTable dtorder = Class.Requests.SelectDB("Order", "OrderDate BETWEEN CAST(\"" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "\" AS DATE) AND CAST(\"" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "\" AS DATE)");
                foreach (DataRow drorder in dtorder.Rows)
                {
                    DataTable dtOrderProduct = Class.Requests.SelectDB("OrderProduct", "OrderID=" + drorder["OrderID"].ToString());
                    double cost = 0;
                    foreach (DataRow drOrderProduct in dtOrderProduct.Rows)
                    {
                        DataTable dtProduct = Class.Requests.SelectDB("Product", "ProductArticleNumber='" + drOrderProduct["ProductArticleNumber"].ToString() + "'");
                        cost += Convert.ToDouble(dtProduct.Rows[0]["ProductCost"]) - (Convert.ToDouble(dtProduct.Rows[0]["ProductCost"]) / 100 * Convert.ToInt32(dtProduct.Rows[0]["ProductDiscountAmount"]));
                    }
                    DataTable dtUser = Class.Requests.SelectDB("User", "UserID=" + drorder["OrderUser"].ToString());
                    DataTable dtStatus = Class.Requests.SelectDB("Status", "StatusID=" + drorder["OrderStatus"].ToString());

                    VV += cost;
                    mechanic.Cells[temp, 1] = drorder["OrderID"].ToString();
                    mechanic.Cells[temp, 2] = drorder["OrderDate"].ToString();
                    mechanic.Cells[temp, 3] = drorder["OrderDeliveryDate"].ToString();
                    mechanic.Cells[temp, 4] = Math.Round(cost).ToString();
                    mechanic.Cells[temp, 5] = dtStatus.Rows[0]["StatusName"].ToString();
                    mechanic.Cells[temp, 6] = dtUser.Rows[0]["UserSurname"].ToString() + " " + dtUser.Rows[0]["UserName"].ToString() + " " + dtUser.Rows[0]["UserPatronymic"].ToString();
                    temp++;
                }
                mechanic.Cells[temp, 1] = "ИТОГО:";
                mechanic.Cells[temp, 2] = Math.Round(VV);
                mechanic.Columns.AutoFit();
                mechanic.Rows.AutoFit();
                excelApp.Visible = true;


            }
            catch (Exception) { MessageBox.Show("Ошибка"); }
        }

        private void SpecialFunc_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
