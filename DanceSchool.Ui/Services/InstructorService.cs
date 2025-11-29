using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Ui.Repositories;

namespace DanceSchool.Ui.Services
{
    public class InstructorService
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
        {
            return await _instructorRepository.GetAllAsync();
        }

        public async Task<Instructor?> GetInstructorByIdAsync(int id)
        {
            return await _instructorRepository.GetByIdAsync(id);
        }

        public async Task CreateInstructorAsync(Instructor instructor)
        {
            await _instructorRepository.AddAsync(instructor);
            await _instructorRepository.SaveChangesAsync();
        }

        public async Task UpdateInstructorAsync(Instructor instructor)
        {
            _instructorRepository.Update(instructor);
            await _instructorRepository.SaveChangesAsync();
        }

        public async Task DeleteInstructorAsync(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor != null)
            {
                _instructorRepository.Delete(instructor);
                await _instructorRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Instructor>> GetInstructorsWithDetailsAsync()
        {
            return await _instructorRepository.GetInstructorsWithDetailsAsync();
        }
    }
}