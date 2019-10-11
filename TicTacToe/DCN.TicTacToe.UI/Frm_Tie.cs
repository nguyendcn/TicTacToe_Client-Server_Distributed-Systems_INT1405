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
    public partial class Frm_Tie : Form
    {
        public Frm_Tie()
        {
            InitializeComponent();
        }

        public event Action<Options> ActionForm;

        public virtual void OnActionForm(Options option)
        {
            if (ActionForm != null) ActionForm(option);
        }

        private void cbtn_Continute_Click(object sender, EventArgs e)
        {
            OnActionForm(Options.Continute);
            this.Visible = true;
        }
    }
}
