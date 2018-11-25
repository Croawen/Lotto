using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lotto.Data.Entities
{
    public class UserRollEntity
    {
        [ForeignKey("Roll")]
        public int RollId { get; set; }
        public RollEntity Roll { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        public bool HasWon { get; set; }
    }
}
