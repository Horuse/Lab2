using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceSchool.Data;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace DanceSchool.Ui.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(DanceSchoolDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Group>> GetGroupsByAgeCategoryAsync(AgeCategory ageCategory)
        {
            return await _dbSet
                .Where(g => g.AgeCategory == ageCategory)
                .ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetGroupsWithStudentsAsync()
        {
            return await _dbSet
                .Include(g => g.StudentGroups)
                .ThenInclude(sg => sg.Student)
                .ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetAllGroupsWithDetailsAsync()
        {
            return await _dbSet
                .Include(g => g.StudentGroups)
                .ThenInclude(sg => sg.Student)
                .Include(g => g.GroupInstructors)
                .ThenInclude(gi => gi.Instructor)
                .ToListAsync();
        }

        public async Task<Group?> GetGroupWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(g => g.StudentGroups)
                .ThenInclude(sg => sg.Student)
                .Include(g => g.GroupInstructors)
                .ThenInclude(gi => gi.Instructor)
                .Include(g => g.Classes)
                .ThenInclude(c => c.Studio)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Group>> GetAvailableGroupsAsync()
        {
            return await _dbSet
                .Where(g => g.StudentGroups.Count < g.MaxCapacity)
                .ToListAsync();
        }
    }
}