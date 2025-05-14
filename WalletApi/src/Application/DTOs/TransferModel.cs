namespace WalletApi.Application.DTOs;

public class TransferModel
{
    public string ToUserId { get; set; }
    public decimal Amount { get; set; }
}