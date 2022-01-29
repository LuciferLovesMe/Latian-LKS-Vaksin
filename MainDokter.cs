using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Vaksin
{
    public partial class MainDokter : Form
    {
        public MainDokter()
        {
            InitializeComponent();
            lblname.Text = Session.nama;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy / HH:mm:ss");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah anda yakin ingin menutup aplikasi?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Application.Exit();
        }

        private void panel_master_Click(object sender, EventArgs e)
        {
            DataWarga master = new DataWarga();
            this.Hide();
            master.ShowDialog();
        }

        private void panel_vaksin_Click(object sender, EventArgs e)
        {
            Vaksinasi vaksinasi = new Vaksinasi();
            this.Hide();
            vaksinasi.ShowDialog();
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
    }
}
