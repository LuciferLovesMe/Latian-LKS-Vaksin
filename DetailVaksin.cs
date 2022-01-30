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
    public partial class DetailVaksin : Form
    {

        SqlConnection connection = new SqlConnection(Utils.conn);
        int id;
        public DetailVaksin()
        {
            InitializeComponent();
            loadwarga();
        }

        void loadwarga()
        {
            SqlCommand command = new SqlCommand("select warga.* from vaksinasi join admin on vaksinasi.id_user = admin.id join warga on admin.id = warga.id_user where vaksinasi.id = "+Selected.id, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            id = Convert.ToInt32(reader["id_user"]);
            DateTime date = Convert.ToDateTime(reader["tanggal_lahir"]);
            lblnama.Text = reader["nama"].ToString();
            lblnik.Text = reader["nik"].ToString();
            textBox2.Text = reader["alamat"].ToString();
            lblttl.Text = reader["tempat_lahir"].ToString() + ", " + date.ToString("dd-MM-yyyy");

            loadgrid();
            connection.Close();
        }

        void loadgrid()
        {
            string com = "select jenis_vaksin.nama_vaksin, detail_vaksinasi.tanggal_vaksin, detail_vaksinasi.periode, admin.nama as nama_dokter from vaksinasi join detail_vaksinasi on vaksinasi.id = detail_vaksinasi.id_vaksinasi join admin on detail_vaksinasi.id_dokter = admin.id join jenis_vaksin on jenis_vaksin.id = detail_vaksinasi.id_jenis_vaksin where vaksinasi.id_user = " + id;
            dataGridView1.DataSource = Command.getdata(com);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Vaksinasi vaksinasi = new Vaksinasi();
            this.Hide();
            vaksinasi.ShowDialog();

        }
    }
}
