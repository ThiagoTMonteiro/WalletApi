using Microsoft.AspNetCore.Mvc;
using WalletApi.Application.DTOs;
using WalletApi.Application.Interfaces.Services;

namespace WalletApi.Api.Controllers;

[Route("v1/api/users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var result = await userService.Register(model);
        
        return !result.Succeeded ? BadRequest(result.Errors) : Ok(new { message = "Usuário Registrado com sucesso!" });
    }
    
}