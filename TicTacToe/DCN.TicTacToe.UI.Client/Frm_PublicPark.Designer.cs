namespace DCN.TicTacToe.UI.Client
{
    partial class Frm_PublicPark
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
            this.pnl_AreaPark = new System.Windows.Forms.Panel();
            this.pnl_Control = new System.Windows.Forms.Panel();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.btn_SendMess = new System.Windows.Forms.Button();
            this.txt_Message = new System.Windows.Forms.TextBox();
            this.pnl_Control.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_AreaPark
            // 
            this.pnl_AreaPark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_AreaPark.Location = new System.Drawing.Point(0, 0);
            this.pnl_AreaPark.Name = "pnl_AreaPark";
            this.pnl_AreaPark.Size = new System.Drawing.Size(800, 450);
            this.pnl_AreaPark.TabIndex = 0;
            this.pnl_AreaPark.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnl_AreaPark_MouseUp);
            // 
            // pnl_Control
            // 
            this.pnl_Control.Controls.Add(this.btn_Exit);
            this.pnl_Control.Controls.Add(this.btn_SendMess);
            this.pnl_Control.Controls.Add(this.txt_Message);
            this.pnl_Control.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_Control.Location = new System.Drawing.Point(0, 417);
            this.pnl_Control.Name = "pnl_Control";
            this.pnl_Control.Size = new System.Drawing.Size(800, 33);
            this.pnl_Control.TabIndex = 1;
            // 
            // btn_Exit
            // 
            this.btn_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Exit.Location = new System.Drawing.Point(6, 4);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(0, 0);
            this.btn_Exit.TabIndex = 2;
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // btn_SendMess
            // 
            this.btn_SendMess.Location = new System.Drawing.Point(716, 4);
            this.btn_SendMess.Name = "btn_SendMess";
            this.btn_SendMess.Size = new System.Drawing.Size(75, 23);
            this.btn_SendMess.TabIndex = 1;
            this.btn_SendMess.Text = "Send";
            this.btn_SendMess.UseVisualStyleBackColor = true;
            this.btn_SendMess.Click += new System.EventHandler(this.btn_SendMess_Click);
            // 
            // txt_Message
            // 
            this.txt_Message.Location = new System.Drawing.Point(436, 6);
            this.txt_Message.Name = "txt_Message";
            this.txt_Message.Size = new System.Drawing.Size(265, 20);
            this.txt_Message.TabIndex = 0;
            // 
            // Frm_PublicPark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Exit;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnl_Control);
            this.Controls.Add(this.pnl_AreaPark);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_PublicPark";
            this.Text = "Frm_PublicPark";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnl_Control.ResumeLayout(false);
            this.pnl_Control.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_AreaPark;
        private System.Windows.Forms.Panel pnl_Control;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Button btn_SendMess;
        private System.Windows.Forms.TextBox txt_Message;
    }
}