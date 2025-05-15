namespace WalletApi.Domain.Entities;

public class Transfer
{
    public int Id { get; set; }
    public string SenderUserId { get; set; }
    public string ReceiverUserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransferDate { get; set; }
}