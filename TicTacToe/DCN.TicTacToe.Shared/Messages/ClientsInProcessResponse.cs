using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class ClientsInProcessResponse : ResponseMessageBase
    {
        public ClientsInProcessResponse(ClientsInProcessRequest request)
            :base(request)
        {

        }

        public List<String> ClientsInProcess { get; set; }
    }
}
