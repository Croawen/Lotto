using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace Lotto.Data.Entities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public ICollection<UserRollEntity> UserRolls { get; set; }
    }
}
