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
    public partial class DataWarga : Form
    {

        public DataWarga()
        {
            InitializeComponent();
            loadgrid();
        }

        void loadgrid()
        {
            string com = "select warga.nik, warga.nama, detail_vaksinasi.tanggal_vaksin, jenis_vaksin.nama_vaksin from vaksinasi join admin on vaksinasi.id_user = Admin.id join warga on Admin.id = warga.id_user join detail_vaksinasi on vaksinasi.id = detail_vaksinasi.id_vaksinasi join jenis_vaksin on detail_vaksinasi.id_jenis_vaksin = jenis_vaksin.id";
            dataGridView1.DataSource = Command.getdata(com);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            dt.Columns.Add("NIK", typeof(string));
            dt.Columns.Add("Nama", typeof(string));
            dt.Columns.Add("Tanggal_Vaksin", typeof(string));
            dt.Columns.Add("Nama_Vaksin", typeof(string));

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dt.Rows.Add(dataGridView1.Rows[i].Cells[0].Value, dataGridView1.Rows[i].Cells[1].Value, dataGridView1.Rows[i].Cells[2].Value, dataGridView1.Rows[i].Cells[3].Value);
            }

            ds.Tables.Add(dt);
            ds.WriteXmlSchema("report.xml");

            CrystalReport1 cr = new CrystalReport1();
            cr.SetDataSource(ds);
            ShowReport show = new ShowReport();
            show.crystalReportViewer1.ReportSource = cr;
            show.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Vaksinasi vaksinasi = new Vaksinasi();
            this.Hide();
            vaksinasi.ShowDialog();
        }
    }
}
