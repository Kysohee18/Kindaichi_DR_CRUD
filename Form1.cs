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
using System.Xml.Serialization;
using System.Linq.Expressions;

namespace CrudMahasiswaADO
{
    public partial class FormMahasiswa : Form
    {  
       public FormMahasiswa()
        {
            InitializeComponent();
            DAL dbLogic = new DAL()
        }

        private void SimpanLog(string pesan)
        {
            dbLogic.InsertLog(pesan);
        }

        private void For_Load(object sender, EventArgs e)
        {

            this.mahasiswaTableAdapter.Fill(this.dBAkademikADODataSet.Mahasiswa);
            cmbJK.Items.Clear();
            cmbJK.Items.Add("L");
            cmbJK.Items.Add("P");

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
          try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Koneksi berhasil!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();

        }

        
            
             
                
               
        

        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNim.Text)) return;

            var confirm = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@NIM", SqlDbType.Char, 11).Value = txtNim.Text;

                            conn.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                                MessageBox.Show("Data berhasil dihapus!");
                            else
                                MessageBox.Show("Data tidak ditemukan");
                        }
                    }
                    MessageBox.Show("Data berhasil dihapus!");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kesalahan: " + ex.Message);
                }
            }
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtNim.Text = row.Cells["NIM"].Value.ToString();
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                cmbJK.Text = row.Cells["JenisKelamin"].Value.ToString();
                dptTanggalLahir.Value = Convert.ToDateTime(row.Cells["TanggalLahir"].Value);
                txtAlamat.Text = row.Cells["Alamat"].Value.ToString();
                txtProdi.Text = row.Cells["KodeProdi"].Value.ToString();
            }
        }

        private void ClearForm()
        {
            txtNim.Clear();
            txtNama.Clear();
            cmbJK.SelectedIndex = -1;
            dptTanggalLahir.Value = DateTime.Now;
            txtAlamat.Clear();
            txtProdi.Clear();
        }

        private void FormMahasiswa_Load(object sender, EventArgs e)
        {
            cmbJK.DataSource = new string[] { "L", "P" };
            //grid
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //binding navigator
            bindingNavigator1.BindingSource = bindingSource;
            
            LoadData();
        }
        //loaddata
        

        private void BindControls()
        {
            // Clear existing bindings to prevent duplicates
            txtNim.DataBindings.Clear();
            txtNama.DataBindings.Clear();
            cmbJK.DataBindings.Clear();
            dptTanggalLahir.DataBindings.Clear();
            txtAlamat.DataBindings.Clear();
            txtProdi.DataBindings.Clear();

            // Bind to BindingSource
            txtNim.DataBindings.Add("Text", bindingSource, "NIM", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNama.DataBindings.Add("Text", bindingSource, "Nama", true, DataSourceUpdateMode.OnPropertyChanged);
            cmbJK.DataBindings.Add("Text", bindingSource, "JenisKelamin", true, DataSourceUpdateMode.OnPropertyChanged);
            dptTanggalLahir.DataBindings.Add("Value", bindingSource, "TanggalLahir", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAlamat.DataBindings.Add("Text", bindingSource, "Alamat", true, DataSourceUpdateMode.OnPropertyChanged);
            txtProdi.DataBindings.Add("Text", bindingSource, "KodeProdi", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        //belum selesai
        

        private void btnResetData_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        IF OBJECT_ID('dbo.Mahasiswa_Backup') IS NOT NULL
                        BEGIN
                            DELETE FROM dbo.Mahasiswa;
                            INSERT INTO dbo.Mahasiswa
                            SELECT * FROM dbo.Mahasiswa_Backup;
                        END";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil direset!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: " + ex.Message);
            }
        }

        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    using (SqlConnection conn = 
                        new SqlConnection(connectionString))
                    {
                       

                        string query =
                            "UPDATE Mahasiswa SET Nama ='" +   
                            txtNama.Text +
                            "' WHERE NIM ='" + 
                            txtNim.Text + "'";

                        SqlCommand cmd =
                            new SqlCommand(query, conn);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        
                        MessageBox.Show("update berhasil!");
                        
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btnRekap_Click(object sender, EventArgs e)
        {
            
            Form2 fm2 = new Form2();
            fm2.Show();
            this.Hide();
        }

    }

}

