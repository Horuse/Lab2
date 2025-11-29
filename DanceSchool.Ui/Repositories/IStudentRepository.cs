using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;

namespace DanceSchool.Ui.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetStudentsBySkillLevelAsync(SkillLevel skillLevel);
        Task<IEnumerable<Student>> GetStudentsWithGroupsAsync();
        Task<IEnumerable<Student>> GetStudentsByGroupIdAsync(int groupId);
        Task<Student?> GetStudentWithDetailsAsync(int id);
    }
}