using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Client.EventArguments
{
    public class FileUploadProgressEventArguments
    {
        public String DestinationPath { get; set; }
        public String FileName { get; set; }
        public int CurrentPosition { get; set; }
        public long TotalBytes { get; set; }
    }
}
