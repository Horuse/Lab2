using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceSchool.Data;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace DanceSchool.Ui.Repositories
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public ClassRepository(DanceSchoolDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Class>> GetClassesForDateAsync(DateTime date)
        {
            var classes = await _dbSet
                .Where(c => c.Date.Date == date.Date)
                .Include(c => c.Group)
                .Include(c => c.Instructor)
                .Include(c => c.Studio)
                .ToListAsync();
                
            return classes.OrderBy(c => c.StartTime);
        }

        public async Task<IEnumerable<Class>> GetClassesWithDetailsAsync()
        {
            var classes = await _dbSet
                .Include(c => c.Group)
                .Include(c => c.Instructor)
                .Include(c => c.Studio)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
                
            return classes.OrderByDescending(c => c.Date).ThenBy(c => c.StartTime);
        }

        public async Task<IEnumerable<Class>> GetClassesByGroupIdAsync(int groupId)
        {
            var classes = await _dbSet
                .Where(c => c.GroupId == groupId)
                .Include(c => c.Instructor)
                .Include(c => c.Studio)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
                
            return classes.OrderByDescending(c => c.Date).ThenBy(c => c.StartTime);
        }

        public async Task<IEnumerable<Class>> GetClassesByInstructorIdAsync(int instructorId)
        {
            var classes = await _dbSet
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.Group)
                .Include(c => c.Studio)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
                
            return classes.OrderByDescending(c => c.Date).ThenBy(c => c.StartTime);
        }

        public async Task<Class?> GetClassWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Group)
                .Include(c => c.Instructor)
                .Include(c => c.Studio)
                .Include(c => c.Attendances)
                .ThenInclude(a => a.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Class>> GetClassesByStudioAsync(int studioId)
        {
            return await _dbSet
                .Where(c => c.StudioId == studioId)
                .Include(c => c.Group)
                .Include(c => c.Instructor)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Class>> GetClassesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(c => c.Date >= startDate && c.Date <= endDate)
                .Include(c => c.Group)
                .Include(c => c.Instructor)
                .Include(c => c.Studio)
                .OrderBy(c => c.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Class>> GetUpcomingClassesAsync()
        {
            return await _dbSet
                .Where(c => c.Date >= DateTime.Now)
                .Include(c => c.Group)
                .Include(c => c.Instructor)
                .Include(c => c.Studio)
                .OrderBy(c => c.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Class>> GetClassesByTypeAsync(ClassType classType)
        {
            return await _dbSet
                .Where(c => c.ClassType == classType)
                .Include(c => c.Group)
                .Include(c => c.Instructor)
                .Include(c => c.Studio)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
        }

        public async Task<bool> IsStudioAvailableForClassAsync(int studioId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeClassId = null)
        {
            var classes = await _dbSet.Where(c => c.StudioId == studioId &&
                                                 c.Date.Date == date.Date)
                                     .ToListAsync();

            if (excludeClassId.HasValue)
                classes = classes.Where(c => c.Id != excludeClassId.Value).ToList();

            // Перевірка перетину часу на клієнті
            return !classes.Any(c => c.StartTime < endTime && c.EndTime > startTime);
        }

        public async Task<bool> IsInstructorAvailableForClassAsync(int instructorId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeClassId = null)
        {
            var classes = await _dbSet.Where(c => c.InstructorId == instructorId &&
                                                 c.Date.Date == date.Date)
                                     .ToListAsync();

            if (excludeClassId.HasValue)
                classes = classes.Where(c => c.Id != excludeClassId.Value).ToList();

            // Перевірка перетину часу на клієнті
            return !classes.Any(c => c.StartTime < endTime && c.EndTime > startTime);
        }
    }
}