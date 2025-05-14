using Microsoft.AspNetCore.Identity;
using WalletApi.Application.DTOs;
using WalletApi.Application.DTOs.Response;

namespace WalletApi.Application.Interfaces.Services;

public interface IAuthService
{
    Task<IdentityResult> Register(RegisterModel model);
    Task<AuthResponse> Login(LoginModel model);
}