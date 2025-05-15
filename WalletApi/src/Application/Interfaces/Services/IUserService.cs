using Microsoft.AspNetCore.Identity;
using WalletApi.Application.DTOs;

namespace WalletApi.Application.Interfaces.Services;

public interface IUserService
{
    Task<IdentityResult> Register(RegisterModel model);
}