using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using DanceSchool.Ui.Repositories;

namespace DanceSchool.Ui.Services
{
    public class ClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IStudioRepository _studioRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IGroupRepository _groupRepository;

        public ClassService(IClassRepository classRepository, IStudioRepository studioRepository, 
                           IInstructorRepository instructorRepository, IGroupRepository groupRepository)
        {
            _classRepository = classRepository;
            _studioRepository = studioRepository;
            _instructorRepository = instructorRepository;
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return await _classRepository.GetClassesWithDetailsAsync();
        }

        public async Task<Class?> GetClassByIdAsync(int id)
        {
            return await _classRepository.GetClassWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Class>> GetUpcomingClassesAsync()
        {
            return await _classRepository.GetUpcomingClassesAsync();
        }

        public async Task<bool> CreateClassAsync(Class newClass)
        {
            if (!await ValidateClassAsync(newClass))
                return false;

            await _classRepository.AddAsync(newClass);
            await _classRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateClassAsync(Class classToUpdate)
        {
            if (!await ValidateClassAsync(classToUpdate, classToUpdate.Id))
                return false;

            _classRepository.Update(classToUpdate);
            await _classRepository.SaveChangesAsync();
            return true;
        }

        public async Task DeleteClassAsync(int id)
        {
            var classToDelete = await _classRepository.GetByIdAsync(id);
            if (classToDelete != null)
            {
                _classRepository.Delete(classToDelete);
                await _classRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Class>> GetClassesByDateAsync(DateTime date)
        {
            return await _classRepository.GetClassesForDateAsync(date);
        }

        private async Task<bool> ValidateClassAsync(Class classEntity, int? excludeId = null)
        {
            var studioAvailable = await _classRepository.IsStudioAvailableForClassAsync(
                classEntity.StudioId, classEntity.Date, excludeId);
            
            var instructorAvailable = await _classRepository.IsInstructorAvailableForClassAsync(
                classEntity.InstructorId, classEntity.Date, excludeId);

            return studioAvailable && instructorAvailable;
        }
    }
}