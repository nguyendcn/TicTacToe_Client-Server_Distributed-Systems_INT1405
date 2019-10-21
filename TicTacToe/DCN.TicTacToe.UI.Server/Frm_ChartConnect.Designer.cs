namespace DCN.TicTacToe.UI.Server
{
    partial class Frm_ChartConnect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chr_Data = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chr_Data)).BeginInit();
            this.SuspendLayout();
            // 
            // chr_Data
            // 
            chartArea1.Name = "ChartArea1";
            this.chr_Data.ChartAreas.Add(chartArea1);
            this.chr_Data.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chr_Data.Legends.Add(legend1);
            this.chr_Data.Location = new System.Drawing.Point(0, 0);
            this.chr_Data.Name = "chr_Data";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chr_Data.Series.Add(series1);
            this.chr_Data.Size = new System.Drawing.Size(800, 450);
            this.chr_Data.TabIndex = 0;
            this.chr_Data.Text = "Online";
            // 
            // Frm_ChartConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chr_Data);
            this.Name = "Frm_ChartConnect";
            this.Text = "Frm_ChartConnect";
            ((System.ComponentModel.ISupportInitialize)(this.chr_Data)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chr_Data;
    }
}