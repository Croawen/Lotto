using Lotto.DAL.TransactionRepository;
using Lotto.Data.Entities;
using Lotto.Services.AuthService.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Lotto.Common.Exceptions;

namespace Lotto.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ITransactionRepository _transactionRepository;

        public AuthService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<LoginResultModel> Login(LoginRequestModel model)
        {
            var user = await _transactionRepository.GetAll<UserEntity>(u => u.Email == model.Email).SingleOrDefaultAsync();

            if (user == null)
                throw new BadRequestException("Invalid login data.");

            var hashBytes = Convert.FromBase64String(user.PasswordHash);
            var saltBytes = Convert.FromBase64String(user.PasswordSalt);

            var passwordHash = new PasswordHash(saltBytes, hashBytes);

            if (!passwordHash.Verify(model.Password))
                throw new BadRequestException("Invalid login data.");

            return LoginThisUser(user);
        }

        public async Task Register(RegisterRequestModel model)
        {
            var user = await _transactionRepository.GetAll<UserEntity>(u => u.Email == model.Email).SingleOrDefaultAsync();

            if (user != null)
                throw new BadRequestException("Invalid login data.");

            var passwordHash = new PasswordHash(model.Password);
            var newUser = new UserEntity
            {
                Email = model.Email,
                PasswordHash = Convert.ToBase64String(passwordHash.Hash),
                PasswordSalt = Convert.ToBase64String(passwordHash.Salt)
            };

            if (_transactionRepository.Add(newUser) == 0)
                throw new TransactionRepositoryException("Failed to add new user entity.");
        }

        private LoginResultModel LoginThisUser(UserEntity user)
        {
            var userClaims = new List<Claim> {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Email", user.Email)
            };

            var tokenResultModel = TokenHelper.GenerateTokenForUser(user, userClaims);

            return new LoginResultModel
            {
                Access_token = tokenResultModel.Access_token,
                Access_token_expires_in = tokenResultModel.Access_token_expires_in,
                Refresh_token = tokenResultModel.Refresh_token,
                Refresh_token_expires_in = tokenResultModel.Refresh_token_expires_in,
                Token_type = tokenResultModel.Token_type,
                User_id = user.Id
            };
        }
    }
}
