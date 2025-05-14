namespace WalletApi.Application.DTOs.Response;

public class AppUserResponse
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string[]? Roles { get; set; }
    public decimal WalletBalance { get; set; }
}