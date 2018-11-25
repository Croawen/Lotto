using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Services.RollService.Models
{
    public class GetNextRollResponseModel
    {
        public int RollId { get; set; }
        public long RollDate { get; set; }
        public int ParticipantsCount { get; set; }
    }
}
