using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CrudMahasiswaADO
{
    public partial class Dasboard : Form
    {
        DAL dbLogic = new DAL();
        bool isInitializing = true;
        DataTable dt;
        int buttonState = 0; // 0 = Semua Data, 1 = Filter Tahun
        public Dasboard()
        {
            InitializeComponent();

            dtpTanggalMasuk.MinDate = new DateTime(2000, 1, 1);
            dtpTanggalMasuk.MaxDate = DateTime.Now;
            dtpTanggalMasuk.Format = DateTimePickerFormat.Custom;
            dtpTanggalMasuk.CustomFormat = "yyyy";
            dtpTanggalMasuk.ShowUpDown = true;

            cmbTipe.DropDownStyle = ComboBoxStyle.DropDownList;
            var items = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.DataVisualization.Charting.SeriesChartType>>
            {
                new System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.DataVisualization.Charting.SeriesChartType>("Kolom", System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column),
                new System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.DataVisualization.Charting.SeriesChartType>("Pie", System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie)
            };
            isInitializing = true;
            cmbTipe.DataSource = items;
            cmbTipe.DisplayMember = "Key";
            cmbTipe.ValueMember = "Value";
            cmbTipe.SelectedIndex = 0;
            isInitializing = false;

            // Pemicu render perdana
            LoadDataChart();
        }
        private void Dasboard_Load(object sender, EventArgs e)
        {
            
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            buttonState = 1;

            
            LoadDataChart();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            buttonState = 0;

            LoadDataChart();
        }
        private void btnDatamhs_Click(object sender, EventArgs e)
        {
            FormMahasiswa formUtama = new FormMahasiswa();


            formUtama.Show();

            this.Close();
        }
        public void LoadDataChart()
        {
            chartProdi.Series.Clear();
            chartProdi.Titles.Clear();
            chartProdi.Legends.Clear();
            chartProdi.ChartAreas.Clear();

            System.Windows.Forms.DataVisualization.Charting.ChartArea ca = new System.Windows.Forms.DataVisualization.Charting.ChartArea("MainArea");
            ca.AxisX.Title = "Program Studi";
            ca.AxisY.Title = "Jumlah Mahasiswa";
            ca.AxisX.LabelStyle.Angle = -45;
            chartProdi.ChartAreas.Add(ca);

            try
            {
                // Routing Data
                if (buttonState == 1)
                {
                    dt = dbLogic.getDataChartByTahun(dtpTanggalMasuk.Value);
                }
                else
                {
                    dt = dbLogic.getAllDataChart();
                }

                System.Windows.Forms.DataVisualization.Charting.SeriesChartType tipeTerpilih = (System.Windows.Forms.DataVisualization.Charting.SeriesChartType)cmbTipe.SelectedValue;
                System.Windows.Forms.DataVisualization.Charting.Series s = new System.Windows.Forms.DataVisualization.Charting.Series("Mahasiswa");
                s.ChartType = tipeTerpilih;
                s.IsValueShownAsLabel = true;

                if (tipeTerpilih == System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie)
                {
                    s.Label = "#VAL";
                    s.LegendText = "#VALX";
                }

                
                foreach (DataRow row in dt.Rows)
                {
                    string namaProdi = row["NamaProdi"].ToString();
                    int jumlahMhs = Convert.ToInt32(row["JmlhMhs"]);
                    s.Points.AddXY(namaProdi, jumlahMhs);
                }

                chartProdi.Series.Add(s);

                chartProdi.Titles.Add(new System.Windows.Forms.DataVisualization.Charting.Title("Statistik Program Studi", System.Windows.Forms.DataVisualization.Charting.Docking.Top, new Font("Arial", 14, FontStyle.Bold), Color.Black));
                chartProdi.Legends.Add(new System.Windows.Forms.DataVisualization.Charting.Legend("LegendArea") { Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Right });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal merender grafik: " + ex.Message);
            }
        }


        private void cmbTipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Pengaman saat komponen awal dimuat
            if (isInitializing) return;

            // Memanggil mesin render secara presisi dengan tanda kurung ()
            LoadDataChart();
        }

        
    }
}
