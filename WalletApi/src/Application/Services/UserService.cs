using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WalletApi.Application.DTOs;
using WalletApi.Application.Interfaces.Services;
using WalletApi.Domain.Entities;

namespace WalletApi.Application.Services;

public class UserService(
    UserManager<AppUser> userManager,
    IMapper mapper) :  IUserService
{
    public async Task<IdentityResult> Register(RegisterModel model)
    {
        var user = mapper.Map<AppUser>(model);
        return await userManager.CreateAsync(user, model.Password);
    }
    
}