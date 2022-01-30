using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LKS_Vaksin
{
    class Utils
    {
        public static string conn = @"Data Source=desktop-00eposj;Initial Catalog=LKS_Vaksin;Integrated Security=True";
    }

    class Encrypt
    {
        public static string enc(string data)
        {
            using(SHA256Managed managed = new SHA256Managed())
            {
                return Convert.ToBase64String(managed.ComputeHash(Encoding.UTF8.GetBytes(data)));
            }
        }
    }

    class Command
    {
        public static DataTable getdata(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(com, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public static void exec(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(com, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
    }

    class Session
    {
        public static int user_id { set; get; }
        public static string username { set; get; }
        public static string nama { set; get; }
        public static int level { set; get; }
    }

    class Selected
    {
        public static int id { set; get; }
    }
}
