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
    public class SkinFrmLogin : DCN.TicTacToe.UI.SkinComponents.SkinControl
    {
        private System.Windows.Forms.Panel pnl_Exists;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.TextBox txt_Username;

        public event Action<SkinFrmLogin, String> ActionForm;

        private bool userIsExists;

        public bool UserIsExists
        {
            get { return this.userIsExists; }

            set
            {
                this.pnl_Exists.Visible = value;
                this.userIsExists = value;

                if (!this.userIsExists)
                {
                    this.Dispose();
                }
            }
        }


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
        public SkinFrmLogin()
        {
            InitializeComponent();

        }

        public SkinFrmLogin(StatusGame statusGame)
        {
            InitializeComponent();

            
        }

        public SkinFrmLogin(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Virtual
        public virtual void OnActionForm(SkinFrmLogin frmSender, String userName)
        {
            if (ActionForm != null) ActionForm(frmSender, userName);
        }
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
            this.pnl_Exists = new System.Windows.Forms.Panel();
            this.btn_Login = new System.Windows.Forms.Button();
            this.txt_Username = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_Exists
            // 
            this.pnl_Exists.BackColor = System.Drawing.Color.Transparent;
            this.pnl_Exists.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.username_exists_text;
            this.pnl_Exists.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnl_Exists.Location = new System.Drawing.Point(44, 223);
            this.pnl_Exists.Name = "pnl_Exists";
            this.pnl_Exists.Size = new System.Drawing.Size(401, 28);
            this.pnl_Exists.TabIndex = 7;
            this.pnl_Exists.Visible = false;
            // 
            // btn_Login
            // 
            this.btn_Login.BackColor = System.Drawing.Color.Transparent;
            this.btn_Login.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Login.FlatAppearance.BorderSize = 0;
            this.btn_Login.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_Login.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_Login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Login.Location = new System.Drawing.Point(145, 391);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(204, 63);
            this.btn_Login.TabIndex = 6;
            this.btn_Login.TabStop = false;
            this.btn_Login.UseVisualStyleBackColor = false;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // txt_Username
            // 
            this.txt_Username.Location = new System.Drawing.Point(107, 278);
            this.txt_Username.Name = "txt_Username";
            this.txt_Username.Size = new System.Drawing.Size(283, 20);
            this.txt_Username.TabIndex = 5;
            this.txt_Username.TextChanged += new System.EventHandler(this.txt_Username_TextChanged);
            // 
            // SkinFrmLogin
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.login;
            this.Controls.Add(this.pnl_Exists);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.txt_Username);
            this.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.Name = "SkinFrmLogin";
            this.PatternBitmap = global::DCN.TicTacToe.UI.Properties.Resources.login;
            this.Size = new System.Drawing.Size(479, 524);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #endregion

        #region Event handler
        private void btn_Login_Click(object sender, EventArgs e)
        {
            OnActionForm(this, this.txt_Username.Text);
        }

        private void txt_Username_TextChanged(object sender, EventArgs e)
        {
            if (pnl_Exists.Visible)
                pnl_Exists.Visible = false;
        }

        #endregion


    }

}
