using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediGate.Entities.DbSet;

namespace MediGate.DataService.IRepository
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetByRefreshToken(string refreshToken);
        Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken);

    }
}