using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Repositories;

namespace DanceSchool.Ui.Services
{
    public class StudioService
    {
        private readonly IStudioRepository _studioRepository;

        public StudioService(IStudioRepository studioRepository)
        {
            _studioRepository = studioRepository;
        }

        public async Task<IEnumerable<Studio>> GetAllStudiosAsync()
        {
            return await _studioRepository.GetAllAsync();
        }

        public async Task<Studio?> GetStudioByIdAsync(int id)
        {
            return await _studioRepository.GetByIdAsync(id);
        }

        public async Task CreateStudioAsync(Studio studio)
        {
            await _studioRepository.AddAsync(studio);
            await _studioRepository.SaveChangesAsync();
        }

        public async Task UpdateStudioAsync(Studio studio)
        {
            _studioRepository.Update(studio);
            await _studioRepository.SaveChangesAsync();
        }

        public async Task DeleteStudioAsync(int id)
        {
            var studio = await _studioRepository.GetByIdAsync(id);
            if (studio != null)
            {
                _studioRepository.Delete(studio);
                await _studioRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Studio>> GetStudiosWithCapacityAsync(int minCapacity)
        {
            return await _studioRepository.GetStudiosWithCapacityAsync(minCapacity);
        }

        public async Task<bool> IsStudioAvailableAsync(int studioId, DateTime dateTime)
        {
            return await _studioRepository.IsStudioAvailableAsync(studioId, dateTime);
        }
    }
}