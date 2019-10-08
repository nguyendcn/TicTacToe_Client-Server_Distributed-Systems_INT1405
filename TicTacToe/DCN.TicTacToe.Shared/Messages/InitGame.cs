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
        public InGameProperties properties { get; set; }
        public bool IsFirst { get; set; }
        public String userName { get; set; }
    }
}
