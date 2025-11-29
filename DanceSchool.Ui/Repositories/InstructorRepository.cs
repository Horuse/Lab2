using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data;
using DanceSchool.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DanceSchool.Ui.Repositories
{
    public class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(DanceSchoolDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Instructor>> GetInstructorsWithGroupsAsync()
        {
            return await _dbSet
                .Include(i => i.GroupInstructors)
                .ThenInclude(gi => gi.Group)
                .ToListAsync();
        }

        public async Task<IEnumerable<Instructor>> GetInstructorsWithDetailsAsync()
        {
            return await _dbSet
                .Include(i => i.GroupInstructors)
                .ThenInclude(gi => gi.Group)
                .Include(i => i.Classes)
                .ThenInclude(c => c.Group)
                .ToListAsync();
        }

        public async Task<Instructor?> GetInstructorWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(i => i.GroupInstructors)
                .ThenInclude(gi => gi.Group)
                .Include(i => i.Classes)
                .ThenInclude(c => c.Group)
                .Include(i => i.TrialLessons)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Instructor>> GetAvailableInstructorsAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}