using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;

namespace DanceSchool.Ui.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<IEnumerable<Group>> GetGroupsByAgeCategoryAsync(AgeCategory ageCategory);
        Task<IEnumerable<Group>> GetGroupsWithStudentsAsync();
        Task<IEnumerable<Group>> GetAllGroupsWithDetailsAsync();
        Task<Group?> GetGroupWithDetailsAsync(int id);
        Task<Group?> GetGroupWithStudentsAsync(int id);
        Task<IEnumerable<Group>> GetAvailableGroupsAsync();
    }
}