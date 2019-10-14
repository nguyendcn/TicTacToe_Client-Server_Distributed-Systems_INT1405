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
    public partial class Frm_SettingConnect : Form
    {
        public Frm_SettingConnect()
        {
            InitializeComponent();
        }

        public Frm_SettingConnect(String address, int port)
        {
            InitializeComponent();
            this.txt_Address.Text = address;
            this.txt_Port.Text = port.ToString();
        }

        public event Action<String, int> ActionForm;

        public virtual void OnActionForm(String address, int port)
        {
            if (ActionForm != null) ActionForm(address, port);
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            OnActionForm(this.txt_Address.Text, int.Parse(this.txt_Port.Text));
            this.Close();
        }
    }
}
