using DCN.TicTacToe.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class InitGame : MessageBase
    {
        public InGameProperties Properties { get; set; }
        public bool IsFirst { get; set; }
        public String UserName { get; set; }

        public InitGame()
        {
            Properties = new InGameProperties();
            IsFirst = false;
            UserName = "";
        }

        public InitGame(InGameProperties inGameProperties, bool isFirst, String userName)
        {
            Properties = inGameProperties;
            IsFirst = isFirst;
            UserName = userName;
        }
    }
}
