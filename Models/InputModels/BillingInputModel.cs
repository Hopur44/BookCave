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
        public int? ZipCode {get; set;}
        [Required]
        public string CardOwner{get; set;}
        [Required]
        [RegularExpression("^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$",
        ErrorMessage = "Only MasterCard(it uses 16 digits and starts with numbers 51 through 55 or with the numbers 2221 through 2720.)")]
        public string CardNumber{get; set;}
        [Required]
        [RegularExpression("^([1-8][0-9]{2}|9[0-8][0-9]|99[0-9])$", ErrorMessage = "must be a number between 100-999")]
        public string CvCode {get; set;}
        [Required]
        public string ExpireDate {get; set;}
    }
}