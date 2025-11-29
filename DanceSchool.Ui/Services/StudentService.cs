using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using DanceSchool.Ui.Repositories;

namespace DanceSchool.Ui.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;

        public StudentService(IStudentRepository studentRepository, IGroupRepository groupRepository)
        {
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetByIdAsync(id);
        }

        public async Task<Student?> GetStudentWithDetailsAsync(int id)
        {
            return await _studentRepository.GetStudentWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Student>> GetStudentsBySkillLevelAsync(SkillLevel skillLevel)
        {
            return await _studentRepository.GetStudentsBySkillLevelAsync(skillLevel);
        }

        public async Task<IEnumerable<Student>> GetStudentsWithGroupsAsync()
        {
            return await _studentRepository.GetStudentsWithGroupsAsync();
        }

        public async Task<bool> CreateStudentAsync(Student student)
        {
            try
            {
                student.RegistrationDate = DateTime.Now;
                await _studentRepository.AddAsync(student);
                await _studentRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            try
            {
                _studentRepository.Update(student);
                await _studentRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(id);
                if (student == null) return false;

                _studentRepository.Delete(student);
                await _studentRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EnrollStudentInGroupAsync(int studentId, int groupId)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(studentId);
                var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);

                if (student == null || group == null) return false;

                if (group.StudentGroups.Count >= group.MaxCapacity) return false;

                var existingEnrollment = group.StudentGroups.Any(sg => sg.StudentId == studentId);
                if (existingEnrollment) return false;

                var studentGroup = new StudentGroup
                {
                    StudentId = studentId,
                    GroupId = groupId,
                    EnrollmentDate = DateTime.Now
                };

                student.StudentGroups.Add(studentGroup);
                await _studentRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveStudentFromGroupAsync(int studentId, int groupId)
        {
            try
            {
                var student = await _studentRepository.GetStudentWithDetailsAsync(studentId);
                if (student == null) return false;

                var enrollment = student.StudentGroups.FirstOrDefault(sg => sg.GroupId == groupId);
                if (enrollment == null) return false;

                student.StudentGroups.Remove(enrollment);
                await _studentRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Student>> GetStudentsByGroupIdAsync(int groupId)
        {
            return await _studentRepository.GetStudentsByGroupIdAsync(groupId);
        }
    }
}