using Lotto.Services.AuthService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lotto.Services.AuthService
{
    public interface IAuthService
    {
        Task<LoginResultModel> Login(LoginRequestModel model);
        Task Register(RegisterRequestModel model);
    }
}
