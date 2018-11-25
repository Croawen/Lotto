using Lotto.Data.Entities;
using Lotto.Services.AuthService.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Lotto.Services.AuthService
{
    public class TokenHelper
    {
        public static LoginResultModel GenerateTokenForUser(UserEntity user, IList<Claim> userClaims)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Create the JWT and write it to a string
            var accessJWT = new JwtSecurityToken(
                issuer: GetIssuer(),
                audience: GetAudience(),
                claims: claims.Union(userClaims),
                notBefore: now,
                expires: now.Add(GetAccessTokenExpirationTime()),
                signingCredentials: new SigningCredentials(GetSignInKey(), SecurityAlgorithms.HmacSha256));

            var encodedAccessJWT = new JwtSecurityTokenHandler().WriteToken(accessJWT);

            var refreshJWT = new JwtSecurityToken(
               issuer: GetIssuer(),
               audience: GetAudience(),
               claims: claims,
               notBefore: now,
               expires: now.Add(GetRefreshTokenExpirationTime()),
               signingCredentials: new SigningCredentials(GetSignInKey(), SecurityAlgorithms.HmacSha256));

            var encodedRefreshJWT = new JwtSecurityTokenHandler().WriteToken(refreshJWT);

            return new LoginResultModel()
            {
                Token_type = "Bearer",
                Access_token = encodedAccessJWT,
                Access_token_expires_in = GetAccessTokenExpirationTimeSec(),
                Refresh_token = encodedRefreshJWT,
                Refresh_token_expires_in = GetRefreshTokenExpirationTimeSec()
            };
        }

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSignInKey(),
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = GetIssuer(),
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = GetAudience(),
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };
        }
        private static SymmetricSecurityKey GetSignInKey()
        {
            const string secretKey = "$up3r_H!p3r_#k$tr@Coo!3k_F4ig)&&__$u@M3k_123-456/789**";
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            return signingKey;
        }

        private static string GetIssuer() => "issuer";

        private static string GetAudience() => "audience";

        private static TimeSpan GetAccessTokenExpirationTime() => TimeSpan.FromDays(30);
        private static int GetAccessTokenExpirationTimeSec() => (int)GetAccessTokenExpirationTime().TotalSeconds;
        private static TimeSpan GetRefreshTokenExpirationTime() => TimeSpan.FromDays(30);
        private static int GetRefreshTokenExpirationTimeSec() => (int)GetRefreshTokenExpirationTime().TotalSeconds;

        public static JwtSecurityToken ReadToken(string refreshToken)
        {
            var jwt = new JwtSecurityTokenHandler().ReadToken(refreshToken) as JwtSecurityToken;
            return jwt;
        }
    }
}
