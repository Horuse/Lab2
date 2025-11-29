using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;

namespace DanceSchool.Ui.Repositories
{
    public interface IClassRepository : IRepository<Class>
    {
        Task<IEnumerable<Class>> GetClassesForDateAsync(DateTime date);
        Task<IEnumerable<Class>> GetClassesWithDetailsAsync();
        Task<IEnumerable<Class>> GetClassesByGroupIdAsync(int groupId);
        Task<IEnumerable<Class>> GetClassesByInstructorIdAsync(int instructorId);
        Task<Class?> GetClassWithDetailsAsync(int id);
        Task<IEnumerable<Class>> GetClassesByStudioAsync(int studioId);
        Task<IEnumerable<Class>> GetClassesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Class>> GetUpcomingClassesAsync();
        Task<IEnumerable<Class>> GetClassesByTypeAsync(ClassType classType);
        Task<bool> IsStudioAvailableForClassAsync(int studioId, DateTime dateTime, int? excludeClassId = null);
        Task<bool> IsInstructorAvailableForClassAsync(int instructorId, DateTime dateTime, int? excludeClassId = null);
    }
}