using DCN.TicTacToe.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCN.TicTacToe.UI
{
    public partial class Frm_Quit : Form
    {
        public Frm_Quit()
        {
            InitializeComponent();
        }

        public event Action<Options> ActionForm;

        public virtual void OnActionForm(Options option)
        {
            if (ActionForm != null) ActionForm(option);
        }

        private void cbtn_Yes_Click(object sender, EventArgs e)
        {
            OnActionForm(Options.YES);
            this.Visible = false;
        }

        private void cbtn_No_Click(object sender, EventArgs e)
        {
            OnActionForm(Options.NO);
            this.Visible = false;
        }

        private void cbtn_Option_MouseHover(object sender, EventArgs e)
        {
        }
    }
}
