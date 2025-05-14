using WalletApi.Application.Interfaces.Repositories;
using WalletApi.Domain.Entities;
using WalletApi.Infrastructure.Data;

namespace WalletApi.Infrastructure.Repository;

public class UserRepository(AppDbContext context) : BaseRepository<AppUser>(context), IUserRepository
{
    
}