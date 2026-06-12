using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CrudMahasiswaADO
{
    public partial class Form3 : Form
    {
        
        static string connectionString = @"Data Source=DESKTOP-6V58GOQ\PUTRASQL; Initial Catalog=DBAkademikADO; Integrated Security=True";
        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;

        CrystalReport1 laporan = new CrystalReport1();

        string prodi { get; set; }  
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
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                
                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inProdi", prodi);
                cmd.Parameters.AddWithValue("@inTglMsuk", tglmasuk.Year.ToString());

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);

                laporan.SetDataSource(dtMahasiswa);

                
                crystalReportViewer1.ReportSource = laporan;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Memuat Laporan: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
