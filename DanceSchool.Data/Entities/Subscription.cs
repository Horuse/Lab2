using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DanceSchool.Data.Enums;

namespace DanceSchool.Data.Entities
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public SubscriptionType SubscriptionType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        public bool IsPaid { get; set; }

        [Required]
        public bool HasSiblingDiscount { get; set; }

        [Required]
        public bool IsFrozen { get; set; }

        public DateTime? FrozenUntil { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }
    }
}