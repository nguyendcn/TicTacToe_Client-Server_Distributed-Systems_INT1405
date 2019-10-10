using DCN.TicTacToe.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class GameResponse : ResponseMessageBase
    {
        public GameResponse(GameRequest request)
            :base(request)
        {
               
        }

        public GameResponse(GameRequest request, StatusGame gameStatus)
            : base(request)
        {
            Game = gameStatus;
        }

        public StatusGame Game { get; set; }
    }
}
