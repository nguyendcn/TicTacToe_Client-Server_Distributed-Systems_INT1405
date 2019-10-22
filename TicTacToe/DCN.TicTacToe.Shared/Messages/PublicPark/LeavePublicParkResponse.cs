using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages.PublicPark
{
    [Serializable]
    public class LeavePublicParkResponse : ResponseMessageBase 
    {
        public LeavePublicParkResponse(LeavePublicParkRequest request) 
            : base(request)
        {

        }
    }
}
