using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DanceSchool.Data.Enums;

namespace DanceSchool.Data.Entities
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public ClassType ClassType { get; set; }

        [MaxLength(200)]
        public string? Topic { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public int StudioId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public Instructor Instructor { get; set; }

        [ForeignKey(nameof(StudioId))]
        public Studio Studio { get; set; }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}