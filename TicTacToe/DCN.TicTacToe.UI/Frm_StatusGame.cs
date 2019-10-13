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
    public partial class Frm_StatusGame : Form
    {
        public Frm_StatusGame()
        {
            InitializeComponent();
        }

        public Frm_StatusGame(StatusGame statusGame, Point locationShow)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = locationShow;

            if(statusGame == StatusGame.Win)
            {
                this.btn_Title.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.text_win;
            }
            else if(statusGame == StatusGame.Lose)
            {
                this.btn_Title.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.text_lose;
            }
            else if(statusGame == StatusGame.Tie)
            {
                this.btn_Title.BackgroundImage = global::DCN.TicTacToe.UI.Properties.Resources.text_tie;
            }
            
        }

        public event Action<Options> ActionForm;

        public virtual void OnActionForm(Options option)
        {
            if (ActionForm != null) ActionForm(option);
        }


        private void cbtn_Continute_Click(object sender, EventArgs e)
        {
            OnActionForm(Options.Continute);
            this.Close();
        }

        private void Frm_StatusGame_Load(object sender, EventArgs e)
        {
            this.btn_Title.Enabled = false;
            this.cbtn_Continute.TabStop = false;
        }
    }
}
