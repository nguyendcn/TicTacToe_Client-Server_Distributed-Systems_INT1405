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
    public class SkinRequestSession : DCN.TicTacToe.UI.SkinComponents.SkinControl
    {

        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        ///
        ///
        ///
        private CircularButton cbtn_No;
        ///
        ///
        ///
        private CircularButton cbtn_Yes;
        ///
        ///
        ///
        public event Action<Options> ActionForm;
        ///
        ///
        ///
        public bool IsAction { get; set; }


        #endregion

        #region Constructors

        /// <summary>
        /// the class constructor.
        /// Setts the animation timers and turns on the message handlers.		
        /// </summary>
        public SkinRequestSession()
        {
            InitializeComponent();

        }

        public SkinRequestSession(IContainer container)
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
            this.cbtn_No = new DCN.TicTacToe.UI.CircularButton();
            this.cbtn_Yes = new DCN.TicTacToe.UI.CircularButton();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // cbtn_No
            // 
            this.cbtn_No.BackColor = System.Drawing.Color.Transparent;
            this.cbtn_No.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbtn_No.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cbtn_No.FlatAppearance.BorderSize = 0;
            this.cbtn_No.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbtn_No.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbtn_No.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtn_No.Location = new System.Drawing.Point(93, 135);
            this.cbtn_No.Name = "cbtn_No";
            this.cbtn_No.Size = new System.Drawing.Size(66, 68);
            this.cbtn_No.TabIndex = 3;
            this.cbtn_No.TabStop = false;
            this.cbtn_No.Tag = "no";
            this.cbtn_No.UseVisualStyleBackColor = false;
            this.cbtn_No.Click += new System.EventHandler(this.cbtn_Option_Click);
            // 
            // cbtn_Yes
            // 
            this.cbtn_Yes.BackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbtn_Yes.FlatAppearance.BorderSize = 0;
            this.cbtn_Yes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbtn_Yes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbtn_Yes.Location = new System.Drawing.Point(199, 132);
            this.cbtn_Yes.Name = "cbtn_Yes";
            this.cbtn_Yes.Size = new System.Drawing.Size(66, 68);
            this.cbtn_Yes.TabIndex = 2;
            this.cbtn_Yes.TabStop = false;
            this.cbtn_Yes.Tag = "yes";
            this.cbtn_Yes.UseVisualStyleBackColor = false;
            this.cbtn_Yes.Click += new System.EventHandler(this.cbtn_Option_Click);
            // 
            // SkinRequestSession
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.request_session;
            this.Controls.Add(this.cbtn_No);
            this.Controls.Add(this.cbtn_Yes);
            this.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.Name = "SkinRequestSession";
            this.PatternBitmap = global::DCN.TicTacToe.UI.Properties.Resources.request_session;
            this.Size = new System.Drawing.Size(359, 204);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        #region Event handler

        #endregion

        private void cbtn_Option_Click(object sender, EventArgs e)
        {
            IsAction = true;
            if ((sender as CircularButton).Tag.Equals("yes"))
                OnActionForm(Options.YES);
            else
                OnActionForm(Options.NO);
            this.Dispose();
        }
    }

}
