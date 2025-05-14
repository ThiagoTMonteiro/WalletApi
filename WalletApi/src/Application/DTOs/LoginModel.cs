using System.ComponentModel.DataAnnotations;

namespace WalletApi.Application.DTOs;

public class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}