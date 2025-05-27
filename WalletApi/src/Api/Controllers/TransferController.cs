using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApi.Application.DTOs;
using WalletApi.Application.Interfaces.Services;

namespace WalletApi.Api.Controllers;


[Route("v1/api/transfer")]
[Authorize]
[ApiController]
public class TransferController(ITransferService transferService) : ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> Transfer([FromBody] TransferModel model)
    {
        var fromUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await transferService.Transfer(model, fromUserId);

        return result == null ? BadRequest(new { message = "Saldo insuficiente!" }) : Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTransfers([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await transferService.GetTransfers(startDate, endDate, userId);
        
        return result.Count == 0 ? NotFound() : Ok(result);
    }
}