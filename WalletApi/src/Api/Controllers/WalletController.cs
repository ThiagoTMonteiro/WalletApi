using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApi.Application.DTOs;
using WalletApi.Application.Interfaces.Services;


namespace WalletApi.Api.Controllers;

[Route("v1/api/wallet")]
[Authorize]
public class WalletController(IWalletService walletService) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> GetBalance()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await walletService.GetBalanceAsync(userId);

        return result == null ? NotFound() : Ok(result);
    }
    
    [HttpPost("deposit")]
    public async Task<IActionResult> AddBalance([FromBody] WalletModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result =  await walletService.AddBalanceAsync(userId, model);
        return result == null ? NotFound() : Ok(result);
    }
}