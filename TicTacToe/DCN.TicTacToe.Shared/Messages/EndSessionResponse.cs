using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class EndSessionResponse : ResponseMessageBase
    {
        public EndSessionResponse(EndSessionRequest request)
            : base(request)
        {

        }
    }
}
