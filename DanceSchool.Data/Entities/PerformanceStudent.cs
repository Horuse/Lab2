using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceSchool.Data.Entities
{
    public class PerformanceStudent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PerformanceId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [MaxLength(100)]
        public string? Role { get; set; }

        [ForeignKey(nameof(PerformanceId))]
        public Performance Performance { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }
    }
}