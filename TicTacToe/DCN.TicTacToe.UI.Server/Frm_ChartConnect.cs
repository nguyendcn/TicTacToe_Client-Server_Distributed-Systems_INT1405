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

namespace DCN.TicTacToe.UI.Server
{
    
    public partial class Frm_ChartConnect : Form
    {
        public delegate void GetDataChart(List<int> data);
        public event GetDataChart UpdateChart;
        private Timer timer;

        private List<int> data;
        public List<int> Data
        {
            get { return this.data; }
            set
            {
                this.data = value;
                UpdateChartTime();
            }
        }

        public Frm_ChartConnect()
        {
            InitializeComponent();

            data = new List<int>(30);
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start(); 

            var chart = chr_Data.ChartAreas[0];
            chart.AxisX.IntervalType = DateTimeIntervalType.Number;

            chart.AxisX.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.IsEndLabelVisible = true;

            chart.AxisX.Minimum = 1;
            chart.AxisX.Maximum = 30;
            chart.AxisY.Minimum = 0;
            chart.AxisY.Maximum = 50;
            chart.AxisY.Interval = 1;
            chart.AxisX.Interval = 1;

            chr_Data.Series.Add("Online");
            chr_Data.Series["Online"].ChartType = SeriesChartType.Spline;
            chr_Data.Series["Online"].Color = Color.Red;
            chr_Data.Series[0].IsVisibleInLegend = false;
            //Random random = new Random();
            //for (int i = 0; i < 30; i++)
            //{

            //    chr_Data.Series["Online"].Points.AddXY(i + 1, random.Next(0, 50));
            //}
            UpdateChartTime();
        }

        private void UpdateChartTime()
        {
            chr_Data.Series["Online"].Points.Clear();
            for(int i = 0; i < data.Count; i++)
            {
                chr_Data.Series["Online"].Points.AddXY(i + 1, data[i]);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OnUpdateChart(this.data);
        }

        public virtual void OnUpdateChart(List<int> data)
        {
            if (this.UpdateChart != null) UpdateChart(data);
        }
    }
}
