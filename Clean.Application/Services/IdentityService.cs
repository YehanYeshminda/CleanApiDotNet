using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clean.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Clean.Application.Services;

public class IdentityService
{
    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _key;

    public IdentityService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
    }

    public JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

    public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
    {
        var tokenDescriptor = GetTokenDescriptor(identity);
        return TokenHandler.CreateToken(tokenDescriptor);
    }

    public string WriteToken(SecurityToken token)
    {
        return TokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claimsIdentity)
    {
        return new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.Now.AddHours(9),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _jwtSettings.Audience[0],
            Issuer = _jwtSettings.Issuer
        };
    }
}