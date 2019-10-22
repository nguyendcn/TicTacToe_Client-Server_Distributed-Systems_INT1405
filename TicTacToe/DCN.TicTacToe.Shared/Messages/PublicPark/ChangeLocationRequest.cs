using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages.PublicPark
{
    [Serializable]
    public class ChangeLocationRequest : RequestMessageBase
    {
        public Point lcStart { get; set; }
        public Point lcEnd { get; set; }
    }
}
