using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class JoinTableRequest : RequestMessageBase
    { 
        public String Email { get; set; }

        public JoinTableRequest(String e, RequestMessageBase requestBase)
        {
            this.CallbackID = requestBase.CallbackID;
            this.Exception = requestBase.Exception;
            this.HasError = requestBase.HasError;
            this.Email = e;
        }
    }
}
