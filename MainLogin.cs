using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Vaksin
{
    public partial class MainLogin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        public MainLogin()
        {
            InitializeComponent();
        }

        bool val()
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1)
            {
                MessageBox.Show("Semua field harus diisi!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(textBox2.TextLength < 8)
            {
                MessageBox.Show("Password minimal 8 digit!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                string pass = Encrypt.enc(textBox2.Text);
                SqlCommand command = new SqlCommand("select * from admin where username = @username and password = '"+pass+"' and status_aktif = 1", connection);
                connection.Open();
                command.Parameters.AddWithValue("@username", textBox1.Text);
                command.Parameters.AddWithValue("@psw", pass);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    Session.username = reader["username"].ToString();
                    Session.nama = reader["nama"].ToString();
                    Session.level = Convert.ToInt32(reader["level"]);
                    Session.user_id = Convert.ToInt32(reader["id"]);
                    connection.Close();

                    if(Session.level == 1)
                    {
                        MainAdmin main = new MainAdmin();
                        this.Hide();
                        main.ShowDialog();
                    }
                    else if(Session.level == 2)
                    {
                        MainDokter main = new MainDokter();
                        this.Hide();
                        main.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Pengguna Tidak Dapat Ditemukan!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    connection.Close();
                    MessageBox.Show("Pengguna Tidak Dapat Ditemukan!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';

            else if (!checkBox1.Checked)
                textBox2.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup aplikasi?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }
    }
}
