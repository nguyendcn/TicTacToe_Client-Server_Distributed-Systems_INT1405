using DCN.TicTacToe.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Models
{
    [Serializable]
    public class InGameProperties
    {
        public int Room { get; set; }
        public StatusInGame Status { get; set; }
        public int WinGame { get; set; }

        public InGameProperties()
        {
            this.Room = -1;
            this.Status = StatusInGame.Decline;
            this.WinGame = 0;
        }

        public void Reset()
        {
            this.Room = -1;
            this.Status = StatusInGame.Decline;
            this.WinGame = 0;
        }
    }
}
