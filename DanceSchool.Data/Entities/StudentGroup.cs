using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceSchool.Data.Entities
{
    public class StudentGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
    }
}