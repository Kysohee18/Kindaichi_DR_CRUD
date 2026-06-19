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
            dptTanggalMasuk.mindate = new DateTime(2000, 1, 1);
            dptTanggalmasuk.format = DateTimePickerFormat.Custom;
            dptTanggalMasuk.customformat = "yyyy";
            dptTanggalMasuk.showupdown = true;
            dptTanggalMasuk.maxdate = DateTime.Now;
            
            dtpTanggalMasuk.MinDate = new DateTime(2000, 1, 1);
            dtpTanggalMasuk.MaxDate = DateTime.Now;
            dtpTanggalMasuk.Format = DateTimePickerFormat.Custom;
            dtpTanggalMasuk.CustomFormat = "yyyy";
            dtpTanggalMasuk.ShowUpDown = true;

            cmbTipe.DropDownStyle = ComboBoxStyle.DropDownList;

            var items = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, SeriesChartType>>
            {
                new System.Collections.Generic.KeyValuePair<string, SeriesChartType>("Kolom", SeriesChartType.Column),
                new System.Collections.Generic.KeyValuePair<string, SeriesChartType>("Pie", SeriesChartType.Pie)
            };

            cmbTipe.DataSource = items;
            cmbTipe.DisplayMember = "Key";
            cmbTipe.ValueMember = "Value";
            cmbTipe.SelectedIndex = 0;

            isInitializing = false;


            LoadDataChart();
        }
        public void LoadDataChart()
        {
            // Pembersihan memori kanvas secara radikal sebelum merender ulang
            chartProdi.Series.Clear();
            chartProdi.Titles.Clear();
            chartProdi.Legends.Clear();
            chartProdi.ChartAreas.Clear();
            
            // Pembuatan area grafik
            ChartArea ca = new ChartArea("MainArea");
            ca.AxisX.Title = "Program Studi";
            ca.AxisY.Title = "Jumlah Mahasiswa";
            ca.AxisX.LabelStyle.Angle = -45;
            chartProdi.ChartAreas.Add(ca);

            try
            {
                // Penentuan jalur pipa data berdasarkan filter
                if (buttonState == 1)
                {
                    dt = dbLogic.getDataChartByTahun(dtpTanggalMasuk.Value);
        }
                else
        {
                    dt = dbLogic.getAllDataChart();
                }

                // Penjahitan data ke dalam Series Grafik
                SeriesChartType tipeTerpilih = (SeriesChartType)cmbTipe.SelectedValue;
                Series s = new Series("Distribusi Mahasiswa");
                s.ChartType = tipeTerpilih;
                s.IsValueShownAsLabel = true;

                if (tipeTerpilih == SeriesChartType.Pie)
                {
                    s.Label = "#VAL";       // Nilai angka
                    s.LegendText = "#VALX"; // Nama kategori
        }

                // Iterasi matriks data dari database ke titik koordinat visual
                foreach (DataRow row in dt.Rows)
        {
                    string namaProdi = row["NamaProdi"].ToString();
                    int jumlahMhs = Convert.ToInt32(row["JmlhMhs"]);
                    s.Points.AddXY(namaProdi, jumlahMhs);
                }

                chartProdi.Series.Add(s);

                // Ornamen judul dan legenda
                chartProdi.Titles.Add(new Title("Statistik Mahasiswa per Program Studi", Docking.Top, new Font("Segoe UI", 14, FontStyle.Bold), Color.Black));
                chartProdi.Legends.Add(new Legend("LegendArea") { Docking = Docking.Right });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kegagalan rendering arsitektural: " + ex.Message, "System Error");
        }
    }
}
