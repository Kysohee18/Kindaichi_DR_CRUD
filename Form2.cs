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

namespace CrudMahasiswaADO
{  
    public partial class Form2 : Form
        
    {   DAL dbLogic = new DAL();
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtprodi;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dtpTanggalMasuk.Format = DateTimePickerFormat.Custom;
            dtpTanggalMasuk.CustomFormat = "yyyy";
            dtpTanggalMasuk.ShowUpDown = true;
            dtpTanggalMasuk.MinDate = new DateTime(2000, 1, 1);
            dtpTanggalMasuk.MaxDate = DateTime.Now;

            cmbProdi.DropDownStyle = ComboBoxStyle.DropDownList;
            btnCetak.Enabled = false;

            try
            {
                DataTable dtprodi = dbLogic.getProdi();

                cmbProdi.DataSource = dtprodi;
                cmbProdi.DisplayMember = "namaprodi";
                cmbProdi.ValueMember = "namaprodi";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data Prodi: " + ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) 
                {
                    conn.Open(); 
                }

                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.Add("@inProdi", SqlDbType.VarChar, 50).Value = cmbProdi.SelectedValue; 
                cmd.Parameters.Add("@inTglMsuk", SqlDbType.VarChar, 4).Value = dtpTanggalMasuk.Value.Year.ToString(); 

                da = new SqlDataAdapter(cmd); 
                dtMahasiswa = new DataTable(); 
                da.Fill(dtMahasiswa); 

                dataGridView1.DataSource = dtMahasiswa; 

                if (dtMahasiswa.Rows.Count > 0) 
                {
                    btnCetak.Enabled = true; 
                }
                else 
                {
                    btnCetak.Enabled = false; 
                    MessageBox.Show("Data tidak ditemukan"); 
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Gagal Load data: " + ex.Message); 
            }
        }
        private void btnCetak_Click(object sender, EventArgs e)
        {
            string prodiTerpilih = cmbProdi.SelectedValue.ToString();
            DateTime tanggalTerpilih = dtpTanggalMasuk.Value;

            Form3 fm3 = new Form3(prodiTerpilih, tanggalTerpilih);

           
            fm3.Show();
            this.Hide();
        }



    }
    


}
