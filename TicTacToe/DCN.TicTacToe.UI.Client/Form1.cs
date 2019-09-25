using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DCN.TicTacToe.Client;

namespace DCN.TicTacToe.UI.Client
{
    public partial class Form1 : Form
    {
        private DCN.TicTacToe.Client.Client client;

        public Form1()
        {
            client = new DCN.TicTacToe.Client.Client();
            InitializeComponent();
        }


        private void btn_ConnectToServer_Click(object sender, EventArgs e)
        {
            client.Connect("localhost", 9999);

        }

        private void Status(String str)
        {
            InvokeUI(() => { label1.Text = str; });
        }

        private void InvokeUI(Action action)
        {
            this.Invoke(action);
        }
    }
}
