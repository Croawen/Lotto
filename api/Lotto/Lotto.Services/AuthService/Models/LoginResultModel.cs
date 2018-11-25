using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Services.AuthService.Models
{
    public class LoginResultModel
    {
        public string Token_type;
        public string Access_token;
        public int Access_token_expires_in;
        public string Refresh_token;
        public int Refresh_token_expires_in;
        public int User_id;
    }
}
