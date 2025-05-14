using Microsoft.AspNetCore.Mvc;
using WalletApi.Application.DTOs;
using WalletApi.Application.Interfaces.Services;

namespace WalletApi.Api.Controllers;

[Route("v1/api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var result = await authService.Register(model);
        
        return !result.Succeeded ? BadRequest(result.Errors) : Ok(new { message = "Usuário Registrado com sucesso!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var result = await authService.Login(model);
        
        return result.Token == string.Empty ? Unauthorized(new { Message = "Credenciais invalidas" }) : Ok(result);
    }
}