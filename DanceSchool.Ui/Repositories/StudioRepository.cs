using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceSchool.Data;
using DanceSchool.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DanceSchool.Ui.Repositories
{
    public class StudioRepository : Repository<Studio>, IStudioRepository
    {
        public StudioRepository(DanceSchoolDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Studio>> GetStudiosWithCapacityAsync(int minCapacity)
        {
            return await _dbSet
                .Where(s => s.Capacity >= minCapacity)
                .ToListAsync();
        }

        public async Task<bool> IsStudioAvailableAsync(int studioId, DateTime dateTime)
        {
            return !await _context.Classes
                .AnyAsync(c => c.StudioId == studioId && 
                              c.Date.Date == dateTime.Date &&
                              Math.Abs((c.Date - dateTime).TotalHours) < 1);
        }
    }
}