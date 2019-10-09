using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Enum
{   
    [Serializable]
    public enum StatusGame
    {
        Win,
        Lost,
        Tie,
        Continue
    }
}
