using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealworldConduit.Infrastructure.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealWorldConduit.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private const int ACCESS_TOKEN_LIFE_TIME = 5;
        private const int REFRESH_TOKEN_LIFE_TIME = ACCESS_TOKEN_LIFE_TIME * 12;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            string uniqueRandomString = StringHelper.GenerateRefreshToken();

            return new RefreshToken
            {
                UserId = user.Id,
                Token = uniqueRandomString,
                ExpiredDate = DateTime.UtcNow.AddMinutes(REFRESH_TOKEN_LIFE_TIME),
            };
        }

        public string GenerateToken(User user)
        {
            var credential = _configuration["AppCredential"];
            var key = Encoding.UTF8.GetBytes(credential);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"), 
                    // TODO Implement role later on
                }),
                Expires = DateTime.UtcNow.AddMinutes(ACCESS_TOKEN_LIFE_TIME),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword, 12);
        }

        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
