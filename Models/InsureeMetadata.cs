using System;
using System.ComponentModel.DataAnnotations;

namespace CarInsurance.Models
{
    [MetadataType(typeof(InsureeMetadata))]
    public partial class Insuree
    {
    }

    public class InsureeMetadata
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Car Year")]
        public int CarYear { get; set; }

        [Required]
        [Display(Name = "Car Make")]
        public string CarMake { get; set; }

        [Required]
        [Display(Name = "Car Model")]
        public string CarModel { get; set; }

        [Required]
        [Display(Name = "DUI?")]
        public bool DUI { get; set; }

        [Required]
        [Display(Name = "Speeding Tickets")]
        public int SpeedingTickets { get; set; }

        [Required]
        [Display(Name = "Coverage Type (Full Coverage?)")]
        public bool CoverageType { get; set; }

        [Display(Name = "Monthly Quote")]
        public decimal Quote { get; set; }
    }
}
