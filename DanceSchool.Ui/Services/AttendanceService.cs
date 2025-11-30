using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using DanceSchool.Ui.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DanceSchool.Ui.Services
{
    public class AttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IClassRepository _classRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository, 
            IGroupRepository groupRepository, 
            IClassRepository classRepository)
        {
            _attendanceRepository = attendanceRepository;
            _groupRepository = groupRepository;
            _classRepository = classRepository;
        }

        public async Task<IEnumerable<Group>> GetGroupsForAttendanceAsync()
        {
            return await _groupRepository.GetAllGroupsWithDetailsAsync();
        }

        public async Task<IEnumerable<Class>> GetClassesByGroupAsync(int groupId)
        {
            return await _classRepository.GetClassesByGroupIdAsync(groupId);
        }

        public async Task<IEnumerable<Student>> GetStudentsByGroupAsync(int groupId)
        {
            var group = await _groupRepository.GetGroupWithDetailsAsync(groupId);
            return group?.StudentGroups?.Select(sg => sg.Student).Where(s => s != null) ?? new List<Student>();
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByClassAsync(int classId)
        {
            return await _attendanceRepository.GetAttendanceByClassAsync(classId);
        }

        public async Task<Attendance> MarkAttendanceAsync(int studentId, int classId, AttendanceStatus status, string? notes = null)
        {
            var existingAttendance = await _attendanceRepository.GetAllAsync();
            var attendance = existingAttendance.FirstOrDefault(a => a.StudentId == studentId && a.ClassId == classId);

            if (attendance == null)
            {
                attendance = new Attendance
                {
                    StudentId = studentId,
                    ClassId = classId,
                    Status = status,
                    Notes = notes
                };
                await _attendanceRepository.AddAsync(attendance);
                await _attendanceRepository.SaveChangesAsync();
            }
            else
            {
                attendance.Status = status;
                attendance.Notes = notes;
                _attendanceRepository.Update(attendance);
                await _attendanceRepository.SaveChangesAsync();
            }

            return attendance;
        }

        public async Task<double> GetAttendanceRateAsync(int studentId, DateTime startDate, DateTime endDate)
        {
            return await _attendanceRepository.GetAttendanceRateAsync(studentId, startDate, endDate);
        }

        public async Task<Dictionary<int, AttendanceStatus>> GetAttendanceStatusForStudentAndClassesAsync(int studentId, IEnumerable<int> classIds)
        {
            var attendances = await _attendanceRepository.GetAllAsync();
            return attendances
                .Where(a => a.StudentId == studentId && classIds.Contains(a.ClassId))
                .ToDictionary(a => a.ClassId, a => a.Status);
        }
    }
}