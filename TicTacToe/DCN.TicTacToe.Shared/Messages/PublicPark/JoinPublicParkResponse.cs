using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages.PublicPark
{
    [Serializable]
    public class JoinPublicParkResponse : ResponseMessageBase
    {
        public Dictionary<String, Point> ListOtherPlayer { get; set; }

        public String UserNameCurrent { get; set; }
        public Point LocationCurrent { get; set; }

        public JoinPublicParkResponse(JoinPublicParkRequest request)
        : base(request)
        {
            
        }

        public JoinPublicParkResponse(JoinPublicParkRequest request,
            Dictionary<String, Point> listOtherPlayer,
            String userCR, Point lcCR) 
        : base(request)
        {
            this.ListOtherPlayer = listOtherPlayer;
            this.UserNameCurrent = userCR;
            this.LocationCurrent = lcCR;
        }
    }
}
