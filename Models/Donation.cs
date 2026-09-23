using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class Donation
    {
        public int Id { get; set; }

        [Display(Name = "Donor Name")]
        public string? DonorName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Range(1, 100000000)]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "ZAR";

        [Required]
        [Display(Name = "Donation Type")]
        public string DonationType { get; set; } = "One-time";

        [Display(Name = "Anonymous Donation")]
        public bool IsAnonymous { get; set; }

        public DateTime DonationDate { get; set; } = DateTime.Now;
    }
}