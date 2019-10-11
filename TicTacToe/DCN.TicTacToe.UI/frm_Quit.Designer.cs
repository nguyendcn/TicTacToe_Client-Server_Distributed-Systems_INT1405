namespace DCN.TicTacToe.UI
{
    partial class Frm_Quit
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
            this.cbtn_Yes = new DCN.TicTacToe.UI.CircularButton();
            this.cbtn_No = new DCN.TicTacToe.UI.CircularButton();
            this.SuspendLayout();
            // 
            // cbtn_Yes
            // 
            this.cbtn_Yes.BackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbtn_Yes.FlatAppearance.BorderSize = 0;
            this.cbtn_Yes.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtn_Yes.Location = new System.Drawing.Point(199, 124);
            this.cbtn_Yes.Name = "cbtn_Yes";
            this.cbtn_Yes.Size = new System.Drawing.Size(57, 59);
            this.cbtn_Yes.TabIndex = 1;
            this.cbtn_Yes.UseVisualStyleBackColor = false;
            this.cbtn_Yes.Click += new System.EventHandler(this.cbtn_Yes_Click);
            this.cbtn_Yes.MouseHover += new System.EventHandler(this.cbtn_Option_MouseHover);
            // 
            // cbtn_No
            // 
            this.cbtn_No.BackColor = System.Drawing.Color.Transparent;
            this.cbtn_No.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbtn_No.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cbtn_No.FlatAppearance.BorderSize = 0;
            this.cbtn_No.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cbtn_No.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbtn_No.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbtn_No.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtn_No.Location = new System.Drawing.Point(95, 125);
            this.cbtn_No.Name = "cbtn_No";
            this.cbtn_No.Size = new System.Drawing.Size(57, 59);
            this.cbtn_No.TabIndex = 2;
            this.cbtn_No.UseVisualStyleBackColor = false;
            this.cbtn_No.Click += new System.EventHandler(this.cbtn_No_Click);
            this.cbtn_No.MouseHover += new System.EventHandler(this.cbtn_Option_MouseHover);
            // 
            // Frm_Quit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.Warning;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.cbtn_No;
            this.ClientSize = new System.Drawing.Size(354, 187);
            this.Controls.Add(this.cbtn_No);
            this.Controls.Add(this.cbtn_Yes);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Quit";
            this.Text = "frm_Quit";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.ResumeLayout(false);

        }

        #endregion

        private CircularButton cbtn_Yes;
        private CircularButton cbtn_No;
    }
}