using WalletApi.Application.DTOs;
using WalletApi.Application.DTOs.Response;

namespace WalletApi.Application.Interfaces.Services;

public interface IWalletService
{
    Task<AppUserResponse> GetBalanceAsync(string userId);
    Task<WalletResponse> AddBalanceAsync(string userId, WalletModel model);
}