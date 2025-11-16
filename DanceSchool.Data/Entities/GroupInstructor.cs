using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceSchool.Data.Entities
{
    public class GroupInstructor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; }

        public bool IsSubstitute { get; set; }

        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public Instructor Instructor { get; set; }
    }
}