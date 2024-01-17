using Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NotiGest.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JwtService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }

        public dynamic GenerateToken(string username, IList<string> rol)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

            // Agrega los roles como claims
            claims.AddRange(rol.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Audience= _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var encrypter = tokenHandler.WriteToken(token);

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("token", encrypter, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddHours(1),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });

            return new { token = encrypter } ;
        }
    }
}
