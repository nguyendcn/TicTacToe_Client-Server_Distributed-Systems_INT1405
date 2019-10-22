using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCN.TicTacToe.UI
{
    public class TextBoxAutoDisposeByTime : TextBox
    {
        private Timer timer;

        public TextBoxAutoDisposeByTime() : base()
        {
            timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
