using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceSchool.Data;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace DanceSchool.Ui.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(DanceSchoolDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Student>> GetStudentsBySkillLevelAsync(SkillLevel skillLevel)
        {
            return await _dbSet
                .Where(s => s.SkillLevel == skillLevel)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsWithGroupsAsync()
        {
            return await _dbSet
                .Include(s => s.StudentGroups)
                .ThenInclude(sg => sg.Group)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsByGroupIdAsync(int groupId)
        {
            return await _dbSet
                .Where(s => s.StudentGroups.Any(sg => sg.GroupId == groupId))
                .ToListAsync();
        }

        public async Task<Student?> GetStudentWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.StudentGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.Attendances)
                .Include(s => s.Subscriptions)
                .Include(s => s.TrialLessons)
                .Include(s => s.PerformanceStudents)
                .ThenInclude(ps => ps.Performance)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}