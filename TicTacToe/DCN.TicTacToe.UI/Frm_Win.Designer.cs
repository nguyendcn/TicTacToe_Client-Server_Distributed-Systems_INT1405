namespace DCN.TicTacToe.UI
{
    partial class Frm_Win
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
            this.cbtn_Continute = new DCN.TicTacToe.UI.CircularButton();
            this.SuspendLayout();
            // 
            // cbtn_Continute
            // 
            this.cbtn_Continute.BackColor = System.Drawing.Color.Transparent;
            this.cbtn_Continute.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.Play_btn;
            this.cbtn_Continute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbtn_Continute.FlatAppearance.BorderSize = 0;
            this.cbtn_Continute.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Continute.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Continute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtn_Continute.Location = new System.Drawing.Point(106, 225);
            this.cbtn_Continute.Name = "cbtn_Continute";
            this.cbtn_Continute.Size = new System.Drawing.Size(68, 68);
            this.cbtn_Continute.TabIndex = 0;
            this.cbtn_Continute.UseVisualStyleBackColor = false;
            this.cbtn_Continute.Click += new System.EventHandler(this.cbtn_Continute_Click);
            // 
            // Frm_Win
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.WinTable;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(292, 321);
            this.Controls.Add(this.cbtn_Continute);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Win";
            this.Text = "Frm_Win";
            this.TransparencyKey = System.Drawing.SystemColors.ButtonShadow;
            this.ResumeLayout(false);

        }

        #endregion

        private CircularButton cbtn_Continute;
    }
}