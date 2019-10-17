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
    public class SkinNotifyGame : DCN.TicTacToe.UI.SkinControl.SkinControl
    {
        private CircularButton cbtn_Continute;
        private System.Windows.Forms.Button btn_Title;
        private System.Windows.Forms.Button btn_Star;
        public event Action<Options> ActionForm;

       
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
        public SkinNotifyGame()
        {
            InitializeComponent();

        }

        public SkinNotifyGame(StatusGame statusGame)
        {
            InitializeComponent();

            if (statusGame == StatusGame.Win)
            {
                this.btn_Title.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.text_win;
                this.btn_Star.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.star_win;
            }
            else if (statusGame == StatusGame.Lose)
            {
                this.btn_Title.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.text_lose;
                this.btn_Star.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.star_lose;
            }
            else if (statusGame == StatusGame.Tie)
            {
                this.btn_Title.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.text_tie;
                this.btn_Star.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.star_tie;
            }
        }

        public SkinNotifyGame(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Virtual
        public virtual void OnActionForm(Options option)
        {
            if (ActionForm != null) ActionForm(option);
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
            this.cbtn_Continute = new DCN.TicTacToe.UI.CircularButton();
            this.btn_Title = new System.Windows.Forms.Button();
            this.btn_Star = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // cbtn_Continute
            // 
            this.cbtn_Continute.BackColor = System.Drawing.Color.Transparent;
            this.cbtn_Continute.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.Play_btn;
            this.cbtn_Continute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbtn_Continute.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbtn_Continute.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cbtn_Continute.FlatAppearance.BorderSize = 0;
            this.cbtn_Continute.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Continute.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Continute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtn_Continute.Location = new System.Drawing.Point(193, 392);
            this.cbtn_Continute.Name = "cbtn_Continute";
            this.cbtn_Continute.Size = new System.Drawing.Size(104, 96);
            this.cbtn_Continute.TabIndex = 1000001;
            this.cbtn_Continute.UseVisualStyleBackColor = false;
            this.cbtn_Continute.Click += new System.EventHandler(this.cbtn_Continute_Click);
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
            this.btn_Title.Location = new System.Drawing.Point(155, 6);
            this.btn_Title.Name = "btn_Title";
            this.btn_Title.Size = new System.Drawing.Size(173, 48);
            this.btn_Title.TabIndex = 1000000;
            this.btn_Title.UseVisualStyleBackColor = false;
            // 
            // btn_Star
            // 
            this.btn_Star.BackColor = System.Drawing.Color.Transparent;
            this.btn_Star.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.star_tie;
            this.btn_Star.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Star.FlatAppearance.BorderSize = 0;
            this.btn_Star.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_Star.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_Star.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Star.Location = new System.Drawing.Point(89, 109);
            this.btn_Star.Name = "btn_Star";
            this.btn_Star.Size = new System.Drawing.Size(320, 145);
            this.btn_Star.TabIndex = 1000002;
            this.btn_Star.UseVisualStyleBackColor = false;
            // 
            // SkinNotifyGame
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.Table;
            this.Controls.Add(this.btn_Star);
            this.Controls.Add(this.cbtn_Continute);
            this.Controls.Add(this.btn_Title);
            this.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.Name = "SkinNotifyGame";
            this.PatternBitmap = global::DCN.TicTacToe.UI.Properties.Resources.Table;
            this.Size = new System.Drawing.Size(479, 524);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        #region Event handler
        private void cbtn_Continute_Click(object sender, EventArgs e)
        {
            if (this.ActionForm != null) OnActionForm(Options.YES);
            this.Dispose();
        }
        #endregion
    }

}
