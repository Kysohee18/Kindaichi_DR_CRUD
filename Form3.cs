using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CrudMahasiswaADO
{
    public partial class Form3 : Form
    {
        private DAL dbLogic = new DAL();
        static string connectionString = @"Data Source=localhost\PUTRASQL; Initial Catalog=DBAkademikADO; Integrated Security=True";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;

        string prodi;
        DateTime tglmasuk;
        CrystalReport1 listmahasiswa = new CrystalReport1();

        public Form3(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();
            prodi = Prodi;
            tglmasuk = TglMasuk;
            try
            {
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tglmasuk);
                listmahasiswa.SetDataSource(dtMahasiswa);
                crystalReportViewer1.ReportSource = listmahasiswa;
                crystalReportViewer1.Refresh();
            }
                catch(Exception ex)
            {
                MessageBox.Show("Gagal loat data: " + ex.Message);
            }
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                // Memanfaatkan DAL yang sudah Anda bangun dengan sempurna
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tglmasuk);

                if (dtMahasiswa != null && dtMahasiswa.Rows.Count > 0)
                {
                    // Membentuk satu instansiasi laporan yang bersih
                    CrystalReport1 laporan = new CrystalReport1();
                    laporan.SetDataSource(dtMahasiswa);

                    // Menyuntikkan laporan ke dalam viewer
                    crystalReportViewer1.ReportSource = laporan;
                    crystalReportViewer1.Refresh();
                }
                else
                {
                    MessageBox.Show("Data rekapitulasi kosong atau tidak ditemukan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat laporan: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}