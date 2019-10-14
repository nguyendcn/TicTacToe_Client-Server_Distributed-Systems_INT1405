namespace DCN.TicTacToe.UI
{
    partial class Frm_Login
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
            this.txt_Username = new System.Windows.Forms.TextBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.pnl_Exists = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // txt_Username
            // 
            this.txt_Username.Location = new System.Drawing.Point(109, 275);
            this.txt_Username.Name = "txt_Username";
            this.txt_Username.Size = new System.Drawing.Size(253, 20);
            this.txt_Username.TabIndex = 0;
            this.txt_Username.TextChanged += new System.EventHandler(this.txt_Username_TextChanged);
            // 
            // btn_Login
            // 
            this.btn_Login.BackColor = System.Drawing.Color.Transparent;
            this.btn_Login.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Login.FlatAppearance.BorderSize = 0;
            this.btn_Login.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_Login.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_Login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Login.Location = new System.Drawing.Point(138, 387);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(195, 59);
            this.btn_Login.TabIndex = 3;
            this.btn_Login.TabStop = false;
            this.btn_Login.UseVisualStyleBackColor = false;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // pnl_Exists
            // 
            this.pnl_Exists.BackColor = System.Drawing.Color.Transparent;
            this.pnl_Exists.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.username_exists_text;
            this.pnl_Exists.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnl_Exists.Location = new System.Drawing.Point(32, 220);
            this.pnl_Exists.Name = "pnl_Exists";
            this.pnl_Exists.Size = new System.Drawing.Size(401, 28);
            this.pnl_Exists.TabIndex = 4;
            this.pnl_Exists.Visible = false;
            // 
            // Frm_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.login;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(454, 518);
            this.Controls.Add(this.pnl_Exists);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.txt_Username);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Login";
            this.Opacity = 0D;
            this.Text = "Frm_Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Username;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.Panel pnl_Exists;
    }
}