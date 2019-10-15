namespace DCN.TicTacToe.UI
{
    partial class Frm_ConnectError
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
            this.SuspendLayout();
            // 
            // cbtn_Yes
            // 
            this.cbtn_Yes.BackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbtn_Yes.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cbtn_Yes.FlatAppearance.BorderSize = 0;
            this.cbtn_Yes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtn_Yes.Location = new System.Drawing.Point(162, 139);
            this.cbtn_Yes.Name = "cbtn_Yes";
            this.cbtn_Yes.Size = new System.Drawing.Size(66, 68);
            this.cbtn_Yes.TabIndex = 1;
            this.cbtn_Yes.TabStop = false;
            this.cbtn_Yes.Tag = "yes";
            this.cbtn_Yes.UseVisualStyleBackColor = false;
            this.cbtn_Yes.Click += new System.EventHandler(this.cbtn_Yes_Click);
            // 
            // Frm_ConnectError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.connect_error;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.cbtn_Yes;
            this.ClientSize = new System.Drawing.Size(391, 209);
            this.Controls.Add(this.cbtn_Yes);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_ConnectError";
            this.Text = "Frm_ConnectError";
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.ResumeLayout(false);

        }

        #endregion

        private CircularButton cbtn_Yes;
    }
}