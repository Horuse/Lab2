using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DanceSchool.Data.Enums;

namespace DanceSchool.Data.Entities
{
    public class TrialLesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TrialLessonStatus Status { get; set; }

        public int? CoordinationScore { get; set; }

        public int? MusicScore { get; set; }

        public int? TechniqueScore { get; set; }

        public int? RecommendedGroupId { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public Instructor Instructor { get; set; }

        [ForeignKey(nameof(RecommendedGroupId))]
        public Group? RecommendedGroup { get; set; }
    }
}