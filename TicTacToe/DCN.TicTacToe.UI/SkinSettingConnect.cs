using DCN.TicTacToe.Shared.Enum;
using System;
using System.ComponentModel;

namespace DCN.TicTacToe.UI
{

    /// <summary>
    /// The class of the control - the pop-up tooltip.
    /// Allows the child elements to be arranged in the tooltip.
    /// Allows the shape to be specified with the help of a bitmap.
    /// </summary>
    [
    ToolboxItem(true),
    ]
    public class SkinSettingConnect : DCN.TicTacToe.UI.SkinComponents.SkinControl
    {

        public event Action<String, int> ActionForm;
       
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.TextBox txt_Port;
        private System.Windows.Forms.TextBox txt_Address;

        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #endregion

        #region Constructors

        /// <summary>
        /// the class constructor.
        /// Setts the animation timers and turns on the message handlers.		
        /// </summary>
        public SkinSettingConnect()
        {
            InitializeComponent();

        }

        public SkinSettingConnect(String address, int port)
        {
            InitializeComponent();

            this.txt_Address.Text = address;
            this.txt_Port.Text = port.ToString();
        }

        public SkinSettingConnect(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Virtual
       
        #endregion
        #region Methods
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Connect = new System.Windows.Forms.Button();
            this.txt_Port = new System.Windows.Forms.TextBox();
            this.txt_Address = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Connect
            // 
            this.btn_Connect.BackColor = System.Drawing.Color.Transparent;
            this.btn_Connect.FlatAppearance.BorderSize = 0;
            this.btn_Connect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_Connect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_Connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Connect.Location = new System.Drawing.Point(94, 249);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(203, 62);
            this.btn_Connect.TabIndex = 5;
            this.btn_Connect.TabStop = false;
            this.btn_Connect.UseVisualStyleBackColor = false;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // txt_Port
            // 
            this.txt_Port.Location = new System.Drawing.Point(165, 191);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(164, 20);
            this.txt_Port.TabIndex = 4;
            // 
            // txt_Address
            // 
            this.txt_Address.Location = new System.Drawing.Point(165, 111);
            this.txt_Address.Name = "txt_Address";
            this.txt_Address.Size = new System.Drawing.Size(164, 20);
            this.txt_Address.TabIndex = 3;
            // 
            // SkinSettingConnect
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.setting_form1;
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.txt_Port);
            this.Controls.Add(this.txt_Address);
            this.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.Name = "SkinSettingConnect";
            this.PatternBitmap = global::DCN.TicTacToe.UI.Properties.Resources.setting_form1;
            this.Size = new System.Drawing.Size(391, 330);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #endregion

        #region Event handler
        public virtual void OnActionForm(String address, int port)
        {
            if (ActionForm != null) ActionForm(address, port);
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            OnActionForm(this.txt_Address.Text, int.Parse(this.txt_Port.Text));
            this.Dispose();
        }
        #endregion
    }

}
