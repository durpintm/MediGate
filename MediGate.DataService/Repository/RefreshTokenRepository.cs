using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediGate.DataService.Data;
using MediGate.DataService.IRepository;
using MediGate.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediGate.DataService.Repository
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<RefreshToken>> GetAll()
        {
            try
            {
                return await dbSet.Where(x => x.Status == 1)
                .AsNoTracking()
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetAll method has generated error", typeof(RefreshTokenRepository));
                return new List<RefreshToken>();
            }
        }

        public async Task<RefreshToken> GetByRefreshToken(string refreshToken)
        {
            try
            {
                return await dbSet.Where(x => x.Token == refreshToken)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetAll method has generated error", typeof(RefreshTokenRepository));
                return null;
            }
        }

        public async Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken)
        {
            try
            {
                var token = await dbSet.Where(x => x.Token.ToLower() == refreshToken.Token.ToLower())
                .AsNoTracking()
                .FirstOrDefaultAsync();

                if (token is null)
                {
                    return false;
                }

                token.IsUsed = refreshToken.IsUsed;
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetAll method has generated error", typeof(RefreshTokenRepository));
                return false;
            }
        }
    }
}