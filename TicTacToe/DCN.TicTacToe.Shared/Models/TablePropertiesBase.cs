using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Models
{
    [Serializable]
    public class TablePropertiesBase
    {
        public int Room { get; set; }
        public String IDUser_1 { get; set; }
        public String IDUser_2 { get; set; }

        public TablePropertiesBase(int room, String userName1, String userName2)
        {
            this.Room = room;
            this.IDUser_1 = userName1;
            this.IDUser_2 = userName2;
        }
    }
}
