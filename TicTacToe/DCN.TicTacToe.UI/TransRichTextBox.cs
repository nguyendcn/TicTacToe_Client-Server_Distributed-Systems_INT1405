using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCN.TicTacToe.UI
{
    public partial class TransRichTextBox : RichTextBox
    {
        public TransRichTextBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            this.TextChanged += TransRichTextBox_TextChanged;
            this.VScroll += TransRichTextBox_TextChanged;
            this.HScroll += TransRichTextBox_TextChanged;
        }

        public TransRichTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        void TransRichTextBox_TextChanged(object sender, System.EventArgs e)
        {
            this.ForceRefresh();

            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            this.TextChanged += TransRichTextBox_TextChanged;
            this.VScroll += TransRichTextBox_TextChanged;
            this.HScroll += TransRichTextBox_TextChanged;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parms = base.CreateParams;
                parms.ExStyle |= 0x20;  // Turn on WS_EX_TRANSPARENT
                return parms;
            }
        }
        public void ForceRefresh()
        {
            this.UpdateStyles();
        }
    }
}
