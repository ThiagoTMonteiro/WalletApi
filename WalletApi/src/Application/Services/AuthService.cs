
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WalletApi.Application.DTOs;
using WalletApi.Application.DTOs.Response;
using WalletApi.Application.Interfaces.Services;
using WalletApi.Domain.Entities;

namespace WalletApi.Application.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    IConfiguration configuration,
    IMapper mapper)
    : IAuthService
{
    
    public async Task<AuthResponse> Login(LoginModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password)) 
            return new AuthResponse();
        
        var userRoles = await userManager.GetRolesAsync(user);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            audience:  configuration["Jwt:Audience"],
            issuer:  configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(5),
            signingCredentials: creds
            );
        
        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        
        return new AuthResponse
        {
            Token = token,
        };
    }

}