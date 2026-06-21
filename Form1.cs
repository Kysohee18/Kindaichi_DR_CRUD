using ExcelDataReader;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace CrudMahasiswaADO
{
    public partial class FormMahasiswa : Form
    {
        private DAL dbLogic = new DAL();
        private BindingSource bindingSource = new BindingSource();
      
        private DataTable dtMahasiswa = new DataTable();

        public FormMahasiswa()
        {
            InitializeComponent();
        }

        private void SimpanLog(string pesan)
        {
            dbLogic.InsertLog(pesan);
        }

        private void FormMahasiswa_Load(object sender, EventArgs e)
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

            bindingNavigator1.BindingSource = bindingSource;
            LoadData();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                bindingSource.DataSource = dbLogic.GetMhs();
                dataGridView1.DataSource = bindingSource;
                DataGridViewImageColumn fotoColumn = (DataGridViewImageColumn)dataGridView1.Columns["Foto"];
                fotoColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;

                HitungTotal();
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    Console.WriteLine("Name: " + col.Name + " | DataPropertyName: " + col.DataPropertyName);
                }
                dataGridView1.Enabled = true;
                btnImpDb.Enabled = false;
                btninsert_1.Enabled = true;
                btn_update_1.Enabled = true;
                btn_delete_1.Enabled = true;
                btnCari_1.Enabled = true;
                btn_load_1.Enabled = true;
                btnResetData.Enabled = true;
                btnTestInjection.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }
        private void HitungTotal()
        {
            try
            {
                int totalMhs = dbLogic.GetTotalMahasiswa();
                labelTotal1.Text = "Total Mahasiswa: " + totalMhs.ToString();
            }
            catch (Exception)
            {
                labelTotal1.Text = "Total Mahasiswa: Eror";
            }
        }

        private void BindControls()
        {
            txtNim.DataBindings.Clear();
            txtNama.DataBindings.Clear();
            cmbJK.DataBindings.Clear();
            dptTanggalLahir.DataBindings.Clear();
            txtAlamat.DataBindings.Clear();
            txtProdi.DataBindings.Clear();

            txtNim.DataBindings.Add("Text", bindingSource, "NIM", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNama.DataBindings.Add("Text", bindingSource, "Nama", true, DataSourceUpdateMode.OnPropertyChanged);
            cmbJK.DataBindings.Add("Text", bindingSource, "JenisKelamin", true, DataSourceUpdateMode.OnPropertyChanged);
            dptTanggalLahir.DataBindings.Add("Value", bindingSource, "TanggalLahir", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAlamat.DataBindings.Add("Text", bindingSource, "Alamat", true, DataSourceUpdateMode.OnPropertyChanged);
            txtProdi.DataBindings.Add("Text", bindingSource, "KodeProdi", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dg = MessageBox.Show(
                    "Yakin ingin menghapus data?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dg == DialogResult.Yes)
                {
                    dbLogic.DeleteMhs(txtNim.Text);
                    MessageBox.Show("Data mahasiswa berhasil dihapus");
                    ClearForm();
                    btn_load_1.PerformClick();
                }
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("General Error : " + ex.Message);
            }
        }

        private void btnResetData_Click(object sender, EventArgs e)
        {
            try
            {
                dbLogic.resetData();
                MessageBox.Show("Data berhasil direset!");
                LoadData();
            }
            catch (Exception ex) { MessageBox.Show("Reset gagal: " + ex.Message); }
        }

        private void btnTestInjection_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    dbLogic.testInject(txtNim.Text);

                    LoadData();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("safe"))
                    {
                        SimpanLog(ex.Message);
                        MessageBox.Show("SQL Error : Unsafe UPDATE operation not allowed");
                    }
                    else
                    {
                        SimpanLog(ex.Message);
                        MessageBox.Show("SQL Error :" + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    SimpanLog(ex.Message);
                    MessageBox.Show("General Error :" + ex.Message);
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

                
                try
                {
                    if (row.Cells["Foto"].Value != null && row.Cells["Foto"].Value != DBNull.Value)
                    {
                        byte[] imgBytes = (byte[])row.Cells["Foto"].Value;

                        using (MemoryStream ms = new MemoryStream(imgBytes))
                        {
                            fotomhs.Image = Image.FromStream(ms);
                            fotomhs.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }
                    else
                    {
                        fotomhs.Image = null;
                    }
                }
                catch (Exception ex)
                {
                    
                    fotomhs.Image = null;
                    MessageBox.Show("Gagal memuat gambar mahasiswa: " + ex.Message);
                }
                
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
       

        private void btnRekap_Click(object sender, EventArgs e)
        {
            Form2 formRekap = new Form2();

            formRekap.Show();

            this.Hide();
        }
        

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(dbLogic.GetConnectionString()))
                {
                    conn.Open();
                    MessageBox.Show("Koneksi berhasil!");
                }
            }
                catch(SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("sql error " + ex.Message);
            }
                catch(Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("General error " + ex.Message);
            }
        }

        private void btninsert_1_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] ConvertImageToBytes(PictureBox pb)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }

                
                byte[] imgBytes = null;
                if (fotomhs.Image != null)
                {
                    imgBytes = ConvertImageToBytes(fotomhs);
                }

                dbLogic.InsertMhs(txtNim.Text, txtNama.Text, txtAlamat.Text, cmbJK.Text, dptTanggalLahir.Value.Date, txtProdi.Text, imgBytes);

                MessageBox.Show("Data mahasiswa berhasil ditambahkan!");
                ClearForm();
                btn_load_1.PerformClick(); 
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("General Error: " + ex.Message);
            }


        }

        private void btn_update_1_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] ConvertImageToBytes(PictureBox pb)
                {
                    using (MemoryStream ms= new MemoryStream())
                    {
                        pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
                byte[] imgBytes = ConvertImageToBytes(fotomhs);
                dbLogic.UpdateMhs(txtNim.Text, txtNama.Text, txtAlamat.Text, cmbJK.Text, dptTanggalLahir.Value.Date, txtProdi.Text, imgBytes);
                MessageBox.Show("Data mahasiswa berhasil diubah");
                ClearForm();
                btn_load_1.PerformClick();
            }
            catch(SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("sql error " + ex.Message);
            }
                catch(Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("General error " + ex.Message);
            }
            


        }

        private void btn_upload_1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fotomhs.Image = Image.FromFile(ofd.FileName);
                fotomhs.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        private void btnImpDb_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk diimport.");
                    return;
                }

                int sukses = 0;

                foreach (DataRow row in dt.Rows)
                {
                    string nim = row["NIM"].ToString().Trim();
                    string nama = row["Nama"].ToString().Trim();
                    string jk = row["JenisKelamin"].ToString().Trim();
                    string alamat = row["Alamat"].ToString().Trim();
                    string kodeProdi = row["NamaProdi"].ToString().Trim();
                    string fotoPath = row.Table.Columns.Contains("FotoPath")
                        ? row["FotoPath"].ToString().Trim()
                        : string.Empty;

                    if (string.IsNullOrEmpty(nim) || string.IsNullOrEmpty(nama))
                        continue;

                    DateTime tglLahir;
                    if (!DateTime.TryParse(row["TanggalLahir"].ToString(), out tglLahir))
                        continue;

                    // Eksekusi injeksi massal ke database melalui DAL
                    dbLogic.InsertMhs(nim, nama, alamat, jk, tglLahir.Date, kodeProdi, null);
                    sukses++;
                }

                MessageBox.Show(sukses + " Data mahasiswa berhasil diimport ke Database!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal import ke database: " + ex.Message);
            }
        }
        private void btnImpExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Excel Workbook| *.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });
                            DataTable dt = result.Tables[0];
                            dataGridView1.DataSource = dt;
                            dataGridView1.Enabled = false;

                            // Manajemen Status Tombol Antarmuka Sesuai Gambar Modul
                            btnImpDb.Enabled = true;
                            btninsert_1.Enabled = false;
                            btn_update_1.Enabled = false;
                            btn_delete_1.Enabled = false;
                            btnCari_1.Enabled = false;
                            btn_load_1.Enabled = false;
                            btnResetData.Enabled = false;
                            btnTestInjection.Enabled = false;
                        }
                    }
                }
            }
        }
    }
}
