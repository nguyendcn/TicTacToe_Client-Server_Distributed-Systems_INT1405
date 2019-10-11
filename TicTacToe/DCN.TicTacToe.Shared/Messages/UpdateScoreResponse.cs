using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCN.TicTacToe.Shared.Messages
{
    [Serializable]
    public class UpdateScoreResponse : ResponseMessageBase
    {
        public UpdateScoreResponse(UpdateScoreRequest request)
            : base(request)
        {

        }

        public UpdateScoreResponse(UpdateScoreRequest request, int socre_1, int score_2)
            : base(request)
        {
            Score_1 = socre_1;
            Score_2 = score_2;
        }

        public int Score_1 { get; set; }
        public int Score_2 { get; set; }
    }
}
