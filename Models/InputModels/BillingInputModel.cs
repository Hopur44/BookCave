using System.ComponentModel.DataAnnotations;

namespace BookCave.Models.InputModels
{
    public class BillingInputModel
    {
        [Required]
         public string StreetAddress {get; set;}
        [Required]
        public string  City {get; set;}
        [Required]
        public string Country {get; set;}
        [Required]
        public int ZipCode {get; set;}
    }
}