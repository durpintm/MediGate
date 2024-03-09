using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediGate.DataService.IRepository;

namespace MediGate.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }

        Task CompleteAsync();
    }
}