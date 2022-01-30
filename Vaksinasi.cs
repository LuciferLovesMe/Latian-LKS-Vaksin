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
    public partial class Vaksinasi : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        int id, periode, id_dokter;
        public Vaksinasi()
        {
            InitializeComponent();
            loadgrid();
            loaddokter();
            loadvaksin();

            if(Session.level == 2)
            {
                paneldokter.Visible = false;
            }
        }

        void loadgrid()
        {
            string com = "select vaksinasi.id, warga.nama , warga.nik, detail_vaksinasi.tanggal_vaksin, jenis_vaksin.nama_vaksin from vaksinasi join admin on vaksinasi.id_user = admin.id join warga on warga.id_user = admin.id join detail_vaksinasi on vaksinasi.id = detail_vaksinasi.id_vaksinasi join jenis_vaksin on detail_vaksinasi.id_jenis_vaksin = jenis_vaksin.id ";
            dataGridView1.DataSource = Command.getdata(com);
        }

        void loadvaksin()
        {
            string com = "select * from jenis_vaksin";
            combovaksin.DataSource = Command.getdata(com);
            combovaksin.DisplayMember = "nama_vaksin";
            combovaksin.ValueMember = "id";
        }

        void loaddokter()
        {
            string com = "select * from admin where status_aktif = 1 and level = 2";
            combodokter.DataSource = Command.getdata(com);
            combodokter.DisplayMember = "nama";
            combodokter.ValueMember = "id";
        }

        bool val()
        {
            if(textBox1.TextLength != 16)
            {
                MessageBox.Show("NIK harus 16 digit!", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (val_proc())
            {
                string com = "insert into vaksinasi values('" + lblnik.Text + "', " + id + ")";
                Command.exec(com);
                SqlCommand command = new SqlCommand("select top(1) * from vaksinasi order by id desc", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                int vac_id = Convert.ToInt32(reader["id"]);
                connection.Close();

                if(Session.level == 2)
                {
                    id_dokter = Session.user_id;
                }
                else
                {
                    id_dokter = Convert.ToInt32(combodokter.SelectedValue);
                }

                string comm = "insert into detail_vaksinasi values(" + vac_id + ", " + Convert.ToInt32(lblperiode.Text) + ", getdate(), " + Convert.ToInt32(combovaksin.SelectedValue) + ", " + id_dokter + ")";
                try
                {
                    Command.exec(comm);
                    MessageBox.Show("Berhasil Menambah Data", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadgrid();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        private void clear()
        {
            lblnama.Text = "";
            lblnik.Text = "";
            lblperiode.Text = "";
            lblttl.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            Selected.id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Selected == true)
            {
                DetailVaksin detail = new DetailVaksin();
                this.Hide();
                detail.ShowDialog();
            }
            else
            {
                MessageBox.Show("Pilih satu data", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            DataWarga data = new DataWarga();
            this.Hide();
            data.ShowDialog();
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

        bool val_proc()
        {
            if(lblperiode.Text != "1" && lblperiode.Text != "2")
            {
                MessageBox.Show("Warga Telah Menerima 2 Dosis Vaksin", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(id_dokter != 0)
            {
                MessageBox.Show("Harap Memilih Dokter", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (val())
            {
                string com = "select * from warga where nik = '" + textBox1.Text + "'";
                SqlCommand command = new SqlCommand(com, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    DateTime date = Convert.ToDateTime(reader["tanggal_lahir"]);
                    id = Convert.ToInt32(reader["id_user"]);
                    lblnik.Text = reader["nik"].ToString();
                    lblnama.Text = reader["nama"].ToString();
                    lblttl.Text = reader["tempat_lahir"].ToString() + ", " + date.ToString("dd-MM-yyyy");
                    textBox2.Text = reader["alamat"].ToString();
                    connection.Close();

                    SqlCommand sql = new SqlCommand("select count(id) as num from vaksinasi where id_user = " + id, connection);
                    connection.Open();
                    SqlDataReader read = sql.ExecuteReader();
                    read.Read();
                    periode = Convert.ToInt32(read["num"]);
                    if (periode == 0)
                        lblperiode.Text = "1";
                    else if (periode == 1)
                        lblperiode.Text = "2";
                    else
                        lblperiode.Text = "Warga Telah Melakukan Vaksin dosis 2";
                    connection.Close();
                }
                else
                {
                    connection.Close();
                    MessageBox.Show("NIK Warga Tidak Dapat Ditemukan", "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
