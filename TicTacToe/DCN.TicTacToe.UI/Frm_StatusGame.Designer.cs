namespace DCN.TicTacToe.UI
{
    partial class Frm_StatusGame
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
            this.btn_Title = new System.Windows.Forms.Button();
            this.cbtn_Continute = new DCN.TicTacToe.UI.CircularButton();
            this.SuspendLayout();
            // 
            // btn_Title
            // 
            this.btn_Title.BackColor = System.Drawing.Color.Transparent;
            this.btn_Title.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.text_tie;
            this.btn_Title.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Title.FlatAppearance.BorderSize = 0;
            this.btn_Title.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_Title.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_Title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Title.Location = new System.Drawing.Point(87, 4);
            this.btn_Title.Name = "btn_Title";
            this.btn_Title.Size = new System.Drawing.Size(110, 30);
            this.btn_Title.TabIndex = 99999;
            this.btn_Title.UseVisualStyleBackColor = false;
            // 
            // cbtn_Continute
            // 
            this.cbtn_Continute.BackColor = System.Drawing.Color.Transparent;
            this.cbtn_Continute.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.Play_btn;
            this.cbtn_Continute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbtn_Continute.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cbtn_Continute.FlatAppearance.BorderSize = 0;
            this.cbtn_Continute.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Continute.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Continute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtn_Continute.Location = new System.Drawing.Point(113, 226);
            this.cbtn_Continute.Name = "cbtn_Continute";
            this.cbtn_Continute.Size = new System.Drawing.Size(68, 68);
            this.cbtn_Continute.TabIndex = 999999;
            this.cbtn_Continute.UseVisualStyleBackColor = false;
            this.cbtn_Continute.Click += new System.EventHandler(this.cbtn_Continute_Click);
            // 
            // Frm_StatusGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.Table;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.cbtn_Continute;
            this.ClientSize = new System.Drawing.Size(288, 319);
            this.Controls.Add(this.cbtn_Continute);
            this.Controls.Add(this.btn_Title);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_StatusGame";
            this.Text = "Frm_StatusGame";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Load += new System.EventHandler(this.Frm_StatusGame_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Title;
        private CircularButton cbtn_Continute;
    }
}