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

        public StatusGame Game { get; set; }
    }
}
