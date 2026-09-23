using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class Volunteer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Skills { get; set; } = string.Empty;

        [Required]
        public string Availability { get; set; } = string.Empty;

        public DateTime DateRegistered { get; set; } = DateTime.Now;
    }
}