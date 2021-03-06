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
using DCN.TicTacToe.Shared.Enum;
using DCN.TicTacToe.Shared.Models;
using DCN.TicTacToe.UI;

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
        private String _addressServer = "localhost";
        private int _portConnect = 9999;

        public Frm_Index()
        {
            
            client = new DCN.TicTacToe.Client.Client();
            InitializeComponent();

            this.pnl_Index.BringToFront();

            this.pnl_MsgChat_1.Visible = false;
            this.pnl_MsgChat_2.Visible = false;
            this.btn_Already.Visible = false;

            ConnectToServer();
            

            client.SessionRequest += client_SessionRequest;
            client.SessionEndedByTheRemoteClient += Client_SessionEndedByTheRemoteClient;
            client.TextMessageReceived += Client_TextMessageReceived;
            client.UpdateTablesInProcess += Client_UpdateTablesInProcess;
            client.EnablePlayRequest += Client_EnablePlayRequest;
            client.UpdateCountDown += Client_UpdateCountDown;
            client.InitGame += Client_InitGame;
            client.GameRequest += Client_GameRequest;
            client.TimeOutRequest += Client_TimeOutRequest;
            client.GameResponse += Client_GameResponse;
            client.AutoAccepInvite += Client_AutoAccepInvite;

            this.timerReqMesgChat = new Timer();
            this.timerReqMesgChat.Interval = 3000;
            this.timerReqMesgChat.Tick += timerReqMesgChat_Tick;

            this.timerReceMesgChat = new Timer();
            this.timerReceMesgChat.Interval = 3000;
            this.timerReceMesgChat.Tick += timerReceMesgChat_Tick;

            this.pnl_GameBoard.Enabled = false;


            InitShowForStart();
        }


        private void Client_AutoAccepInvite(TicTacToe.Client.Client obj)
        {
            this.InvokeUI(() => {
                this.pnl_PlayerArea_2.Visible = true;
                this.pnl_PlayerArea_2.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.avatar_player_2;
            });
            
        }

        private void InitShowForStart()
        {
            this.pnl_Common.Visible = false;
            this.pnl_GamePlay.Visible = false;
            this.btn_Previous.Visible = false;
            this.pnl_PlayerArea_2.Visible = false;
        }

        private void Client_SessionEndedByTheRemoteClient(TicTacToe.Client.Client obj)
        {
            this.InvokeUI(() => {
                if (this.pnl_GamePlay.Visible)
                {
                    SkinOtherQuit otherQuit = new SkinOtherQuit();
                    otherQuit.ActionForm += (option) => {
                        if (option == Options.YES)
                        {
                            this.pnl_Notify.SendToBack();
                        }
                    };
                    this.pnl_Notify.Controls.Add(otherQuit);
                    otherQuit.BringToFront();
                    this.pnl_Notify.BringToFront();

                    this.pnl_PlayerArea_2.Visible = false;
                    this.btn_Player_1_Notify.Visible = this.btn_Player_2_Notify.Visible = false;
                    this.btn_Already.Visible = false;
                }
            });

        }

        private void Client_GameResponse(Shared.Messages.GameResponse obj)
        {
            Status(obj.Game.ToString());

            this.InvokeUI(() =>
            {
                SkinNotifyGame skinNotifyGame = new SkinNotifyGame(obj.Game);
                pnl_Notify.Controls.Add(skinNotifyGame);
                skinNotifyGame.BringToFront();
                pnl_Notify.BringToFront();
                skinNotifyGame.ActionForm += SkinNotifyGame_ActionForm;
                skinNotifyGame.Location = new Point(0, 0);


                foreach (Control ctr in pnl_GameBoard.Controls)
                {
                    if(ctr is Button)
                        ctr.Enabled = true;
                }
                this.btn_Player_2_Notify.Visible = this.btn_Player_1_Notify.Visible = false;
                this.btn_Player_2_Notify.BackgroundImage = this.btn_Player_1_Notify.BackgroundImage = null;
            });

            UpdateScoreRequest();

        }

        

        private void UpdateScoreRequest()
        {
            client.RequestUpdateScore((cl, args) =>
            {
                this.InvokeUI(() =>
                {
                    this.lbl_Score_1.Text = args.Score_1.ToString();
                    this.lbl_Score_2.Text = args.Score_2.ToString();
                });
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
                this.btn_Player_2_Notify.Visible = false;
                this.btn_Player_2_Notify.BackgroundImage = null;

                this.btn_Player_1_Notify.Visible = true;
                this.btn_Player_1_Notify.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.in_turn;
                ShowGameBoard_1(obj.BoardGame);
            });
        }

        private void Client_InitGame(Shared.Messages.InitGame obj)
        {
            this.InvokeUI(() => {
                this.pnl_GameBoard.Enabled = obj.IsFirst;

                this.btn_Player_1_Notify.Visible = this.btn_Player_2_Notify.Visible = false;
                this.btn_Player_1_Notify.BackgroundImage = this.btn_Player_2_Notify.BackgroundImage = null;
                if (obj.IsFirst)
                {
                    btn_Player_1_Notify.Visible = true;
                    btn_Player_1_Notify.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.in_turn;
                }
                else
                {
                    btn_Player_2_Notify.Visible = true;
                    btn_Player_2_Notify.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.in_turn;
                }

                //this.lbl_Score_1.Text = obj.Properties.WinGame.ToString();
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
                this.btn_Countdown.Text = obj.Time.ToString();
            });
        }

        private void Client_EnablePlayRequest(Shared.Messages.AcceptPlayRequest obj)
        {
            this.InvokeUI(() =>
            {
                if (obj.IsAlready)
                {
                    this.btn_Player_2_Notify.Visible = true;
                    this.btn_Player_2_Notify.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.ok_text;
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
                    btn.Size = new Size(150, 150);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseDownBackColor = btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    btn.Image = global::DCN.TicTacToe.UI.Client.Client_Resx.table150x150;
                    btn.Text = table.Room.ToString();
                    btn.TextAlign = ContentAlignment.BottomCenter;
                    btn.Name = table.IDUser_1;
                    btn.Tag = table.IDUser_1;
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
            SkinFrmQuit quit = new SkinFrmQuit();
            quit.ActionForm += Quit_ActionForm;
            pnl_Notify.Controls.Add(quit);
            quit.BringToFront();
            pnl_Notify.BringToFront();
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
            //client.Connect("localhost", 9999);
            //client.Connect("104.215.154.47", 9999);
        }

        private void Status(String str)
        {
            InvokeUI(() => { this.Text += str; });
        }

        private void InvokeUI(Action action)
        {
            this.Invoke(action);
        }

        private void btn_ConnectToPlayer_Click(object sender, EventArgs e)
        {
            SkinFrmFindPlayer findPlayer = new SkinFrmFindPlayer();
            findPlayer.ActionForm += FindPlayer_ActionForm;
            this.pnl_Notify.Controls.Add(findPlayer);
            findPlayer.BringToFront();
            this.pnl_Notify.BringToFront();

            
        }
        
        private void client_SessionRequest(DCN.TicTacToe.Client.Client client, TicTacToe.Client.EventArguments.SessionRequestEventArguments args)
        {
            this.InvokeUI(() =>
            {
                SkinRequestSession frmRequestSession = new SkinRequestSession();
                this.pnl_Notify.Controls.Add(frmRequestSession);
                frmRequestSession.BringToFront();
                this.pnl_Notify.BringToFront();
                frmRequestSession.ActionForm += (option) => {
                    if(option == Options.YES)
                    {
                        //TODO: Send to table game
                        this.pnl_GamePlay.Visible = true;
                        this.pnl_GamePlay.BringToFront();
                        this.btn_Already.Visible = true;
                        this.pnl_PlayerArea_2.Visible = true;
                        Debug.WriteLine("Yes");

                        args.Confirm();
                    }
                    else
                    {
                        Debug.WriteLine("No");
                        args.Refuse();
                    }
                    this.pnl_Notify.SendToBack();
                };

                //wait client choose action
                //while (!frmRequestSession.IsAction) { System.Threading.Thread.Sleep(100); }
            });
        }

        private void Btn_Previous_Click(object sender, EventArgs e)
        {
            if (this.pnl_GamePlay.Visible)
            {
                SkinFrmQuit quit = new SkinFrmQuit();
                quit.ActionForm += Quit_ActionForm_ForPrevious;
                this.pnl_Notify.Controls.Add(quit);
                quit.BringToFront();
                this.pnl_Notify.BringToFront();
            }
            else if(this.pnl_Common.Visible)
            {
                this.pnl_Index.Visible = true;
                this.pnl_Index.BringToFront();
                (sender as Button).Visible = false;
            }
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
                            btn.Size = new Size(150, 150);
                            btn.FlatStyle = FlatStyle.Flat;
                            btn.FlatAppearance.BorderSize = 0;
                            btn.FlatAppearance.MouseDownBackColor = btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
                            btn.Image = global::DCN.TicTacToe.UI.Client.Client_Resx.table150x150;
                            btn.Text = table.Room.ToString();
                            btn.TextAlign = ContentAlignment.BottomCenter;
                            btn.Name = table.IDUser_1;
                            btn.Tag = table.IDUser_1;
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
                        pnl_GamePlay.Visible = true;
                        pnl_GamePlay.BringToFront();

                        this.pnl_PlayerArea_2.Visible = true;
                        this.pnl_PlayerArea_2.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.avatar_player_2;
                    }
                });

            });
        }

        private void btn_Register_Click(object sender, EventArgs e)
        {
            client.RequestCreateTable(true, -1, (client, response) =>
            {
                if (response.IsSuccess)
                {
                    //this.Text = "Create table success!";
                    Status("Create table success!");
                    this.InvokeUI(() =>
                    {
                        this.pnl_GamePlay.Visible = true;
                        this.pnl_GamePlay.BringToFront();
                        this.btn_Already.Visible = false;
                        this.pnl_PlayerArea_1.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.avatar_player_1;
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
                ShowChatQueue(this.pnl_HisChat.Controls, txt_Message.Text);
                client.SendTextMessage(txt_Message.Text);
            }
            else
            {
                this.timerReqMesgChat.Start();
                this.pnl_MsgChat_1.Visible = true;
                this.txt_MessContent_1.Text = txt_Message.Text;
                ShowChatQueue(this.pnl_HisChat.Controls, txt_Message.Text);
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
                    ShowChatQueue(this.pnl_HisChat.Controls, msgs);
                    txt_MessContent_2.Text = msgs;
                }
                else
                {
                    this.timerReceMesgChat.Start();
                    this.pnl_MsgChat_2.Visible = true;
                    this.pnl_MsgChat_2.BringToFront();
                    ShowChatQueue(this.pnl_HisChat.Controls, msgs);
                    txt_MessContent_2.Text = msgs;
                }
            });
        }

        private void btn_Already_Click(object sender, EventArgs e)
        {
            this.btn_Player_1_Notify.Visible = true;
            this.btn_Player_1_Notify.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.ok_text;
            client.RequestAlreadyPlayGame();
            (sender as Button).Visible = false;
        }

        private void Btn_BoardItem_Click(object sender, EventArgs e)
        {

            (sender as Button).Tag = (int)Game.O_VAL;
            (sender as Button).BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.O_value;
            (sender as Button).Enabled = false;
            this.pnl_GameBoard.Enabled = false;
            int[,] gameBoard = new int[3, 3];
            int i = 0;
            foreach (Control ctr in this.pnl_GameBoard.Controls)
            {
                if(ctr is Button)
                {
                    gameBoard[i / 3, i % 3] = Convert.ToInt32(ctr.Tag);
                    i++;
                }
            }

            this.btn_Player_1_Notify.Visible = false;
            this.btn_Player_1_Notify.BackgroundImage = null;
            this.btn_Player_2_Notify.Visible = true;
            this.btn_Player_2_Notify.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.in_turn;

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
                        else
                            ctr.Enabled = false;

                        SetUpImageBoard(ctr, (Game)board[ind / 3, ind % 3]);

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
                        else
                            ctr.Enabled = true;
                        SetUpImageBoard(ctr, (Game)board[ind / 3, ind % 3]);
                        ind++;
                    }
                }
            }
        }

        private void SetUpImageBoard(Control control, Game game)
        {
            control.Tag = (int)game;
            if (game== Game.X_VAL)
            {
                control.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.X_value;
            }
            else if(game == Game.O_VAL)
            {
                control.BackgroundImage = global::DCN.TicTacToe.UI.Client.Client_Resx.O_value;
            }
            else if(game == Game.SPACE)
            {
                control.BackgroundImage = null;
            }
        }

        private void pnl_Common_VisibleChanged(object sender, EventArgs e)
        {
            if(pnl_Common.Visible)
            {
                this.btn_findOnline.PerformClick();
            }
        }

        private void Login(int type)
        {
            SkinFrmLogin login = new SkinFrmLogin();

            this.pnl_Notify.Controls.Add(login);
            login.BringToFront();
            this.pnl_Notify.BringToFront();
            if(type == 1)
                login.ActionForm += Login_ActionForm; 
            else if(type == 2)
                login.ActionForm += Login_ActionForm1;
        }   

        private void ConnectToServer()
        {
            try
            {
                client.Connect(_addressServer, _portConnect);
            }
            catch (Exception ex)
            {
                SkinConnectError connectError = new SkinConnectError();
                connectError.ActionForm += ConnectError_ActionForm;
                this.pnl_Notify.Controls.Add(connectError);
                connectError.BringToFront();
                this.pnl_Notify.BringToFront();
            }
        }

        private void ShowChatQueue(Control.ControlCollection queue , String newMess)
        {
            Control temp = new Control();
            int count = 0;
            foreach(Control ctr in queue)
            {
                if(count == 0)
                {
                    temp = ctr;
                }
                else
                {
                    temp.Text = ctr.Text;
                    temp = ctr;
                }
                count++;
            }
            temp.Text = newMess;
        }


        #region Event handler

        #region Form index

        private void btn_SettingConnect_Click(object sender, EventArgs e)
        {
            SkinSettingConnect skinSettingConnect = new SkinSettingConnect(_addressServer, _portConnect);
            skinSettingConnect.ActionForm += SkinSettingConnect_ActionForm;
            this.pnl_Notify.Controls.Add(skinSettingConnect);
            skinSettingConnect.BringToFront();
            this.pnl_Notify.BringToFront();
        }

        private void picb_PlayNow_Click(object sender, EventArgs e)
        {
            if (client.Status == Shared.Enum.StatusEnum.Connected)
            {
                Login(1);
            }
            else
            {
                //MessageBox.Show("You had login.");
                this.pnl_Index.Visible = false;
                this.pnl_Common.Visible = true;
                this.pnl_Common.BringToFront();
                this.btn_Previous.Visible = true;
            }
        }

        private void picb_PublicPark_Click(object sender, EventArgs e)
        {
            if (client.Status == Shared.Enum.StatusEnum.Connected)
            {
                Login(2);
            }
            else
            {
                //MessageBox.Show("You had login.");
                Frm_PublicPark publicPark = new Frm_PublicPark(this.client);
                publicPark.Show();
            }

        }

        private void picb_Exit_Click(object sender, EventArgs e)
        {
            this.btn_Exit.PerformClick();
        }

        private void picb_About_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Action for notify

        private void SkinNotifyGame_ActionForm(Options obj)
        {
            this.InvokeUI(() =>
            {
                this.pnl_Notify.SendToBack();
            });
        }

        private void Quit_ActionForm(Options obj)
        {
            if (obj == Options.YES)
            {
                if (this.pnl_GamePlay.Visible)
                    client.EndCurrentSession((senderClient, response) => {
                        this.InvokeUI(() => {
                            client.Disconnect();
                            this.Close();
                        });
                    });
                else
                {
                    if (client.TcpClient.Connected)
                        client.Disconnect();
                    this.Close();
                }

            }
            else if (obj == Options.NO)
            {
                this.pnl_Notify.SendToBack();
            }

        }

        private void FindPlayer_ActionForm(SkinFrmFindPlayer frmFind, string data)
        {
            if (data.Equals(""))
            {
                this.pnl_Notify.SendToBack();
                return;
            }

            foreach (Control ctr in flp_ShowTableGame.Controls)
            {
                if (ctr is Button)
                {
                    if (ctr.Text.Equals(data) || ctr.Tag.Equals(data))
                    {
                        (ctr as Button).PerformClick();
                        frmFind.Dispose();
                        this.pnl_Notify.SendToBack();
                        return;
                    }
                }
            }

            client.RequestSession(data, (senderClient, args) =>
            {

                if (args.IsConfirmed)
                {
                    Status("Session started with " + data);

                    InvokeUI(() =>
                    {
                        Debug.WriteLine("Session started with " + data);
                        //TODO: Create table and come to the game.
                        frmFind.Dispose();
                        this.pnl_Notify.SendToBack();

                        this.pnl_GamePlay.Visible = true;
                        this.pnl_GamePlay.BringToFront();
                        this.btn_Already.Visible = true;
                        this.pnl_PlayerArea_2.Visible = true;

                    });
                }
                else
                {
                    this.InvokeUI(() => {
                        if (args.Exception.Message.Contains("exist"))
                        {
                            frmFind.Message = "notExist";
                        }
                        else
                        {
                            frmFind.Message = "decline";
                        }
                        Debug.WriteLine(args.Exception);
                    });
                }

            });
        }

        private void Quit_ActionForm_ForPrevious(Options obj)
        {
            if (obj == Options.YES)
            {
                if (this.pnl_GamePlay.Visible)
                {
                    client.EndCurrentSession((senderClient, response) => {
                        this.InvokeUI(() =>
                        {
                            if (!response.HasError)
                            {
                                this.pnl_Notify.SendToBack();
                                this.pnl_GamePlay.Visible = false;
                                this.pnl_Common.Visible = true;
                                this.pnl_Common.BringToFront();
                            }
                            else
                            {
                                Debug.WriteLine(response.Exception);
                            }
                        });
                    });
                }

            }
            else
            {
                this.pnl_Notify.SendToBack();
            }
        }

        private void SkinSettingConnect_ActionForm(string address, int port)
        {
            try
            {
                if (client.TcpClient.Connected)
                    client.Disconnect();
                this._addressServer = address;
                this._portConnect = port;
                client.Connect(address, port);
                this.pnl_Notify.SendToBack();
            }
            catch (Exception ex)
            {
                SkinConnectError connectError = new SkinConnectError();
                connectError.ActionForm += ConnectError_ActionForm;
                this.pnl_Notify.Controls.Add(connectError);
                connectError.BringToFront();
                this.pnl_Notify.BringToFront();
            }
        }

        private void ConnectError_ActionForm(Options obj)
        {
            if (obj == Options.YES)
            {
                this.pnl_Notify.SendToBack();
            }
        }

        private void Login_ActionForm1(SkinFrmLogin frmSender, string userName)
        {
            client.Login(userName, (senderClient, args) => {
                if (args.IsValid)
                {
                    this.InvokeUI(() => {
                        frmSender.UserIsExists = false;
                        this.pnl_Notify.SendToBack();

                        Frm_PublicPark publicPark = new Frm_PublicPark(this.client);
                        publicPark.Show();
                    });

                }
                if (args.HasError)
                {
                    this.InvokeUI(() =>
                    {
                        frmSender.UserIsExists = true;
                    });

                }
            });
        }

        private void Login_ActionForm(SkinFrmLogin frmSender, string userName)
        {
            client.Login(userName, (senderClient, args) => {
                if (args.IsValid)
                {
                    this.InvokeUI(() => {
                        frmSender.UserIsExists = false;
                        this.pnl_Notify.SendToBack();
                        this.pnl_Index.Visible = false;
                        this.pnl_Common.Visible = true;
                        this.pnl_Common.BringToFront();
                        this.btn_Previous.Visible = true;
                        //this.btn_findOnline.PerformClick();

                    });

                }
                if (args.HasError)
                {
                    this.InvokeUI(() =>
                    {
                        frmSender.UserIsExists = true;
                    });

                }
            });
        }

        #endregion

        #region Form Common

        #endregion
        #endregion
    }
}
