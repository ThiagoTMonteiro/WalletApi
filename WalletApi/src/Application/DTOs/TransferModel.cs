namespace WalletApi.Application.DTOs;

public class TransferModel
{
    public string ReceiverUserId { get; set; }
    public decimal Amount { get; set; }
}