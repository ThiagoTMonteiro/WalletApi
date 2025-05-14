using WalletApi.Application.Interfaces.Repositories;
using WalletApi.Domain.Entities;
using WalletApi.Infrastructure.Data;

namespace WalletApi.Infrastructure.Repository;

public class TransferRepository (AppDbContext context) : BaseRepository<Transfer> (context), ITransferRepository
{
    
}