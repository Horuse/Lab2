using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DanceSchool.Data.Enums;

namespace DanceSchool.Data.Entities
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public AttendanceStatus Status { get; set; }

        [MaxLength(200)]
        public string? AbsentReason { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }
    }
}