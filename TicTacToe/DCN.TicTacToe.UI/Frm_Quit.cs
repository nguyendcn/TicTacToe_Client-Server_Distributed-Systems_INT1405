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

        public Frm_Quit(Point locationShow)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = locationShow;
        }

        public event Action<Options> ActionForm;

        public virtual void OnActionForm(Options option)
        {
            if (ActionForm != null) ActionForm(option);
        }

        private void cbtn_Option_Click(object sender, EventArgs e)
        {
            if((sender as CircularButton).Tag.Equals("yes"))
                OnActionForm(Options.YES);
            else
                OnActionForm(Options.NO);
            this.Close();
        }
    }
}
