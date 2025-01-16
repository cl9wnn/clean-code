using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence.Entities;

namespace BusinessLogic.Services;

public class JwtService(IOptions<AuthSettings> options)
{
    public string GenerateJwtToken(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim("email", account.Email!),
            new Claim("firstname", account.FirstName!),
            new Claim("id", account.AccountId.ToString())
        };
        
        var jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(options.Value.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                options.Value.SecretKey!)), SecurityAlgorithms.HmacSha256)
        );
        
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}