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
    public partial class MasterVaksin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        int id, cond;
        public MasterVaksin()
        {
            InitializeComponent();
            dis();
            loadgrid();

            lblname.Text = Session.nama;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy / HH:mm:ss");
        }

        void loadgrid()
        {
            string com = "select * from jenis_Vaksin";
            dataGridView1.DataSource = Command.getdata(com);
        }

        void dis()
        {
            textBox1.Enabled = false;
            btnsave.Enabled = false;
            btncancel.Enabled = false;
            btnhapus.Enabled = true;
            btn_tambah.Enabled = true;
            btn_edit.Enabled = true;
        }

        void enable()
        {
            textBox1.Enabled = true;
            btnsave.Enabled = true;
            btncancel.Enabled = true;
            btnhapus.Enabled = false;
            btn_tambah.Enabled = false;
            btn_edit.Enabled = false;
        }

        private void panelvaksin_Click(object sender, EventArgs e)
        {

        }

        bool val()
        {
            if(textBox1.TextLength < 1)
            {
                MessageBox.Show("Nama vaksin harus diisi!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from jenis_vaksin where nama_vaksin = '" + textBox1.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Nama Jenis Vaksin Telah Digunakan!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();
            return true;
        }

        bool val_up()
        {
            if (textBox1.TextLength < 1)
            {
                MessageBox.Show("Nama vaksin harus diisi!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from jenis_vaksin where nama_vaksin = '" + textBox1.Text + "'", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                if(Convert.ToInt32(reader["id"]) != id)
                {
                    connection.Close();
                    MessageBox.Show("Nama Jenis Vaksin Telah Digunakan!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            connection.Close();
            return true;
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
                    string com = "delete from jenis_vaksin where id = " + id;
                    try
                    {
                        Command.exec(com);
                        MessageBox.Show("Berhasil Menghapus", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        loadgrid();
                        dis();
                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        MessageBox.Show(ex.Message);
                    }
                }
                
            }
        }

        private void clear()
        {
            textBox1.Text = "";
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if(cond == 1 && val())
            {
                string com = "insert into jenis_vaksin values('" + textBox1.Text + "')";
                try
                {
                    Command.exec(com);
                    MessageBox.Show("Berhasil Menambah", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadgrid();
                    dis();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
            if(cond == 2 && val_up())
            {
                string com = "update jenis_vaksin set nama_Vaksin = '" + textBox1.Text + "' where id = " + id;
                try
                {
                    Command.exec(com);
                    MessageBox.Show("Berhasil Mengubah", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadgrid();
                    dis();
                }
                catch (Exception ex)
                {
                    connection.Close();
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void textboxsearch_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from jenis_vaksin where nama_Vaksin like '%" + textboxsearch.Text + "%'";
            dataGridView1.DataSource = Command.getdata(com);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup aplikasi?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void panel_master_Click(object sender, EventArgs e)
        {
            MasterAdmin master = new MasterAdmin();
            this.Hide();
            master.ShowDialog();
        }

        private void panel_vaksin_Click(object sender, EventArgs e)
        {
            MasterDokter dokter = new MasterDokter();
            this.Hide();
            dokter.ShowDialog();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            MasterWarga master = new MasterWarga();
            this.Hide();
            master.ShowDialog();
        }

        private void btn_tambah_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }
    }
}
