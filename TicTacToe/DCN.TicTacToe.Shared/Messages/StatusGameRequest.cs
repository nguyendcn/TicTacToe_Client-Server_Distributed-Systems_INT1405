using DCN.TicTacToe.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class StatusGameRequest : RequestMessageBase
    {
        public StatusGame StatusGame { get; set; } 

        public StatusGameRequest()
        {
            StatusGame = StatusGame.Continue;
        }

        public StatusGameRequest(StatusGame status)
        {
            StatusGame = status;
        }
    }
}
