using DanceSchool.Data;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace DanceSchool.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("=== Школа танців - Демонстрація роботи БД ===\n");

            using (var context = new DanceSchoolDbContext())
            {
                System.Console.WriteLine("Ініціалізація бази даних...");
                DbInitializer.Seed(context);
                System.Console.WriteLine("Дані успішно додано!\n");
                
                System.Console.WriteLine("=== ДЕМОНСТРАЦІЯ LINQ ЗАПИТІВ ===\n");
                
                // з умовою
                var starterStudents = context.Students
                    .Where(s => s.SkillLevel == SkillLevel.Starter)
                    .ToList();

                System.Console.WriteLine($"=== Учні рівня Starter ({starterStudents.Count}) ===");
                foreach (var student in starterStudents)
                {
                    System.Console.WriteLine($"{student.FirstName} {student.LastName}");
                }

                var kids68Groups = context.Groups
                    .Where(g => g.AgeCategory == AgeCategory.Kids6_8)
                    .ToList();

                System.Console.WriteLine($"\n=== Групи для дітей 6-8 років ({kids68Groups.Count}) ===");
                foreach (var group in kids68Groups)
                {
                    System.Console.WriteLine($"{group.Name} - Рівень: {group.SkillLevel}");
                }

                var todayClasses = context.Classes
                    .Where(c => c.Date.Date == DateTime.Today)
                    .Include(c => c.Group)
                    .ToList();

                System.Console.WriteLine($"\n=== Заняття на сьогодні ({todayClasses.Count}) ===");
                foreach (var cls in todayClasses)
                {
                    System.Console.WriteLine($"{cls.Group.Name} - {cls.StartTime} до {cls.EndTime}");
                }

                // звязки
                var studentsWithGroups = context.Students
                    .Include(s => s.StudentGroups)
                        .ThenInclude(sg => sg.Group)
                    .ToList();

                System.Console.WriteLine($"\n=== Учні та їхні групи ===");
                foreach (var student in studentsWithGroups)
                {
                    System.Console.WriteLine($"\n{student.FirstName} {student.LastName}:");
                    foreach (var sg in student.StudentGroups)
                    {
                        System.Console.WriteLine($"  - {sg.Group.Name}");
                    }
                }

                var classesWithDetails = context.Classes
                    .Include(c => c.Instructor)
                    .Include(c => c.Group)
                    .Include(c => c.Studio)
                    .Take(10)
                    .ToList();

                System.Console.WriteLine($"\n=== Деталі занять (перші 10) ===");
                foreach (var cls in classesWithDetails)
                {
                    System.Console.WriteLine($"\nГрупа: {cls.Group.Name}");
                    System.Console.WriteLine($"Педагог: {cls.Instructor.FirstName} {cls.Instructor.LastName}");
                    System.Console.WriteLine($"Зала: {cls.Studio.Name}");
                    System.Console.WriteLine($"Час: {cls.Date.ToShortDateString()} {cls.StartTime}-{cls.EndTime}");
                }

                var performancesWithStudents = context.Performances
                    .Include(p => p.PerformanceStudents)
                        .ThenInclude(ps => ps.Student)
                    .ToList();

                System.Console.WriteLine($"\n=== Виступи та їх учасники ===");
                foreach (var performance in performancesWithStudents)
                {
                    System.Console.WriteLine($"\n{performance.Name} ({performance.Date.ToShortDateString()}):");
                    foreach (var ps in performance.PerformanceStudents)
                    {
                        System.Console.WriteLine($"  - {ps.Student.FirstName} {ps.Student.LastName} ({ps.Role ?? "учасник"})");
                    }
                }

                // агрегація
                var groupStudentCounts = context.Groups
                    .Select(g => new
                    {
                        GroupName = g.Name,
                        StudentCount = g.StudentGroups.Count,
                        MaxCapacity = g.MaxCapacity
                    })
                    .OrderByDescending(x => x.StudentCount)
                    .ToList();

                System.Console.WriteLine($"\n=== Кількість учнів по групах ===");
                foreach (var item in groupStudentCounts)
                {
                    System.Console.WriteLine($"{item.GroupName}: {item.StudentCount}/{item.MaxCapacity} учнів");
                }

                var totalAttendances = context.Attendances.Count();
                var presentCount = context.Attendances.Count(a => a.Status == AttendanceStatus.Present);
                var averageAttendance = totalAttendances > 0 ? (double)presentCount / totalAttendances * 100 : 0;

                System.Console.WriteLine($"\n=== Статистика відвідуваності ===");
                System.Console.WriteLine($"Всього відміток: {totalAttendances}");
                System.Console.WriteLine($"Присутні: {presentCount}");
                System.Console.WriteLine($"Середня відвідуваність: {averageAttendance:F2}%");

                var studentAbsences = context.Students
                    .Select(s => new
                    {
                        StudentName = s.FirstName + " " + s.LastName,
                        AbsenceCount = s.Attendances.Count(a => a.Status == AttendanceStatus.Absent)
                    })
                    .Where(x => x.AbsenceCount > 0)
                    .OrderByDescending(x => x.AbsenceCount)
                    .Take(5)
                    .ToList();

                System.Console.WriteLine($"\n=== Топ-5 учнів з пропусками ===");
                foreach (var item in studentAbsences)
                {
                    System.Console.WriteLine($"{item.StudentName}: {item.AbsenceCount} пропусків");
                }

                var upcomingPerformances = context.Performances
                    .Where(p => p.Date >= DateTime.Now)
                    .OrderBy(p => p.Date)
                    .ToList();

                System.Console.WriteLine($"\n=== Найближчі виступи ===");
                foreach (var perf in upcomingPerformances)
                {
                    System.Console.WriteLine($"{perf.Name} - {perf.Date.ToShortDateString()} ({perf.PerformanceType})");
                }

                // додатково
                var instructorWorkload = context.Instructors
                    .Select(i => new
                    {
                        InstructorName = i.FirstName + " " + i.LastName,
                        GroupCount = i.GroupInstructors.Count,
                        ClassCount = i.Classes.Count,
                        Specialization = i.Specialization
                    })
                    .ToList();

                System.Console.WriteLine($"\n=== Навантаження інструкторів ===");
                foreach (var item in instructorWorkload)
                {
                    System.Console.WriteLine($"{item.InstructorName}: {item.GroupCount} груп, {item.ClassCount} занять ({item.Specialization})");
                }

                var subscriptionStats = context.Subscriptions
                    .ToList()
                    .GroupBy(s => s.SubscriptionType)
                    .Select(g => new
                    {
                        Type = g.Key,
                        Count = g.Count(),
                        PaidCount = g.Count(s => s.IsPaid),
                        AveragePrice = g.Average(s => s.Price)
                    })
                    .ToList();

                System.Console.WriteLine($"\n=== Статистика абонементів ===");
                foreach (var item in subscriptionStats)
                {
                    System.Console.WriteLine($"{item.Type}: {item.Count} шт, оплачених: {item.PaidCount}, середня ціна: {item.AveragePrice:C}");
                }

                System.Console.WriteLine("\n=== Завершено ===");
            }
        }
    }
}
