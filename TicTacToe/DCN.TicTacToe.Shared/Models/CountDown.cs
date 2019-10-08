using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DCN.TicTacToe.Shared.Models
{
    [Serializable]
    public class CountDown
    {
        public int CurrentTime { get; set; }
        public int Step { get; set; }

        private Timer timer;
        
        public CountDown()
        {
            CurrentTime = Step = 0;
            timer = new Timer();

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
        }

        public delegate int CountDownAction(int time);
        public event CountDownAction CoutDownEv;

        public virtual void OnCountDown()
        {
            if (this.CoutDownEv != null)
                CoutDownEv(this.CurrentTime);

        }
    }
}
