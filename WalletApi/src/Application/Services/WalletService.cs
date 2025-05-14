using AutoMapper;
using WalletApi.Application.DTOs;
using WalletApi.Application.DTOs.Response;
using WalletApi.Application.Interfaces.Repositories;
using WalletApi.Application.Interfaces.Services;


namespace WalletApi.Application.Services;

public class WalletService(IUserRepository userRepository, IMapper mapper)
    : IWalletService
{
    public async Task<AppUserResponse> GetBalanceAsync(string userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        return user == null ? new AppUserResponse() : mapper.Map<AppUserResponse>(user);
    }
    
    public async Task<WalletResponse> AddBalanceAsync(string userId, WalletModel model)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null) return new WalletResponse();

        user.WalletBalance += model.Amount;
        await userRepository.SaveChangesAsync();

        return mapper.Map<WalletResponse>(user);
    }
}