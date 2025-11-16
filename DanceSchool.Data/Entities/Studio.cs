using System.ComponentModel.DataAnnotations;

namespace DanceSchool.Data.Entities
{
    public class Studio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Capacity { get; set; }

        [MaxLength(50)]
        public string? FloorType { get; set; }

        [MaxLength(500)]
        public string? Equipment { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}