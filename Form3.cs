using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CrudMahasiswaADO
{
    public partial class Form3 : Form
    {
        private DAL dbLogic = new DAL();
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
                
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tglmasuk);

                if (dtMahasiswa != null && dtMahasiswa.Rows.Count > 0)
                {
                    CrystalReport1 laporan = new CrystalReport1();
                    laporan.SetDataSource(dtMahasiswa);

                    
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