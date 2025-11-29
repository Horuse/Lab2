using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;

namespace DanceSchool.Ui.Repositories
{
    public interface IStudioRepository : IRepository<Studio>
    {
        Task<IEnumerable<Studio>> GetStudiosWithCapacityAsync(int minCapacity);
        Task<bool> IsStudioAvailableAsync(int studioId, DateTime dateTime);
    }
}