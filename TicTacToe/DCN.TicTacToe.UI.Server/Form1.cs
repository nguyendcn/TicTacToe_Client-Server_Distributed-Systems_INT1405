using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCN.TicTacToe.UI.Server
{
    public partial class Form1 : Form
    {
        private DCN.TicTacToe.Server.Server server;
        private Timer updateListTimer;

        public Form1()
        {
            server = new TicTacToe.Server.Server(9999);
            InitializeComponent();

            updateListTimer = new Timer();
            updateListTimer.Interval = 1000;
            updateListTimer.Tick += UpdateListTimer_Tick;
            updateListTimer.Start();
        }

        private void UpdateListTimer_Tick(object sender, EventArgs e)
        {
            UpdateClientsList();
        }

        private void InvokeUI(Action action)
        {
            this.Invoke(action);
        }

        private void UpdateClientsList()
        {
            InvokeUI(() =>
            {
                label1.Text = "";

                foreach (var receiver in server.Receivers)
                {
                    String[] str = new String[5];
                    str[0] = receiver.ID.ToString();
                    str[1] = receiver.Email;
                    str[2] = receiver.Status.ToString();
                    str[3] = receiver.TotalBytesUsage.ToString();

                    if (receiver.OtherSideReceiver != null)
                    {
                        str[4] = receiver.OtherSideReceiver.Email;
                    }

                    label1.Text += ("//" + str[0]); 
                }
            });
        }

        private void btn_StartServer_Click(object sender, EventArgs e)
        {
            this.server.Start();
        }
    }
}
