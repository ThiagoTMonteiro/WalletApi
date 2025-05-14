namespace WalletApi.Application.DTOs.Response;

public class TransferResponse
{
    public string SenderUserId { get; set; }
    public string ReceiverUserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}