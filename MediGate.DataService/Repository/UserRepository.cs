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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                return await dbSet.Where(x => x.Status == 1)
                .AsNoTracking()
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetAll method has generated error", typeof(UserRepository));
                return new List<User>();
            }
        }

        public async Task<bool> UpdateUserProfile(User user)
        {
            try
            {
                var existingUser = await dbSet.Where(x => x.Status == 1
                && x.Id == user.Id)
                .FirstOrDefaultAsync();

                if (existingUser == null) return false;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.MobileNumber = user.MobileNumber;
                existingUser.Phone = user.Phone;
                existingUser.Sex = user.Sex;
                existingUser.Address = user.Address;
                existingUser.UpdateDate = DateTime.UtcNow;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} UpdateUserProfile method has generated error", typeof(UserRepository));
                return false;
            }
        }
    }
}