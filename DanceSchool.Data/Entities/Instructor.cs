using System.ComponentModel.DataAnnotations;

namespace DanceSchool.Data.Entities
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Specialization { get; set; }

        [Required]
        public DateTime HireDate { get; set; }
    }
}