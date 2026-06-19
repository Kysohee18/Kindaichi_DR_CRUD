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

       
        public Form3(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();
            prodi = Prodi;
            tglmasuk = TglMasuk;
            try
            {
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tglmasuk);
                listmahasiswa.setDataSource(dtMahasiswa);
                crystalReportViewer1.ReportSource = listMahasiswa;
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
              
                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inProdi", prodi);
                cmd.Parameters.AddWithValue("@inTglMsuk", tglmasuk.Year.ToString());

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();

                da.Fill(dtMahasiswa);

                CrystalReport1 laporan = new CrystalReport1();

                laporan.SetDataSource(dtMahasiswa);

                crystalReportViewer1.ReportSource = laporan;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Memuat Laporan: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}