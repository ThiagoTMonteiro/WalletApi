using WalletApi.Application.DTOs;
using WalletApi.Application.DTOs.Response;

namespace WalletApi.Application.Interfaces.Services;

public interface ITransferService
{
    Task<TransferResponse> Transfer(TransferModel model, string fromUserId);
    Task<List<TransferResponse>> GetTransfers(DateTime? startDate, DateTime? endDate, string userId);
}