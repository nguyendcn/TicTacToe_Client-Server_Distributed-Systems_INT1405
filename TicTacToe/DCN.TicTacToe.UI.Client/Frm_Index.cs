﻿using System;
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

            this.pnl_Index.BringToFront();


            client.Connect("localhost", 9999);

            client.SessionRequest += client_SessionRequest;
            client.TextMessageReceived += Client_TextMessageReceived;
            client.UpdateClientInProcess += Client_UpdateClientInProcess;
        }

        private void Client_UpdateClientInProcess(Shared.Messages.UpdateClientsInProcessRequest msg)
        {
            this.InvokeUI(() =>
            {
                flp_ShowTableGame.Controls.Clear();
                foreach (string namePlayer in msg.ClientsInProcess)
                {
                    Button btn = new Button();
                    btn.Size = new Size(100, 100);
                    btn.Text = namePlayer;
                    btn.Click += Btn_TableGame_Click;

                    flp_ShowTableGame.Controls.Add(btn);
                }
            });

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
            this.btn_findOnline.PerformClick();
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
                        flp_ShowTableGame.Controls.Clear();
                        args.ClientsInProcess.ForEach((namePlayer) =>
                        {
                            Button btn = new Button();
                            btn.Size = new Size(100, 100);
                            btn.Text = namePlayer;
                            btn.Click += Btn_TableGame_Click;

                            flp_ShowTableGame.Controls.Add(btn);
                        });
                        
                    }

                });

            });
        }

        private void Btn_TableGame_Click(object sender, EventArgs e)
        {
            Button btn_Table = sender as Button;
            string playerName = btn_Table.Text;

            client.RequestSession(playerName, (clientSend, args) =>
            {
                this.InvokeUI(() => {
                    if (args.IsConfirmed)
                    {
                        this.Text = args.Email;
                        pnl_GamePlay.BringToFront();
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
                    this.InvokeUI(() =>
                    {
                        this.pnl_GamePlay.BringToFront();
                    });
                    //redirect to table game.
                }
            });
        }

        private void cmb_Online_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb_NamePlayer = sender as ComboBox;
            string clientName = cmb_NamePlayer.SelectedItem.ToString();

            foreach(Button ctr in flp_ShowTableGame.Controls)
            {
                flp_ShowTableGame.Controls.Remove(ctr);
            }

            client.RequestSession(clientName, (clientSend, args) =>
            {
            this.InvokeUI(()=>{
                this.Text = args.Email;
            });
            });
        }
    }
}