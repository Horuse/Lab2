using System;
using System.Collections.Generic;
using System.Linq;
using DanceSchool.Data;
using DanceSchool.Data.Entities;
using DanceSchool.Data.Enums;

namespace DanceSchool.Ui.Services
{
    public static class DbInitializer
    {
        public static void Seed(DanceSchoolDbContext context)
        {
            context.Database.EnsureCreated();
            
            if (context.Students.Any())
                return;
            
            var students = new List<Student>
            {
                new Student { FirstName = "Анна", LastName = "Петренко", DateOfBirth = new DateTime(2018, 5, 15), PhoneNumber = "+380501234567", Email = "anna.petrenko@example.com", ParentContact = "+380501234567", SkillLevel = SkillLevel.Starter, RegistrationDate = DateTime.Now.AddMonths(-3) },
                new Student { FirstName = "Максим", LastName = "Коваленко", DateOfBirth = new DateTime(2016, 8, 22), PhoneNumber = "+380502345678", Email = "maxim.kovalenko@example.com", ParentContact = "+380502345678", SkillLevel = SkillLevel.Intermediate, RegistrationDate = DateTime.Now.AddMonths(-6) },
                new Student { FirstName = "Софія", LastName = "Сидоренко", DateOfBirth = new DateTime(2019, 3, 10), PhoneNumber = "+380503456789", Email = "sofia.sidorenko@example.com", ParentContact = "+380503456789", SkillLevel = SkillLevel.Starter, RegistrationDate = DateTime.Now.AddMonths(-2) },
                new Student { FirstName = "Данило", LastName = "Мельник", DateOfBirth = new DateTime(2017, 11, 5), PhoneNumber = "+380504567890", Email = "danylo.melnyk@example.com", ParentContact = "+380504567890", SkillLevel = SkillLevel.Intermediate, RegistrationDate = DateTime.Now.AddMonths(-4) },
                new Student { FirstName = "Марія", LastName = "Бондаренко", DateOfBirth = new DateTime(2015, 1, 18), PhoneNumber = "+380505678901", Email = "maria.bondarenko@example.com", ParentContact = "+380505678901", SkillLevel = SkillLevel.PrePerformance, RegistrationDate = DateTime.Now.AddMonths(-12) },
                new Student { FirstName = "Олександр", LastName = "Шевченко", DateOfBirth = new DateTime(2008, 9, 3), PhoneNumber = "+380506789012", Email = "oleksandr.shevchenko@example.com", ParentContact = "+380506789012", SkillLevel = SkillLevel.Intermediate, RegistrationDate = DateTime.Now.AddMonths(-8) },
                new Student { FirstName = "Катерина", LastName = "Іваненко", DateOfBirth = new DateTime(2007, 4, 28), PhoneNumber = "+380507890123", Email = "kateryna.ivanenko@example.com", ParentContact = "+380507890123", SkillLevel = SkillLevel.PrePerformance, RegistrationDate = DateTime.Now.AddMonths(-10) },
                new Student { FirstName = "Ігор", LastName = "Поліщук", DateOfBirth = new DateTime(1995, 7, 12), PhoneNumber = "+380508901234", Email = "igor.polishchuk@example.com", SkillLevel = SkillLevel.Intermediate, RegistrationDate = DateTime.Now.AddMonths(-5) },
                new Student { FirstName = "Ольга", LastName = "Григоренко", DateOfBirth = new DateTime(1988, 12, 25), PhoneNumber = "+380509012345", Email = "olga.hryhorenko@example.com", SkillLevel = SkillLevel.Starter, RegistrationDate = DateTime.Now.AddMonths(-1) },
                new Student { FirstName = "Вікторія", LastName = "Лисенко", DateOfBirth = new DateTime(1992, 6, 8), PhoneNumber = "+380501123456", Email = "viktoria.lysenko@example.com", SkillLevel = SkillLevel.PrePerformance, RegistrationDate = DateTime.Now.AddMonths(-7) }
            };
            context.Students.AddRange(students);
            context.SaveChanges();
            
            var groups = new List<Group>
            {
                new Group { Name = "Малятка", AgeCategory = AgeCategory.Kids4_5, SkillLevel = SkillLevel.Starter, MaxCapacity = 8, Schedule = "Понеділок, Среда 16:00-16:45" },
                new Group { Name = "Зіроньки", AgeCategory = AgeCategory.Kids6_8, SkillLevel = SkillLevel.Intermediate, MaxCapacity = 10, Schedule = "Вівторок, Четвер 17:00-18:00" },
                new Group { Name = "Енергія", AgeCategory = AgeCategory.Teens, SkillLevel = SkillLevel.PrePerformance, MaxCapacity = 12, Schedule = "Понеділок, Среда, П'ятниця 18:30-19:30" },
                new Group { Name = "Грація", AgeCategory = AgeCategory.Adults, SkillLevel = SkillLevel.Intermediate, MaxCapacity = 15, Schedule = "Вівторок, Четвер 20:00-21:00" }
            };
            context.Groups.AddRange(groups);
            context.SaveChanges();

            var instructors = new List<Instructor>
            {
                new Instructor { FirstName = "Тетяна", LastName = "Дорошенко", PhoneNumber = "+380671111111", Email = "tetyana.doroshenko@danceschool.com", Specialization = "Сучасні танці, Hip-Hop", HireDate = new DateTime(2020, 9, 1) },
                new Instructor { FirstName = "Андрій", LastName = "Козлов", PhoneNumber = "+380672222222", Email = "andriy.kozlov@danceschool.com", Specialization = "Класичний танець, Джаз", HireDate = new DateTime(2019, 2, 15) },
                new Instructor { FirstName = "Вікторія", LastName = "Савенко", PhoneNumber = "+380673333333", Email = "viktoria.savenko@danceschool.com", Specialization = "Народний танець, Степ", HireDate = new DateTime(2021, 6, 10) }
            };
            context.Instructors.AddRange(instructors);
            context.SaveChanges();

            var studios = new List<Studio>
            {
                new Studio { Name = "Зала 1", Capacity = 15, FloorType = "Паркет", Equipment = "Дзеркала, балетний станок, музична система", IsAvailable = true, Notes = "Основна зала для занять" },
                new Studio { Name = "Зала 2", Capacity = 20, FloorType = "Лінолеум", Equipment = "Дзеркала, музична система, проектор", IsAvailable = true, Notes = "Зала для великих груп та виступів" }
            };
            context.Studios.AddRange(studios);
            context.SaveChanges();

            var studentGroups = new List<StudentGroup>
            {
                new StudentGroup { StudentId = 1, GroupId = 1, EnrollmentDate = DateTime.Now.AddMonths(-3) },
                new StudentGroup { StudentId = 2, GroupId = 2, EnrollmentDate = DateTime.Now.AddMonths(-6) },
                new StudentGroup { StudentId = 3, GroupId = 1, EnrollmentDate = DateTime.Now.AddMonths(-2) },
                new StudentGroup { StudentId = 4, GroupId = 2, EnrollmentDate = DateTime.Now.AddMonths(-4) },
                new StudentGroup { StudentId = 5, GroupId = 2, EnrollmentDate = DateTime.Now.AddMonths(-12) },
                new StudentGroup { StudentId = 6, GroupId = 3, EnrollmentDate = DateTime.Now.AddMonths(-8) },
                new StudentGroup { StudentId = 7, GroupId = 3, EnrollmentDate = DateTime.Now.AddMonths(-10) },
                new StudentGroup { StudentId = 8, GroupId = 4, EnrollmentDate = DateTime.Now.AddMonths(-5) },
                new StudentGroup { StudentId = 9, GroupId = 4, EnrollmentDate = DateTime.Now.AddMonths(-1) },
                new StudentGroup { StudentId = 10, GroupId = 4, EnrollmentDate = DateTime.Now.AddMonths(-7) }
            };
            context.StudentGroups.AddRange(studentGroups);
            context.SaveChanges();

            var groupInstructors = new List<GroupInstructor>
            {
                new GroupInstructor { GroupId = 1, InstructorId = 1, AssignedDate = DateTime.Now.AddMonths(-3), IsSubstitute = false },
                new GroupInstructor { GroupId = 2, InstructorId = 1, AssignedDate = DateTime.Now.AddMonths(-6), IsSubstitute = false },
                new GroupInstructor { GroupId = 2, InstructorId = 2, AssignedDate = DateTime.Now.AddMonths(-6), IsSubstitute = false },
                new GroupInstructor { GroupId = 3, InstructorId = 2, AssignedDate = DateTime.Now.AddMonths(-8), IsSubstitute = false },
                new GroupInstructor { GroupId = 4, InstructorId = 3, AssignedDate = DateTime.Now.AddMonths(-5), IsSubstitute = false }
            };
            context.GroupInstructors.AddRange(groupInstructors);
            context.SaveChanges();

            var classes = new List<Class>();
            var random = new Random();
            var classTypes = new[] { ClassType.Regular, ClassType.Technical, ClassType.OpenForParents };
            
            for (int i = 0; i < 15; i++)
            {
                var dayOffset = random.Next(-14, 7);
                var date = DateTime.Today.AddDays(dayOffset);
                var groupId = random.Next(1, 5);
                var instructorId = groupId <= 2 ? 1 : (groupId == 3 ? 2 : 3);
                var studioId = random.Next(1, 3);
                
                classes.Add(new Class
                {
                    Date = date,
                    StartTime = new TimeSpan(16 + random.Next(0, 4), 0, 0),
                    EndTime = new TimeSpan(17 + random.Next(0, 4), 0, 0),
                    ClassType = classTypes[random.Next(classTypes.Length)],
                    Topic = $"Урок {i + 1}",
                    GroupId = groupId,
                    InstructorId = instructorId,
                    StudioId = studioId
                });
            }
            context.Classes.AddRange(classes);
            context.SaveChanges();

            var performances = new List<Performance>
            {
                new Performance { Name = "Весняний концерт", PerformanceType = PerformanceType.SchoolConcert, Date = DateTime.Today.AddDays(30), Venue = "Актова зала школи", MusicTrack = "Spring Mix", CostumeRequirements = "Світлі сукні, квіти у волоссі", Notes = "Річний звітний концерт" },
                new Performance { Name = "Міський фестиваль танцю", PerformanceType = PerformanceType.Festival, Date = DateTime.Today.AddDays(45), Venue = "Палац культури", MusicTrack = "Modern Dance Suite", CostumeRequirements = "Чорні костюми з яскравими акцентами", Notes = "Участь у міському конкурсі" },
                new Performance { Name = "Новорічна казка", PerformanceType = PerformanceType.YearEndShow, Date = DateTime.Today.AddDays(60), Venue = "Театр танцю", MusicTrack = "Winter Wonderland", CostumeRequirements = "Тематичні костюми згідно з ролями", Notes = "Головна постановка року" }
            };
            context.Performances.AddRange(performances);
            context.SaveChanges();

            var subscriptions = new List<Subscription>();
            for (int i = 1; i <= 10; i++)
            {
                subscriptions.Add(new Subscription
                {
                    StudentId = i,
                    SubscriptionType = i % 2 == 0 ? SubscriptionType.Monthly : SubscriptionType.EightClasses,
                    StartDate = DateTime.Today.AddDays(-30),
                    EndDate = DateTime.Today.AddDays(30),
                    Price = i % 2 == 0 ? 1200m : 800m,
                    IsPaid = random.Next(0, 10) > 2,
                    HasSiblingDiscount = random.Next(0, 10) > 7,
                    IsFrozen = false
                });
            }
            context.Subscriptions.AddRange(subscriptions);
            context.SaveChanges();
            
            var trialLessons = new List<TrialLesson>
            {
                new TrialLesson { StudentId = 1, InstructorId = 1, Date = DateTime.Today.AddDays(-30), Status = TrialLessonStatus.Completed, CoordinationScore = 8, MusicScore = 7, TechniqueScore = 6, RecommendedGroupId = 1, Notes = "Гарні природні дані, рекомендується група для початківців" },
                new TrialLesson { StudentId = 2, InstructorId = 1, Date = DateTime.Today.AddDays(-25), Status = TrialLessonStatus.Completed, CoordinationScore = 9, MusicScore = 8, TechniqueScore = 7, RecommendedGroupId = 2, Notes = "Відмінна координація, може перейти до середньої групи" },
                new TrialLesson { StudentId = 3, InstructorId = 2, Date = DateTime.Today.AddDays(-20), Status = TrialLessonStatus.Completed, CoordinationScore = 6, MusicScore = 7, TechniqueScore = 5, RecommendedGroupId = 1, Notes = "Потребує більше практики з технікою" },
                new TrialLesson { StudentId = 8, InstructorId = 3, Date = DateTime.Today.AddDays(5), Status = TrialLessonStatus.Scheduled, Notes = "Заплановано пробний урок для дорослого" },
                new TrialLesson { StudentId = 9, InstructorId = 3, Date = DateTime.Today.AddDays(-5), Status = TrialLessonStatus.Cancelled, Notes = "Скасовано через хворобу" }
            };
            context.TrialLessons.AddRange(trialLessons);
            context.SaveChanges();

            var performanceStudents = new List<PerformanceStudent>
            {
                new PerformanceStudent { PerformanceId = 1, StudentId = 1, Role = "Квітка" },
                new PerformanceStudent { PerformanceId = 1, StudentId = 2, Role = "Метелик" },
                new PerformanceStudent { PerformanceId = 1, StudentId = 3, Role = "Сонечко" },
                new PerformanceStudent { PerformanceId = 2, StudentId = 5, Role = "Соло танцівниця" },
                new PerformanceStudent { PerformanceId = 2, StudentId = 6, Role = "Головний танцівник" },
                new PerformanceStudent { PerformanceId = 2, StudentId = 7, Role = "Партнерка" },
                new PerformanceStudent { PerformanceId = 3, StudentId = 8, Role = "Снігова королева" },
                new PerformanceStudent { PerformanceId = 3, StudentId = 9, Role = "Фея зими" },
                new PerformanceStudent { PerformanceId = 3, StudentId = 10, Role = "Сніжинка" }
            };
            context.PerformanceStudents.AddRange(performanceStudents);
            context.SaveChanges();

            var attendances = new List<Attendance>();
            var attendanceStatuses = new[] { AttendanceStatus.Present, AttendanceStatus.Absent, AttendanceStatus.Frozen };
            var weights = new[] { 0.8, 0.15, 0.05 };
            
            foreach (var cls in context.Classes.Where(c => c.Date <= DateTime.Today).ToList())
            {
                var groupStudents = context.StudentGroups.Where(sg => sg.GroupId == cls.GroupId).ToList();
                
                foreach (var sg in groupStudents)
                {
                    var rand = random.NextDouble();
                    var status = AttendanceStatus.Present;
                    
                    if (rand > weights[0]) 
                        status = rand > weights[0] + weights[1] ? AttendanceStatus.Frozen : AttendanceStatus.Absent;
                    
                    attendances.Add(new Attendance
                    {
                        StudentId = sg.StudentId,
                        ClassId = cls.Id,
                        Status = status,
                        AbsentReason = status == AttendanceStatus.Absent ? "Хвороба" : null,
                        Notes = status == AttendanceStatus.Present ? "Активно займався" : null
                    });
                }
            }
            context.Attendances.AddRange(attendances);
            context.SaveChanges();
        }
    }
}