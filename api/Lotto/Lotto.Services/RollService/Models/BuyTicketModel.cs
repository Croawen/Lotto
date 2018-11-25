using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Services.RollService.Models
{
    public class BuyTicketModel
    {
        public int RollId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
