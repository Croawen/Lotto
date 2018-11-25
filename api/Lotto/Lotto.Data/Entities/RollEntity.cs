using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lotto.Data.Entities
{
    public class RollEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTimeOffset RollDate { get; set; }

        public bool IsFinished { get; set; }

        public double? WinningLat { get; set; }

        public double? WinningLng { get; set; }

        public ICollection<UserRollEntity> Participants { get; set; }

        public int? PreviousRollId { get; set; }
    }
}
