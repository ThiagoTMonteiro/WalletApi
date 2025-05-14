using Microsoft.AspNetCore.Identity;

namespace WalletApi.Domain.Entities;

public class AppUser : IdentityUser
{
    public decimal WalletBalance { get; set; } = 0m;
}