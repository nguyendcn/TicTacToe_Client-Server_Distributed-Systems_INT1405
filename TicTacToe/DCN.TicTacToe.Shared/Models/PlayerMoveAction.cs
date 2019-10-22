using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DCN.TicTacToe.Shared.Models
{
    public class PlayerMoveAction
    {
        private Timer timer = new Timer();
        private int index = 0;

        private Point pStart;
        private Point pEnd;

        public List<Point> ListLocation { get; set; }
        

        public delegate void LocationPlayer(Point lc);

        public event LocationPlayer ChangeLocationPlayer;

        public PlayerMoveAction()
        {
            timer = new Timer();
            timer.Interval = 500;
            timer.Elapsed += Timer_Elapsed;

            ListLocation = new List<Point>();
        }

        public PlayerMoveAction(Point pStart, Point pEnd)
        {
            timer = new Timer();
            timer.Interval = 500;
            timer.Elapsed += Timer_Elapsed;
            ListLocation = GetListPointInLine(pStart, pEnd);
        }

        public void SetProperty(Point pStart, Point pEnd)
        {
            if (timer.Enabled)
                Stop();
            this.pStart = pStart;
            this.pEnd = pEnd;
            this.index = 0;

            ListLocation = GetListPointInLine(pStart, pEnd);
            Start();

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(index < ListLocation.Count)
                OnChangeLocationPlayer(ListLocation[index++]);
            else
            {
                this.timer.Stop();
            }
        }

        public List<Point> GetListPointInLine(Point pStart, Point pEnd)
        {
            List<Point> lp = new List<Point>();
            Point vectorCP = new Point(pEnd.X - pStart.X, pEnd.Y - pStart.Y);
            double step = 0.1;
            while(step <= 1.0)
            {
                lp.Add(new Point(
                    (int)Math.Round(pStart.X + (vectorCP.X * step), MidpointRounding.AwayFromZero),
                    (int)Math.Round(pStart.Y + (vectorCP.Y * step), MidpointRounding.AwayFromZero)));
                step += 0.1;
            }
            

            return lp;
        }


        public void Stop()
        {
            this.timer.Stop();
        }

        public void Start()
        {
            this.timer.Start();
        }


        public virtual void OnChangeLocationPlayer(Point lc)
        {
            if (ChangeLocationPlayer != null) ChangeLocationPlayer(lc);
        }

    }
}
