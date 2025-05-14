namespace WalletApi.Domain.Entities;

public class Transfer
{
    public int Id { get; set; }
    public string FromUserId { get; set; }
    public string ToUserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransferDate { get; set; }
}