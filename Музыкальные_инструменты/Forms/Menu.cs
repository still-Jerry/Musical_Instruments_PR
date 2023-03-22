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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            label2.Text = "Сотрудник: \n" + Auth.User;
            if (Auth.Role == 2)
            {
                label2.Text = "Должность: Менеджер";
                button6.Visible = false;
                button7.Visible = false;
                button5.Visible = false;
                this.Size = new Size(269, 286);
            }
            else
            {
                label2.Text = "Должность: Администратор";
                button6.Visible = true;
                button7.Visible = true;
                button5.Visible = true;
                this.Size = new Size(269, 403);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            View View = new View();
            this.Visible = false;
            View.ShowDialog();
            //this.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Product Product = new Product();
            this.Visible = false;
            Product.ShowDialog();
            //this.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditOrder EditOrder = new EditOrder();
            this.Visible = false;
            EditOrder.ShowDialog();
            //this.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SpecialFunc SpecialFunc = new SpecialFunc();
            this.Visible = false;
            SpecialFunc.ShowDialog();
            //this.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Dir Dir = new Dir();
            this.Visible = false;
            Dir.ShowDialog();
            //this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Auth Auth = new Auth();
            this.Visible = false;
            Auth.ShowDialog();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

       
    }
}
