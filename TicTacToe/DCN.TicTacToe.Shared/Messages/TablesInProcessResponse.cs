using DCN.TicTacToe.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class TablesInProcessResponse : ResponseMessageBase
    {
        public TablesInProcessResponse(TablesInProcessRequest request)
            :base(request)
        {

        }

        public List<TablePropertiesBase> ClientsInProcess { get; set; }
    }
}
