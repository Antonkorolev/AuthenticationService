using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendService.BusinessLogic.Tasks.CreateJwtToken.Models;
using Microsoft.IdentityModel.Tokens;

namespace BackendService.BusinessLogic.Tasks.CreateJwtToken;

public sealed class CreateJwtTokenTask : ICreateJwtTokenTask
{
    public Task<string> CreateAsync(CreateJwtTokenTaskRequest request)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, request.UserId.ToString())
        };

        foreach (var roleInfo in request.RoleInfos)
        {
            foreach (var permission in roleInfo.Permissions)
            {
                claims.Add(new Claim($"{roleInfo.RoleCode}", $"{permission.PermissionCode}"));

            }
        }
        
        var jwtToken = new JwtSecurityToken(
            issuer: "AuthServer",
            audience: "AuthClient",
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Unique_Key")), SecurityAlgorithms.HmacSha256)); // TODO move key to db
            
        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwtToken));
    }
}