using System.ComponentModel.DataAnnotations;

namespace WalletApi.Application.DTOs;

public class RegisterModel
{
    [Required]
    [MinLength(6)]
    public string? UserName { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; }
}