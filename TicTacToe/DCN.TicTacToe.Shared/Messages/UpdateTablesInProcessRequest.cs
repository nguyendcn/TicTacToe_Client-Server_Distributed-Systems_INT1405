using DCN.TicTacToe.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class UpdateTablesInProcessRequest : RequestMessageBase
    {
        public List<TablePropertiesBase> ClientsInProcess { get; set; }

        public UpdateTablesInProcessRequest()
        {
            this.ClientsInProcess = new List<TablePropertiesBase>();
        }

        public UpdateTablesInProcessRequest(List<TablePropertiesBase> list)
        {
            this.ClientsInProcess = list;
        }
    }
}
