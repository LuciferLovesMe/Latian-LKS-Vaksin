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
    public partial class MasterWarga : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        int id, cond;
        public MasterWarga()
        {
            InitializeComponent();
            dis();
            loadgrid();

            lblname.Text = Session.nama;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy / HH:mm:ss");
        }

        void loadgrid()
        {
            string com = "select * from warga";
            dataGridView1.DataSource = Command.getdata(com);
        }

        void dis()
        {
            btn_tambah.Enabled = true;
            btnhapus.Enabled = true;
            btn_edit.Enabled = true;
            btnsave.Enabled = false;
            btncancel.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            dateTimePicker1.Enabled = false;
        }

        void enable()
        {
            btn_tambah.Enabled = false;
            btnhapus.Enabled = false;
            btn_edit.Enabled = false;
            btnsave.Enabled = true;
            btncancel.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            dateTimePicker1.Enabled = true;
        }

        bool val()
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1 || dateTimePicker1.Value == null)
            {
                MessageBox.Show("Semua field harus diisi", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBox1.TextLength != 16)
            {
                MessageBox.Show("NIK harus 16 digit!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            SqlCommand command = new SqlCommand("select * from warga where nik = '" + textBox1.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("NIK telah digunakan!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        private void btn_tambah_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Selected != false)
            {
                cond = 2;
                enable();
            }
        }

        private void btnhapus_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Selected != false)
            {
                DialogResult result = MessageBox.Show("Apakah anda yakin ingin menghapus?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string com = "delete from warga where id_user = " + id;
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Berhasil Menghapus", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        clear();
                        loadgrid();
                        dis();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        MessageBox.Show("" + ex);
                        throw;
                    }
                }
            }
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        string getwarga()
        {
            SqlCommand command = new SqlCommand("select count(id) as num from admin where level = 3", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int num = Convert.ToInt32(reader["num"]);
            connection.Close();
            if (num > 0)
            {
                return "warga" + num.ToString();
            }
            else
            {
                return "warga1";
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if(cond == 1 && val())
            {
                string com = "insert into admin values('" + getwarga() + "', '123123123', 1, 3, '')";
                Command.exec(com);

                SqlCommand command = new SqlCommand("select top(1) * from admin where level = 3 order by id desc", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int userid = Convert.ToInt32(reader["id"]);
                connection.Close();

                string comm = "insert into warga values('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', '" + textBox4.Text + "', '" + textBox5.Text + "', " + userid + ")";
                try
                {
                    Command.exec(comm);
                    MessageBox.Show("Berhasil Menambah", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid();
                    dis();
                    clear();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    MessageBox.Show(ex.Message);
                }
            }
            else if (cond == 2 && val_up())
            {
                string com = "update warga set nik = '" + textBox1.Text + "', nama = '" + textBox2.Text + "', alamat = '" + textBox4.Text + "', noHp = '" + textBox5.Text + "', tempat_lahir = '" + textBox3.Text + "', tanggal_lahir = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' where id_user = " + id;
                try
                {
                    Command.exec(com);
                    MessageBox.Show("Berhasil Mengubah", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid();
                    dis();
                    clear();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[6].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[3].Value);
        }

        private void textboxsearch_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from warga where nama like '%" + textboxsearch.Text + "%' or nik like '%" + textboxsearch.Text + "%'";
            dataGridView1.DataSource = Command.getdata(com);

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup aplikasi?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin login = new MainLogin();
                this.Hide();
                login.ShowDialog();
            }
        }

        bool val_up()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1 || dateTimePicker1.Value == null)
            {
                MessageBox.Show("Semua field harus diisi", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox1.TextLength != 16)
            {
                MessageBox.Show("NIK harus 16 digit!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            SqlCommand command = new SqlCommand("select * from warga where nik = '" + textBox1.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                if(Convert.ToInt32(reader["id_user"]) != id)
                {
                    connection.Close();
                    MessageBox.Show("NIK telah digunakan!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            connection.Close();
            return true;
        }
    }
}
