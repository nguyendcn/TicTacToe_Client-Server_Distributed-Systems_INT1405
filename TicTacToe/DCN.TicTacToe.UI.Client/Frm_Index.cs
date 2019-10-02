using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DCN.TicTacToe.Client;

namespace DCN.TicTacToe.UI.Client
{
    public partial class Frm_Index : Form
    {
        #region Properties for title bar
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        private DCN.TicTacToe.Client.Client client;

        public Frm_Index()
        {
            
            client = new DCN.TicTacToe.Client.Client();
            InitializeComponent();

            this.tpnl_Login.Visible = false;
            this.pnl_GamePlay.Visible = false;

            client.Connect("localhost", 9999);

            client.SessionRequest += client_SessionRequest;
            client.TextMessageReceived += Client_TextMessageReceived;
        }

        private void Client_TextMessageReceived(TicTacToe.Client.Client client, string message)
        {
            this.Text = "Message: " + message;
        }

        #region Set up for title bar
        private void pnl_titleBar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void btn_Maximise_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                return;
            }
            this.WindowState = FormWindowState.Maximized;
        }

        private void btn_Minimise_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        private void btn_ConnectToServer_Click(object sender, EventArgs e)
        {
            client.Connect("localhost", 9999);

        }

        private void Status(String str)
        {
            InvokeUI(() => { this.Text += str; });
        }

        private void InvokeUI(Action action)
        {
            this.Invoke(action);
        }

        private void pnl_ExitFormLogin_Click(object sender, EventArgs e)
        {
            this.txt_UserName.Text = "";
            this.tpnl_Login.Visible = false;

        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            client.Login(txt_UserName.Text, (senderClient, args) =>
            {

                if (args.IsValid)
                {
                    //client.Status = Shared.Enum.StatusEnum.Validated;
                    Status("User Validated!");
                    this.InvokeUI(() =>
                    {
                        this.Text = "Client - " + txt_UserName.Text;

                    });
                }

                if (args.HasError)
                {
                    Status(args.Exception.ToString());
                }

            });
            this.txt_UserName.Text = "";
            this.tpnl_Login.Visible = false;
            this.pnl_Index.Visible = false;
            this.pnl_Common.Visible = true;
            this.pnl_Common.BringToFront();
        }

        private void picb_PlayNow_Click(object sender, EventArgs e)
        {
            if (client.Status == Shared.Enum.StatusEnum.Connected)
            {
                this.tpnl_Login.Visible = true;
                this.tpnl_Login.BringToFront();
            }
            else
            {
                MessageBox.Show("You had login.");
            }
        }

        private void btn_ConnectToPlayer_Click(object sender, EventArgs e)
        {
            client.RequestSession(txt_PlayerID.Text, (senderClient, args) =>
            {

                if (args.IsConfirmed)
                {
                    Status("Session started with " + txt_PlayerID.Text);

                    InvokeUI(() =>
                    {
                        Debug.WriteLine("Session started with " + txt_PlayerID.Text);
                    });
                }
                else
                {
                    Status(args.Exception.ToString());
                }

            });
        }

        void client_SessionRequest(DCN.TicTacToe.Client.Client client, TicTacToe.Client.EventArguments.SessionRequestEventArguments args)
        {
            this.InvokeUI(() =>
            {

                if (MessageBox.Show(this, "Session request from " + args.Request.Email + ". Confirm request?", this.Text, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    args.Confirm();
                    Status("Session started with " + args.Request.Email);

                    InvokeUI(() =>
                    {

                    });
                }
                else
                {
                    args.Refuse();
                }

            });
        }

        private void button4_Click(object sender, EventArgs e)
        {

            client.SendTextMessage(client.Address);
        }

        private void btn_findOnline_Click(object sender, EventArgs e)
        {
            client.RequestClientsInProcess((client, args) =>
            {
                this.InvokeUI(() =>
                {
                    if (args.ClientsInProcess != null)
                    {
                        cmb_Online.Items.Clear();

                        args.ClientsInProcess.ForEach((clt) =>
                        {
                            cmb_Online.Items.Add(clt);
                        });
                        cmb_Online.SelectedIndex = 0;
                    }
                });

            });
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            client.RequestCreateTable(true, (client, args) => {
                if (args.IsSuccess)
                {
                    //this.Text = "Create table success!";
                    Status("Create table success!");
                }
            });
        }
    }
}
