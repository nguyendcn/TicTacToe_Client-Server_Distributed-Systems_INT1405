using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages.PublicPark
{
    [Serializable]
    public class UpdateLocationPlayerRequest : RequestMessageBase
    {
        public String UserName { get; set; }
        public Point Location { get; set; }
    }
}
