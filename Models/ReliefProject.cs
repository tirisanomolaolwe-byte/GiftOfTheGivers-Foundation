using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class ReliefProject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public string Status { get; set; } = "Ongoing";

        [Display(Name = "Date Started")]
        [DataType(DataType.Date)]
        public DateTime DateStarted { get; set; } = DateTime.Now;

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}