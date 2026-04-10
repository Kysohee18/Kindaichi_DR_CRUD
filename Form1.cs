using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CrudMahasiswaADO
{
    public partial class FormMahasiswa : Form
    {       
        private SqlConnection conn;
        private readonly string connectionString = @"Data Source=DESKTOP-6V58GOQ\PUTRASQL; Initial Catalog=DBAkademikADO; Integrated Security=True";
        public FormMahasiswa()
        {
            InitializeComponent();
           conn = new SqlConnection(connectionString);
        }
        private void ConnectToDatabase()
        {
            try
            {   if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                MessageBox.Show("Koneksi berhasil!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
           
            {
                conn.Close();
            }
        }
        private void For_Load(object sender, EventArgs e)
        {
            cmbJK.Items.Clear();
            cmbJK.Items.Add("L");
            cmbJK.Items.Add("P");

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtNim_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNama_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbJK_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dptTanggalLahir_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtAlamat_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtProdi_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToDatabase();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    dataGridView1.Columns.Add("NIM", "NIM");
                    dataGridView1.Columns.Add("Nama", "Nama");
                    dataGridView1.Columns.Add("JenisKelamin", "Jenis Kelamin");
                    dataGridView1.Columns.Add("Tanggal Lahir", "Tanggal Lahir");
                    dataGridView1.Columns.Add("Alamat", "Alamat");
                    dataGridView1.Columns.Add("KodeProdi", "Kode Prodi");

                    string query = "SELECT FROM Mahasiswa";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(
                            reader["NIM"].ToString(),
                            reader["Nama"].ToString(),
                            reader["JenisKelamin"].ToString(),
                            Convert.ToDateTime(reader["Tanggallahir"]).ToShortDateString(),
                            reader["Alamat"].ToString(),
                            reader["KodeProdi"].ToString()
                        );
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }

        }

        private void addData_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
