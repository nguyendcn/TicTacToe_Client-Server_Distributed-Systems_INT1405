using DCN.TicTacToe.Shared.Messages.PublicPark;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DCN.TicTacToe.UI.Client
{
    public partial class Frm_PublicPark : Form
    {

        private DCN.TicTacToe.Client.Client client;
        private String userName;

        public Frm_PublicPark()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public Frm_PublicPark(DCN.TicTacToe.Client.Client client)
        {
            InitializeComponent();

            this.client = client;
            client.AddNewPlayer += Client_AddNewPlayer;
            client.UpdateLocationP += Client_UpdateLocationP;
            client.JoinPPResponse += Client_JoinPPResponse;
        }

        private void Client_JoinPPResponse(JoinPublicParkResponse obj)
        {
            this.Controls.Clear();
            this.InvokeUI(() => {
                foreach (KeyValuePair<String, Point> kvp in obj.ListOtherPlayer)
                {
                    Button btn = new Button();
                    btn.Size = new Size(50, 50);
                    btn.Text = kvp.Key;
                    btn.Tag = kvp.Key;
                    btn.Location = kvp.Value;
                    this.Controls.Add(btn);
                }

                Button btnCurrentPlayer = new Button();
                btnCurrentPlayer.Size = new Size(50, 50);
                btnCurrentPlayer.Text = obj.UserNameCurrent;
                btnCurrentPlayer.Tag = obj.UserNameCurrent;
                btnCurrentPlayer.Location = obj.LocationCurrent;
                btnCurrentPlayer.LocationChanged += Btn_LocationChanged;
                this.Controls.Add(btnCurrentPlayer);

                this.userName = obj.UserNameCurrent;
            });
        }

        private void Client_UpdateLocationP(UpdateLocationPlayerRequest obj)
        {
            foreach (Control ctr in this.Controls)
            {
                if (ctr.Tag.Equals(obj.UserName))
                {
                    this.InvokeUI(()=> {
                        ctr.Location = obj.Location;
                    });
                    
                }
            }
        }

        private void Client_AddNewPlayer(AddNewPlayRequest obj)
        {
            this.InvokeUI(() =>
            {
                Button btn = new Button();
                btn.Location = obj.Location;
                btn.Size = new Size(30, 30);
                btn.Text = obj.UserName;
                btn.Tag = obj.UserName;
                this.Controls.Add(btn);
            });

        }

        private void Btn_LocationChanged(object sender, EventArgs e)
        {
            client.RequestUpdateLocation(this.userName, (sender as Button).Location);
        }

        private void InvokeUI(Action action)
        {
            this.Invoke(action);
        }

        private void Frm_PublicPark_MouseUp(object sender, MouseEventArgs e)
        {
            foreach(Control ctr in this.Controls)
            {
                if(ctr.Tag.Equals(this.userName))
                {
                    ctr.Location = e.Location;
                }
            }
        }
    }
}
