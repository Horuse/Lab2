using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;

namespace DanceSchool.Ui.Repositories
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<IEnumerable<Instructor>> GetInstructorsWithGroupsAsync();
        Task<IEnumerable<Instructor>> GetInstructorsWithDetailsAsync();
        Task<Instructor?> GetInstructorWithDetailsAsync(int id);
        Task<IEnumerable<Instructor>> GetAvailableInstructorsAsync();
    }
}