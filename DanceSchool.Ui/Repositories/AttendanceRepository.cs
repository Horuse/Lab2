using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceSchool.Data;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace DanceSchool.Ui.Repositories
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(DanceSchoolDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByStudentAsync(int studentId)
        {
            return await _dbSet
                .Include(a => a.Student)
                .Include(a => a.Class)
                .ThenInclude(c => c.Group)
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.Class.Date)
                .ThenByDescending(a => a.Class.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByClassAsync(int classId)
        {
            return await _dbSet
                .Include(a => a.Student)
                .Where(a => a.ClassId == classId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Where(a => a.Class.Date >= startDate.Date && a.Class.Date <= endDate.Date)
                .ToListAsync();
        }

        public async Task<double> GetAttendanceRateAsync(int studentId, DateTime startDate, DateTime endDate)
        {
            var totalClasses = await _context.Classes
                .Include(c => c.Group)
                .ThenInclude(g => g.StudentGroups)
                .Where(c => c.Date >= startDate.Date && 
                           c.Date <= endDate.Date && 
                           c.Group.StudentGroups.Any(sg => sg.StudentId == studentId))
                .CountAsync();

            if (totalClasses == 0) return 0;

            var attendedClasses = await _dbSet
                .Include(a => a.Class)
                .Where(a => a.StudentId == studentId && 
                           a.Class.Date >= startDate.Date && 
                           a.Class.Date <= endDate.Date &&
                           a.Status == AttendanceStatus.Present)
                .CountAsync();

            return (double)attendedClasses / totalClasses * 100;
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByStatusAsync(AttendanceStatus status)
        {
            return await _dbSet
                .Include(a => a.Student)
                .Include(a => a.Class)
                .Where(a => a.Status == status)
                .ToListAsync();
        }
    }
}