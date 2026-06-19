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
        DAL dblogic = new DAL();
        bool isInitializing = true;
        DataTable dt;
        int button = 0;
        public Dasboard()
        {
            InitializeComponent();
            dptTanggalMasuk.mindate = new DateTime(2000, 1, 1);
            dptTanggalmasuk.format = DateTimePickerFormat.Custom;
            dptTanggalMasuk.customformat = "yyyy";
            dptTanggalMasuk.showupdown = true;
            dptTanggalMasuk.maxdate = DateTime.Now;
            
            cmbTipe.dropdownstyle = ComboBoxStyle.DropDownList;
            var items = new List<Keyvaluepair<string, SeriesChartType>>
            {
                new KeyValuePair<string, SeriesChartType>("Kolom", SeriesChartType.Coloum),
                new KeyValuePair<string, SeriesChartType>("Pie", SeriesChartType.Pie),
            };
            isInitializing = true;
            cmbtipe.datasource = items;
            cmbtipe.Displaymember = "key";
            cmbtipe.valuemember = "value";
            cmbtipe.SelectedIndex = 0;

            isInitializing = false;
            loadDataChart();
        }
        public void LoadDataChart()
        {
            chartprodi.series.Clear();
            chartprodi.titles.Clear();
            chartprodi.Legend.Clear();
            chartprodi.ChartAreas.Clear();
            
            ChartArea ca = new ChartArea ("MainArea");
            ca.Axisy.Title = "Program Studi";
            ca.AxisY.Title = "jumlah mahasiswa";

        }
        private void Dasboard_Load(object sender, EventArgs e)
        {

        }

        private void tipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;
            if (button == 1) { }
            else
            {
                    LoadDataChart

            }
        }
    }
}
