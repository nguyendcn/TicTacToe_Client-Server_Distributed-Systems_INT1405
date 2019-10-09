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
        public int TotalTime { get; set; }

        public Timer Timer { get; set; }
        
        public CountDown()
        {
            CurrentTime = TotalTime = 15;
            Step = 1;
            Timer = new Timer();

            Timer.Interval = 1000;
            Timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CurrentTime -= Step;
            OnCountDown(this.CurrentTime);
        }

        public delegate void CountDownAction(int time);
        public event CountDownAction CoutDownEv;

        public virtual void OnCountDown(int time)
        {
            if (this.CoutDownEv != null)
                CoutDownEv(time);

        }
    }
}
