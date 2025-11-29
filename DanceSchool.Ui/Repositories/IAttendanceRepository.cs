using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;

namespace DanceSchool.Ui.Repositories
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task<IEnumerable<Attendance>> GetAttendanceByStudentAsync(int studentId);
        Task<IEnumerable<Attendance>> GetAttendanceByClassAsync(int classId);
        Task<IEnumerable<Attendance>> GetAttendanceByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<double> GetAttendanceRateAsync(int studentId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Attendance>> GetAttendanceByStatusAsync(AttendanceStatus status);
    }
}