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

        private Timer Timer;
        

        public CountDown()
        {
            CurrentTime = TotalTime = 5;
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

        #region Methods
        /// <summary>
        /// Reset countdown time for timmer.
        /// </summary>
        public void Reset()
        {
            Timer.Stop();
            CurrentTime = TotalTime;
            Start();
        }

        public void Start()
        {
            Timer.Start();
            OnCountDown(CurrentTime);
        }

        public void Stop()
        {
            Timer.Stop();
            CurrentTime = TotalTime;
        }

        #endregion
    }
}
