using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages.PublicPark
{
    [Serializable]
    public class AddNewPlayRequest: RequestMessageBase
    {
        public String UserName { get; set; }
        public Point Location { get; set; }

        public AddNewPlayRequest()
        {

        }
        public AddNewPlayRequest(String userName, Point location)
        {
            this.UserName = userName;
            this.Location = location;
        }
    }
}
