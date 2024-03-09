using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediGate.DataService.IConfiguration;
using MediGate.DataService.IRepository;
using MediGate.DataService.Repository;
using MediGate.Entities.DbSet;
using Microsoft.Extensions.Logging;

namespace MediGate.DataService.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;


        public IUserRepository Users { get; private set; }

        public IRefreshTokenRepository RefreshTokens { get; private set; }

        public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;

            Users = new UserRepository(_context, _logger);
            RefreshTokens = new RefreshTokenRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}