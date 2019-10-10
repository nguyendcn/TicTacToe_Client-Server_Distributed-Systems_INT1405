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
using DCN.TicTacToe.Shared.Enum;
using DCN.TicTacToe.Shared.Models;

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

        private Timer timerReqMesgChat;
        private Timer timerReceMesgChat;

        public Frm_Index()
        {

            client = new DCN.TicTacToe.Client.Client();
            InitializeComponent();

            this.pnl_Index.BringToFront();

            this.pnl_MsgChat_1.Visible = false;
            this.pnl_MsgChat_2.Visible = false;
            this.btn_Already.Visible = false;

            client.Connect("localhost", 9999);

            client.SessionRequest += client_SessionRequest;
            client.TextMessageReceived += Client_TextMessageReceived;
            client.UpdateTablesInProcess += Client_UpdateTablesInProcess;
            client.EnablePlayRequest += Client_EnablePlayRequest;
            client.UpdateCountDown += Client_UpdateCountDown;
            client.InitGame += Client_InitGame;
            client.GameRequest += Client_GameRequest;
            client.TimeOutRequest += Client_TimeOutRequest;
            client.GameResponse += Client_GameResponse;

            this.timerReqMesgChat = new Timer();
            this.timerReqMesgChat.Interval = 3000;
            this.timerReqMesgChat.Tick += timerReqMesgChat_Tick;

            this.timerReceMesgChat = new Timer();
            this.timerReceMesgChat.Interval = 3000;
            this.timerReceMesgChat.Tick += timerReceMesgChat_Tick;

            this.pnl_GameBoard.Enabled = false;
        }

        private void Client_GameResponse(Shared.Messages.GameResponse obj)
        {
            Status(obj.Game.ToString());

            this.InvokeUI(() =>
            {
                pnl_GameBoard.Enabled = false;
                foreach (Control ctr in pnl_GameBoard.Controls)
                {
                    if(ctr is Button)
                        ctr.Enabled = true;
                }
            });
            
        }

        private void Client_TimeOutRequest(Shared.Messages.TimeOutRequest obj)
        {
            this.InvokeUI(() =>
            {
                this.Text = "timeout!";
            });
        }

        private void Client_GameRequest(Shared.Messages.GameRequest obj)
        {
            this.InvokeUI(() => {
                this.pnl_GameBoard.Enabled = true;
                ShowGameBoard_1(obj.BoardGame);
            });
        }

        private void Client_InitGame(Shared.Messages.InitGame obj)
        {
            this.InvokeUI(() => {
                this.pnl_GameBoard.Enabled = obj.IsFirst;

                this.lbl_Score_1.Text = obj.properties.WinGame.ToString();
                ShowGameBoard(new int[,] { 
                                           {-1, -1, -1}, 
                                           {-1, -1, -1}, 
                                           {-1, -1, -1}
                                         });
            });
        }

        private void Client_UpdateCountDown(Shared.Messages.UpdateCountDownRequest obj)
        {
            this.InvokeUI(() =>
            {
                this.lbl_Countdown.Text = obj.Time.ToString();
            });
        }

        private void Client_EnablePlayRequest(Shared.Messages.AcceptPlayRequest obj)
        {
            this.InvokeUI(() =>
            {
                if (obj.IsAlready)
                {
                    this.label2.Text = "Already";
                }
                else
                {
                    this.pnl_GameBoard.Enabled = false;
                    this.btn_Already.Visible = true;
                }
            });

        }

        private void timerReqMesgChat_Tick(object sender, EventArgs e)
        {
            this.pnl_MsgChat_1.Visible = false;

            this.timerReqMesgChat.Stop();
        }

        private void timerReceMesgChat_Tick(object sender, EventArgs e)
        {
            this.pnl_MsgChat_2.Visible = false;

            this.timerReceMesgChat.Stop();
        }


        private void Client_UpdateTablesInProcess(Shared.Messages.UpdateTablesInProcessRequest msg)
        {
            this.InvokeUI(() =>
            {
                flp_ShowTableGame.Controls.Clear();
                foreach (TablePropertiesBase table in msg.ClientsInProcess)
                {
                    Button btn = new Button();
                    btn.Size = new Size(100, 100);
                    btn.Text = table.Room.ToString();
                    btn.Name = table.IDUser_1;
                    btn.Click += Btn_TableGame_Click;

                    flp_ShowTableGame.Controls.Add(btn);
                }
            });

        }

        private void Client_TextMessageReceived(TicTacToe.Client.Client client, string message)
        {
            ChatMessageBox(message);
            Debug.WriteLine("Message received: " + message);
            //Status("Message received: " + message);
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
                        args.ClientsInProcess.ForEach((table) =>
                        {
                            Button btn = new Button();
                            btn.Size = new Size(100, 100);
                            btn.Text = table.Room.ToString();
                            btn.Name = table.IDUser_1.ToString();
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
            string playerName = btn_Table.Name;

            client.RequestSession(playerName, (clientSend, args) =>
            {
                this.InvokeUI(() =>
                {
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
            client.RequestCreateTable(true, -1, (client, args) =>
            {
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
                else
                {
                    Status("Table is exists");
                }
            });
        }

        private void cmb_Online_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb_NamePlayer = sender as ComboBox;
            string clientName = cmb_NamePlayer.SelectedItem.ToString();

            foreach (Button ctr in flp_ShowTableGame.Controls)
            {
                flp_ShowTableGame.Controls.Remove(ctr);
            }

            client.RequestSession(clientName, (clientSend, args) =>
            {
                this.InvokeUI(() =>
                {
                    this.Text = args.Email;
                });
            });
        }

        private void btn_SendMessage_Click(object sender, EventArgs e)
        {
            if (this.timerReqMesgChat.Enabled)
            {
                this.timerReqMesgChat.Stop();
                this.timerReqMesgChat.Start();
                this.pnl_MsgChat_1.Visible = true;
                this.txt_MessContent_1.Text = txt_Message.Text;
                client.SendTextMessage(txt_Message.Text);
            }
            else
            {
                this.timerReqMesgChat.Start();
                this.pnl_MsgChat_1.Visible = true;
                this.txt_MessContent_1.Text = txt_Message.Text;
                client.SendTextMessage(txt_Message.Text);
            }
        }

        private void ChatMessageBox(String msgs)
        {
            InvokeUI(() =>
            {
                if (this.timerReceMesgChat.Enabled)
                {
                    this.timerReceMesgChat.Stop();
                    this.timerReceMesgChat.Start();
                    this.pnl_MsgChat_2.Visible = true;
                    this.pnl_MsgChat_2.BringToFront();
                    txt_MessContent_2.Text = msgs;
                }
                else
                {
                    this.timerReceMesgChat.Start();
                    this.pnl_MsgChat_2.Visible = true;
                    this.pnl_MsgChat_2.BringToFront();
                    txt_MessContent_2.Text = msgs;
                }
            });
        }

        private void btn_Already_Click(object sender, EventArgs e)
        {
            this.label1.Text = "Already";
            client.RequestAlreadyPlayGame();
            (sender as Button).Visible = false;
        }

        private void Btn_BoardItem_Click(object sender, EventArgs e)
        {
            (sender as Button).Text = "0";
            (sender as Button).Enabled = false;
            this.pnl_GameBoard.Enabled = false;
            int[,] gameBoard = new int[3, 3];
            int i = 0;
            foreach (Control ctr in this.pnl_GameBoard.Controls)
            {
                if(ctr is Button)
                {
                    gameBoard[i / 3, i % 3] = Convert.ToInt32(ctr.Text);
                    i++;
                }
            }

            client.RequestGame(gameBoard);
        }

        private void ShowGameBoard(int[,] board)
        {
            int ind = board.Length - 1;
            if(board != null)
            {
                foreach(Control ctr in pnl_GameBoard.Controls)
                {
                    if (ctr is Button)
                    {
                        if (board[ind / 3, ind % 3] == (int)Game.SPACE)
                            ctr.Enabled = true;
                        ctr.Text = board[ind / 3, ind % 3].ToString();
                        ind--;
                    }
                }
            }
        }
        private void ShowGameBoard_1(int[,] board)
        {
            int ind = 0;
            if (board != null)
            {
                foreach (Control ctr in pnl_GameBoard.Controls)
                {
                    if (ctr is Button)
                    {
                        if (board[ind / 3, ind % 3] != (int)Game.SPACE)
                            ctr.Enabled = false;
                        ctr.Text = board[ind / 3, ind % 3].ToString();
                        ind++;
                    }
                }
            }
        }
    }
}
