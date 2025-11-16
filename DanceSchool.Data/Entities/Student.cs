using System.ComponentModel.DataAnnotations;
using DanceSchool.Data.Enums;

namespace DanceSchool.Data.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? ParentContact { get; set; }

        [Required]
        public SkillLevel SkillLevel { get; set; }

        public string? HealthNotes { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<TrialLesson> TrialLessons { get; set; } = new List<TrialLesson>();
        public ICollection<PerformanceStudent> PerformanceStudents { get; set; } = new List<PerformanceStudent>();
    }
}