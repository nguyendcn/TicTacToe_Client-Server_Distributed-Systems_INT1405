﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages.PublicPark
{
    [Serializable]
    public class SendMessagePublicPark:MessageBase
    {
        public String UserName { get; set; }
        public String Message { get; set; }
    }
}
