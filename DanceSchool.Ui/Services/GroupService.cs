using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using DanceSchool.Ui.Repositories;

namespace DanceSchool.Ui.Services
{
    public class GroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IInstructorRepository _instructorRepository;

        public GroupService(IGroupRepository groupRepository, IInstructorRepository instructorRepository)
        {
            _groupRepository = groupRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task<IEnumerable<Group>> GetAllGroupsAsync()
        {
            return await _groupRepository.GetAllAsync();
        }

        public async Task<Group?> GetGroupByIdAsync(int id)
        {
            return await _groupRepository.GetByIdAsync(id);
        }

        public async Task<Group?> GetGroupWithDetailsAsync(int id)
        {
            return await _groupRepository.GetGroupWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Group>> GetGroupsByAgeCategoryAsync(AgeCategory ageCategory)
        {
            return await _groupRepository.GetGroupsByAgeCategoryAsync(ageCategory);
        }

        public async Task<IEnumerable<Group>> GetGroupsWithStudentsAsync()
        {
            return await _groupRepository.GetGroupsWithStudentsAsync();
        }

        public async Task<IEnumerable<Group>> GetAllGroupsWithDetailsAsync()
        {
            return await _groupRepository.GetAllGroupsWithDetailsAsync();
        }

        public async Task<IEnumerable<Group>> GetAvailableGroupsAsync()
        {
            return await _groupRepository.GetAvailableGroupsAsync();
        }

        public async Task<bool> CreateGroupAsync(Group group)
        {
            try
            {
                if (group.MaxCapacity <= 0) return false;

                await _groupRepository.AddAsync(group);
                await _groupRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateGroupAsync(Group group)
        {
            try
            {
                if (group.MaxCapacity <= 0) return false;

                _groupRepository.Update(group);
                await _groupRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            try
            {
                var group = await _groupRepository.GetGroupWithDetailsAsync(id);
                if (group == null) return false;

                if (group.StudentGroups.Any() || group.Classes.Any())
                {
                    return false;
                }

                _groupRepository.Delete(group);
                await _groupRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AssignInstructorToGroupAsync(int groupId, int instructorId)
        {
            try
            {
                var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);
                var instructor = await _instructorRepository.GetByIdAsync(instructorId);

                if (group == null || instructor == null) return false;

                var existingAssignment = group.GroupInstructors.Any(gi => gi.InstructorId == instructorId);
                if (existingAssignment) return false;

                var groupInstructor = new GroupInstructor
                {
                    GroupId = groupId,
                    InstructorId = instructorId,
                    AssignedDate = DateTime.Now
                };

                group.GroupInstructors.Add(groupInstructor);
                await _groupRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveInstructorFromGroupAsync(int groupId, int instructorId)
        {
            try
            {
                var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);
                if (group == null) return false;

                var assignment = group.GroupInstructors.FirstOrDefault(gi => gi.InstructorId == instructorId);
                if (assignment == null) return false;

                group.GroupInstructors.Remove(assignment);
                await _groupRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddStudentToGroupAsync(int groupId, int studentId)
        {
            try
            {
                var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);
                if (group == null) return false;

                // Check if group is at capacity
                if (group.StudentGroups.Count >= group.MaxCapacity) return false;

                // Check if student is already in the group
                var existingAssignment = group.StudentGroups.Any(sg => sg.StudentId == studentId);
                if (existingAssignment) return false;

                var studentGroup = new StudentGroup
                {
                    GroupId = groupId,
                    StudentId = studentId,
                    EnrollmentDate = DateTime.Now
                };

                group.StudentGroups.Add(studentGroup);
                await _groupRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveStudentFromGroupAsync(int groupId, int studentId)
        {
            try
            {
                var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);
                if (group == null) return false;

                var assignment = group.StudentGroups.FirstOrDefault(sg => sg.StudentId == studentId);
                if (assignment == null) return false;

                group.StudentGroups.Remove(assignment);
                await _groupRepository.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}