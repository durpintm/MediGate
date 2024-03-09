using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediGate.Entities.DbSet;

namespace MediGate.DataService.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> UpdateUserProfile(User user);
        Task<User> GetByIdentityId(Guid identityId);
    }
}