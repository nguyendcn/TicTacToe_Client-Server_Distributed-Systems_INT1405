using System;
using System.Linq;

namespace DCN.TicTacToe.Shared.Enum
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Serializable]
    public enum StatusEnum
    {
        Connected,
        Disconnected,
        Validated,
        InSession,
        InProcess
    }

}
