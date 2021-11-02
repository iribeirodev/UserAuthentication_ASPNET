using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using ShopAutenticacao.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ShopAutenticacao.Services
{
    /// <summary>
    /// Geração de token no formato JWT
    /// </summary>    
    public static class TokenService
    {
        public static string GenerateToken(User user) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}