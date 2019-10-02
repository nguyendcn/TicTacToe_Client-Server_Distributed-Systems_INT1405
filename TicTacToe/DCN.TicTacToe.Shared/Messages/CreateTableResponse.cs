using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class CreateTableResponse : ResponseMessageBase
    {
        public CreateTableResponse(RequestMessageBase request)
            : base(request)
        {

        }

        public bool IsSuccess{ get; set; }
    }
}
