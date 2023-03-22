using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace Музыкальные_инструменты.Class
{
    class Requests
    {
        //public static string StrCon = "host=10.207.106.12;uid=user38;pwd=gj65;database=db38";
        public static string StrCon = "host=localhost;uid=root;pwd=root;database=db";

        public static DataTable SelectDB(string table, string where = "", string atr = "*")
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection con = new MySqlConnection(StrCon);
                con.Open();
                string q = "Select " + atr + " from `" + table + "`";
                if (where != "")
                {
                    q += " where " + where;
                }
                MySqlCommand cmd = new MySqlCommand(q, con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                adp.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception) { return dt; }

        }

        public static DataTable SelectInnerJoin(string table, string table1, string on, string Where = "")
        {
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection con = new MySqlConnection(StrCon);
                con.Open();
                string q = "Select * from `" + table + "` inner join `" + table1 + "` on " + on;
                if (Where != "")
                {
                    q += " where " + Where;
                }
                MySqlCommand cmd = new MySqlCommand(q, con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                adp.Fill(dt);
                con.Close();
                return dt;
            }
            catch (Exception) { return dt; }

        }

        public static bool UpdateBD(string TableName, string value, string where)
        {

            try
            {
                MySqlConnection con = new MySqlConnection(StrCon);
                con.Open();
                string Request = "Update `" + TableName + "` set " + value + " where " + where + ";";
                MySqlCommand cmd = new MySqlCommand(Request, con);
                int n = cmd.ExecuteNonQuery();
                con.Close();
                return n > 0;
            }
            catch (Exception) { return false; }
        }

        public static bool DeleteBD(string TableName, string where)
        {

            try
            {
                MySqlConnection con = new MySqlConnection(StrCon);
                con.Open();
                string Request = "Delete  from " + TableName + " where " + where + ";";
                MySqlCommand cmd = new MySqlCommand(Request, con);
                int n = cmd.ExecuteNonQuery();
                con.Close();
                return n > 0;
            }
            catch (Exception) { return false; }
        }

        public static bool InsertDB(string TableName, string Attributes, string VALUES)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(StrCon);
                con.Open();
                string Request = "Insert Into  `" + TableName + "` (" + Attributes + ") VALUES (" + VALUES + ")";
                MySqlCommand cmd = new MySqlCommand(Request, con);
                int n = cmd.ExecuteNonQuery();
                con.Close();
                return n > 0;
            }
            catch (Exception) { return false; }
        }

        public static string InsertTransactionOrder(string S, string N, string P, string PP, string dateD, string dateO, Dictionary<string, int> ProductO)
        {

            MySqlConnection con = new MySqlConnection(StrCon);
            con.Open();
            MySqlTransaction trans = con.BeginTransaction();

            string Qsql1 = "Select * from user where UserSurname='" + S + "' and UserName='" + N + "' and UserPatronymic='" + P + "';";
            MySqlCommand sql1 = new MySqlCommand(Qsql1, con);
            sql1.Transaction = trans;

            string Qsql2 = "Insert Into  `user` (UserSurname, UserName, UserPatronymic, UserLogin, UserPassword, UserRole) VALUES ('" + S + "', '" + N + "', '" + P + "', 'login" + Class.RandomStr.RandomStrUser() + "','" + Class.RandomStr.RandomStrUser() + "',1);";

            string Qsql3 = "SELECT LAST_INSERT_ID();";

            string idUser = "";


            try
            {
                MySqlDataAdapter adpsql1 = new MySqlDataAdapter(sql1);
                DataTable dtUser = new DataTable();
                adpsql1.Fill(dtUser);

                if (dtUser.Rows.Count == 0)
                {
                    MySqlCommand sql23 = new MySqlCommand(Qsql2 + Qsql3, con);
                    sql23.Transaction = trans;
                    idUser = sql23.ExecuteScalar().ToString();
                }
                else
                {
                    idUser = dtUser.Rows[0][0].ToString();
                }

                string Qsql4 = "Insert Into  `order` (OrderStatus, OrderDeliveryDate, OrderPickupPoint, OrderCode, OrderUser, OrderDate) VALUES (2,\"" + dateD + "\"," + PP + "," + Class.RandomStr.RandomCode() + "," + idUser + ",\"" + dateO + "\");";
                MySqlCommand sql43 = new MySqlCommand(Qsql4 + Qsql3, con);
                sql43.Transaction = trans;
                string IDOrder = sql43.ExecuteScalar().ToString();


                foreach (var p in ProductO)
                {
                    string Qsql5 = "Insert Into  `orderproduct` (OrderID, ProductArticleNumber, OrderProductCount) VALUES (" + IDOrder + ",'" + p.Key + "'," + p.Value + ");";
                    MySqlCommand sql5 = new MySqlCommand(Qsql5, con);
                    sql5.Transaction = trans;
                    sql5.ExecuteNonQuery();
                }


                trans.Commit();

                return IDOrder;
            }
            catch (Exception) { trans.Rollback(); con.Close(); return ""; }
        }

        public static bool ExportTable(string table)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(StrCon);
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `" + table + "`", con);
                MySqlDataReader rdr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(rdr);

                string FileName = table + "_" + DateTime.Now.ToString("yyyy_MM_dd__hh_mm_ss");
                StreamWriter writer = new StreamWriter(FileName + ".csv", false);

                //colums
                for (int i = 0, len = dt.Columns.Count - 1; i <= len; ++i)
                {
                    if (i != len)
                        writer.Write(dt.Columns[i].ColumnName + ";");
                    else
                        writer.Write(dt.Columns[i].ColumnName);
                }

                writer.Write("\n");

                int count = dt.Rows.Count;

                //data
                foreach (DataRow dataRow in dt.Rows)
                {

                    string r = String.Join(";", dataRow.ItemArray);
                    string rez = r.Replace(',', '.');
                    writer.WriteLine(rez);
                }

                writer.Close();

                con.Close();

                return true;
            }
            catch (Exception) { return false; }
        }

        public static bool ImportTable(string TableName, string Attributes, string VALUES)
        {
            try
            {
                MySqlConnection con = new MySqlConnection(StrCon);
                con.Open();
                string Request = "Insert Into  `" + TableName + "` (" + Attributes + ") VALUES " + VALUES + ";";
                MySqlCommand cmd = new MySqlCommand(Request, con);
                int n = cmd.ExecuteNonQuery();
                con.Close();
                return n > 0;
            }
            catch (Exception) { return false; }
        }
    }
}
