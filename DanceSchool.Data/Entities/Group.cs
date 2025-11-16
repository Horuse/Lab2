using System.ComponentModel.DataAnnotations;
using DanceSchool.Data.Enums;

namespace DanceSchool.Data.Entities
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public AgeCategory AgeCategory { get; set; }

        [Required]
        public SkillLevel SkillLevel { get; set; }

        [Required]
        public int MaxCapacity { get; set; }

        [MaxLength(200)]
        public string? Schedule { get; set; }
    }
}