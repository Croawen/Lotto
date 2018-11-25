using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Services.AuthService.Models
{
    public class RegisterRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
