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
    public class SkinFrmFindPlayer : DCN.TicTacToe.UI.SkinComponents.SkinControl
    {
        private System.Windows.Forms.Panel picb_Notify;
        private System.Windows.Forms.Button btn_Back;
        private System.Windows.Forms.Button btn_Find;
        private System.Windows.Forms.TextBox txt_ut;

        public event Action<SkinFrmFindPlayer, String> ActionForm;

        private String message;

        public String Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                if (message.Equals("decline"))
                {
                    this.picb_Notify.Visible = true;
                    this.picb_Notify.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.decline_invite_text;
                }
                else if(message.Equals("notExist"))
                {
                    this.picb_Notify.Visible = true;
                    this.picb_Notify.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.find_text1;
                }
                this.ActiveControl = this.txt_ut;
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
        public SkinFrmFindPlayer()
        {
            InitializeComponent();

        }

        public SkinFrmFindPlayer(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Virtual
        public virtual void OnActionForm(SkinFrmFindPlayer frmSender, String userName)
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
            this.picb_Notify = new System.Windows.Forms.Panel();
            this.btn_Back = new System.Windows.Forms.Button();
            this.btn_Find = new System.Windows.Forms.Button();
            this.txt_ut = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // picb_Notify
            // 
            this.picb_Notify.BackColor = System.Drawing.Color.Transparent;
            this.picb_Notify.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.find_text1;
            this.picb_Notify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picb_Notify.Location = new System.Drawing.Point(27, 218);
            this.picb_Notify.Name = "picb_Notify";
            this.picb_Notify.Size = new System.Drawing.Size(430, 31);
            this.picb_Notify.TabIndex = 9;
            this.picb_Notify.Visible = false;
            // 
            // btn_Back
            // 
            this.btn_Back.BackColor = System.Drawing.Color.Transparent;
            this.btn_Back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Back.FlatAppearance.BorderSize = 0;
            this.btn_Back.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_Back.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_Back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Back.Location = new System.Drawing.Point(141, 421);
            this.btn_Back.Name = "btn_Back";
            this.btn_Back.Size = new System.Drawing.Size(204, 63);
            this.btn_Back.TabIndex = 8;
            this.btn_Back.TabStop = false;
            this.btn_Back.UseVisualStyleBackColor = false;
            this.btn_Back.Click += new System.EventHandler(this.btn_Back_Click);
            // 
            // btn_Find
            // 
            this.btn_Find.BackColor = System.Drawing.Color.Transparent;
            this.btn_Find.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Find.FlatAppearance.BorderSize = 0;
            this.btn_Find.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_Find.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_Find.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Find.Location = new System.Drawing.Point(145, 335);
            this.btn_Find.Name = "btn_Find";
            this.btn_Find.Size = new System.Drawing.Size(204, 63);
            this.btn_Find.TabIndex = 10;
            this.btn_Find.TabStop = false;
            this.btn_Find.UseVisualStyleBackColor = false;
            this.btn_Find.Click += new System.EventHandler(this.btn_Find_Click);
            // 
            // txt_ut
            // 
            this.txt_ut.Location = new System.Drawing.Point(102, 280);
            this.txt_ut.Name = "txt_ut";
            this.txt_ut.Size = new System.Drawing.Size(300, 20);
            this.txt_ut.TabIndex = 11;
            this.txt_ut.TextChanged += new System.EventHandler(this.txt_ut_TextChanged);
            // 
            // SkinFrmFindPlayer
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.find_player;
            this.Controls.Add(this.txt_ut);
            this.Controls.Add(this.btn_Find);
            this.Controls.Add(this.picb_Notify);
            this.Controls.Add(this.btn_Back);
            this.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.Name = "SkinFrmFindPlayer";
            this.PatternBitmap = global::DCN.TicTacToe.UI.Properties.Resources.find_player;
            this.Size = new System.Drawing.Size(479, 524);
            this.TransparentColor = System.Drawing.Color.Transparent;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #endregion

        #region Event handler
        private void btn_Find_Click(object sender, EventArgs e)
        {
            OnActionForm(this, txt_ut.Text);
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            OnActionForm(this, "");
            this.Dispose();
        }

        private void txt_ut_TextChanged(object sender, EventArgs e)
        {
            if (picb_Notify.BackgroundImage != null)
                picb_Notify.BackgroundImage = null;
        }
        #endregion



    }

}
