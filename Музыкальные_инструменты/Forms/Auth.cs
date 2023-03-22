using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Музыкальные_инструменты.Forms
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }
        public static int UserID = -1;
        public static string User = "";
        public static int Role = 0;

        public static bool Captcha = false;
        public static string CaptchaStr = "";

        /// <summary>
        /// Кнопка перехода на форму просмотра
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                View View = new View();
                this.Visible = false;
                View.ShowDialog();
                //textBox1.Text = "";
                //textBox2.Text = "";
                //textBox3.Text = "";
                Captcha = false;
                groupBox1.Visible = Captcha;
                //this.Visible = true;
            }
            catch (Exception) { MessageBox.Show("Ошибка перехода на форму просмотра."); }
        }

        private void Auth_Load(object sender, EventArgs e)
        {
            this.Size = new Size(234, 300);
            Captcha = false;
            groupBox1.Visible = Captcha;
            
        }

        /// <summary>
        /// Кнопка авторизации
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Поля Логин и Пароль обязательны");
                    return;
                }
                if (Captcha)
                {

                    if (textBox3.Text == "")
                    {
                        MessageBox.Show("Каптча обязательна");
                        return;
                    }
                }
                
                DataTable dt = Class.Requests.SelectDB("User", "UserLogin='" + textBox1.Text + "' and UserPassword='" + textBox2.Text + "'");
                if (dt.Rows.Count != 0)
                {
                    if (Captcha)
                    {
                        if (textBox3.Text != CaptchaStr)
                        {
                            MessageBox.Show("Приложение заблокировано на 10с");
                            button1.Enabled = false;
                            Thread.Sleep(10000);
                            CaptchaView();
                            textBox3.Text = "";
                            button1.Enabled = true;
                            MessageBox.Show("Приложение разблокировано");
                            return;
                        }
                    }
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    Captcha = false;
                    groupBox1.Visible = Captcha;
                    User = dt.Rows[0]["UserSurname"].ToString() + " " + dt.Rows[0]["UserName"].ToString() + " " + dt.Rows[0]["UserPatronymic"].ToString();
                    Role = Convert.ToInt32(dt.Rows[0]["UserRole"]);
                    UserID = Convert.ToInt32(dt.Rows[0]["UserID"]);
                    if (dt.Rows[0]["UserRole"].ToString() == "1")
                    {
                        View View = new View();
                        this.Visible = false;
                        View.ShowDialog();
                        //User = "";
                        //Role = 0;
                        //UserID = -1;
                        //this.Visible = true;
                    }
                    else
                    {
                        Menu Menu = new Menu();
                        this.Visible = false;
                        Menu.ShowDialog();
                        //User = "";
                        //Role = 0;
                        //UserID = -1;
                        //this.Visible = true;
                    }


                }
                else
                {
                    if (Captcha)
                    {
                        MessageBox.Show("Приложение заблокировано на 10с");
                        button1.Enabled = false;
                        Thread.Sleep(10000);
                        CaptchaView();
                        textBox3.Text = "";
                        button1.Enabled = true;
                        MessageBox.Show("Приложение разблокировано");
                    }
                    else
                    {
                        this.Size = new Size(494, 300);
                        Captcha = true;
                        groupBox1.Visible = Captcha;
                        CaptchaView();
                    }
                }


            }
            catch (Exception) { MessageBox.Show("Ошибка Авторизации"); }
        }

        /// <summary>
        /// Функция генерации и вывода captcha
        /// </summary>
        private void CaptchaView()
        {
            try
            {
                Random rdm = new Random();
                if (rdm.Next(1, 3) == 1)
                {
                    groupBox1.BackgroundImage = new Bitmap(Музыкальные_инструменты.Properties.Resources.capt1);
                   
                }
                else
                {
                    groupBox1.BackgroundImage = new Bitmap(Музыкальные_инструменты.Properties.Resources.capt2);
                  
                }



                CaptchaStr = Class.RandomStr.RandomCaptcha();
                label4.Text = CaptchaStr[0].ToString();
                label5.Text = CaptchaStr[1].ToString();
                label6.Text = CaptchaStr[2].ToString();
                label7.Text = CaptchaStr[3].ToString();
                label8.Text = CaptchaStr[4].ToString();
            }
            catch (Exception) { MessageBox.Show("Ошибка отображения каптчи"); }
        }

        /// <summary>
        /// Кнопка обновления captcha
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            CaptchaView();
        }

        private void Auth_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
