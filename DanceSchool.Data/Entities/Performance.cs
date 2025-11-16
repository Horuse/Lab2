using System.ComponentModel.DataAnnotations;
using DanceSchool.Data.Enums;

namespace DanceSchool.Data.Entities
{
    public class Performance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public PerformanceType PerformanceType { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(200)]
        public string? Venue { get; set; }

        [MaxLength(200)]
        public string? MusicTrack { get; set; }

        [MaxLength(500)]
        public string? CostumeRequirements { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}