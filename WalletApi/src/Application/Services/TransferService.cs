using AutoMapper;
using WalletApi.Application.DTOs;
using WalletApi.Application.DTOs.Response;
using WalletApi.Application.Interfaces.Repositories;
using WalletApi.Application.Interfaces.Services;
using WalletApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace WalletApi.Application.Services;

public class TransferService(IUserRepository userRepository, ITransferRepository transferRepository, IMapper mapper) 
    : ITransferService
{
    public async Task<TransferResponse> Transfer(TransferModel model, string senderUserId)
    {
        var fromUser = await userRepository.GetByIdAsync(senderUserId);
        var toUser = await userRepository.GetByIdAsync(model.ReceiverUserId);

        if (fromUser == null || toUser == null) return new TransferResponse();

        if (fromUser.WalletBalance < model.Amount)
            throw new Exception("Saldo insuficiente."); 

        fromUser.WalletBalance -= model.Amount;
        toUser.WalletBalance += model.Amount;

        var transfer = new Transfer
        {
            SenderUserId = senderUserId,
            ReceiverUserId = model.ReceiverUserId,
            Amount = model.Amount,
            TransferDate = DateTime.UtcNow
        };
        
        await transferRepository.AddAsync(transfer);
        await transferRepository.SaveChangesAsync();

        return mapper.Map<TransferResponse>(transfer);
    }
    
    public async Task<List<TransferResponse> > GetTransfers(DateTime? startDate, DateTime? endDate, string userId)
    {
        var transfers = await transferRepository
            .AsQueryable()
            .Where(t =>
                (t.SenderUserId == userId || t.ReceiverUserId == userId) &&
                (!startDate.HasValue || t.TransferDate >= startDate.Value) &&
                (!endDate.HasValue || t.TransferDate <= endDate.Value))
            .ToListAsync();
        
        return transfers.Count == 0 ? [] : mapper.Map<List<TransferResponse>>(transfers);

    }
}