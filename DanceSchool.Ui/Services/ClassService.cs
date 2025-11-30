using System;
using System.Collections.Generic;
using System.Linq;
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
            var validationError = await ValidateClassAsync(newClass);
            if (!string.IsNullOrEmpty(validationError))
                throw new InvalidOperationException(validationError);

            await _classRepository.AddAsync(newClass);
            await _classRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateClassAsync(Class classToUpdate)
        {
            var validationError = await ValidateClassAsync(classToUpdate, classToUpdate.Id);
            if (!string.IsNullOrEmpty(validationError))
                throw new InvalidOperationException(validationError);

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

        public async Task<IEnumerable<Class>> GetClassesByInstructorAndDateAsync(int instructorId, DateTime date)
        {
            var allClasses = await _classRepository.GetClassesByInstructorIdAsync(instructorId);
            return allClasses.Where(c => c.Date.Date == date.Date);
        }

        public async Task<IEnumerable<Class>> GetClassesByStudentAndDateAsync(int studentId, DateTime date)
        {
            // Потрібно отримати заняття через групи, до яких належить студент
            var studentGroups = await GetStudentGroupsAsync(studentId);
            var allClasses = await _classRepository.GetClassesWithDetailsAsync();
            
            return allClasses.Where(c => c.Date.Date == date.Date && 
                                        studentGroups.Any(g => g.Id == c.GroupId));
        }

        private async Task<IEnumerable<Group>> GetStudentGroupsAsync(int studentId)
        {
            var groups = await _groupRepository.GetGroupsWithStudentsAsync();
            var studentGroups = new List<Group>();
            
            foreach (var group in groups)
            {
                if (group.StudentGroups?.Any(sg => sg.StudentId == studentId) == true)
                {
                    studentGroups.Add(group);
                }
            }
            
            return studentGroups;
        }

        private async Task<string> ValidateClassAsync(Class classEntity, int? excludeId = null)
        {
            var studioAvailable = await _classRepository.IsStudioAvailableForClassAsync(
                classEntity.StudioId, classEntity.Date, classEntity.StartTime, classEntity.EndTime, excludeId);
            
            var instructorAvailable = await _classRepository.IsInstructorAvailableForClassAsync(
                classEntity.InstructorId, classEntity.Date, classEntity.StartTime, classEntity.EndTime, excludeId);

            if (!studioAvailable)
                return "Студія зайнята на цей час.";
            
            if (!instructorAvailable)
                return "Інструктор зайнятий на цей час.";

            return string.Empty;
        }
    }
}