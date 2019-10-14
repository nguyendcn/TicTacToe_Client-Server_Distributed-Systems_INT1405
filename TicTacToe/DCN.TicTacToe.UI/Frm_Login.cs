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
    public partial class Frm_Login : Form
    {
        public Frm_Login()
        {
            InitializeComponent();
        }



        public bool UserIsExists{
            get { return UserIsExists; }
            set {
                this.pnl_Exists.Visible = value;
                UserIsExists = value;

                if (!UserIsExists)
                {
                    this.Close();
                }
            }
        }

        public event Action<Frm_Login, String> ActionForm;

        public virtual void OnActionForm(Frm_Login frmSender, String userName)
        {
            if (ActionForm != null) ActionForm(frmSender, userName);
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            OnActionForm(this, this.txt_Username.Text);
        }

        private void txt_Username_TextChanged(object sender, EventArgs e)
        {
            if (pnl_Exists.Visible)
                pnl_Exists.Visible = false;
        }
    }
}
