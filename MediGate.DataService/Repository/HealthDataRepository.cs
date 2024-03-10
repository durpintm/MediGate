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
    public class HealthDataRepository : GenericRepository<HealthData>, IHealthDataRepository
    {

        public HealthDataRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<HealthData>> GetAll()
        {
            try
            {
                return await dbSet.Where(x => x.Status == 1)
                .AsNoTracking()
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetAll method has generated error", typeof(HealthDataRepository));
                return new List<HealthData>();
            }
        }

        public async Task<bool> UpdateHealthData(HealthData healthData)
        {
            try
            {
                var existingHealthData = await dbSet.Where(x => x.Status == 1
                && x.Id == healthData.Id)
                .FirstOrDefaultAsync();

                if (existingHealthData == null) return false;

                existingHealthData.BloodType = healthData.BloodType;
                existingHealthData.Height = healthData.Height;
                existingHealthData.Race = healthData.Race;
                existingHealthData.Weight = healthData.Weight;
                existingHealthData.UseGlasses = healthData.UseGlasses;
                existingHealthData.UpdateDate = DateTime.UtcNow;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} UpdateHealthData method has generated error", typeof(HealthDataRepository));
                return false;
            }
        }



    }
}