using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CrudMahasiswaADO
{
    public partial class Form3 : Form
    {
        // Gunakan localhost untuk mengamankan soket jaringan pelaporan
        static string connectionString = @"Data Source=localhost\PUTRASQL; Initial Catalog=DBAkademikADO; Integrated Security=True";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;

        // Instansiasi dokumen laporan murni sesuai modul
        CrystalReport1 laporan = new CrystalReport1();

        string prodi;
        DateTime tglmasuk;

        public Form3(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();
            prodi = Prodi;
            tglmasuk = TglMasuk;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inProdi", prodi);
                cmd.Parameters.AddWithValue("@inTglMsuk", tglmasuk.Year.ToString());

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();

                // Mengisi data sekaligus membuka-menutup koneksi secara otomatis
                da.Fill(dtMahasiswa);

                // Menyuntikkan data murni sesuai modul
                laporan.SetDataSource(dtMahasiswa);

                // Memproyeksikan laporan ke komponen viewer UI
                crystalReportViewer1.ReportSource = laporan;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Memuat Laporan: " + ex.Message);
            }
        }
    }
}