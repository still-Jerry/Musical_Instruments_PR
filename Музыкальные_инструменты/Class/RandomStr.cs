using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;

namespace Музыкальные_инструменты.Class
{
    class RandomStr
    {
        public static string RandomCaptcha(int n = 5)
        {
            try
            {
                string abc = "qwertyuiopasdfghjklzxcvbnm1234567890";
                string rez = "";
                Random rdm = new Random();
                for (int i = 0; i < n; i++)
                {
                    rez += abc[rdm.Next(abc.Length)];
                }
                return rez;
            }
            catch (Exception)
            {
                return "zxcvb";
            }
        }
        public static string RandomCode(int n = 3)
        {
            try
            {
                string abc = "1234567890";
                string rez = "";
                Random rdm = new Random();
                for (int i = 0; i < n; i++)
                {
                    rez += abc[rdm.Next(abc.Length)];
                }
                if (rez.Length == 2) { rez += "6"; }
                return rez;
            }
            catch (Exception)
            {
                return "123";
            }
        }

        public static string RandomStrUser(int n = 4)
        {
            try
            {
                string abc = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
                string rez = "";
                Random rdm = new Random();
                for (int i = 0; i < n; i++)
                {
                    rez += abc[rdm.Next(abc.Length)];
                }
                return rez;
            }
            catch (Exception)
            {
                return "zxcb";
            }
        }
    }
}
