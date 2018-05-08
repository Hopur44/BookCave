using System.ComponentModel.DataAnnotations;
namespace BookCave.Models.InputModels
{
    public class RegisterInputModel
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        public string FirstName {get; set;}
        [Required]
        public string LastName {get; set;}
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$",
         ErrorMessage = "Must be atleast 8 characters, one lowercase, one uppercase and 1 number")]
        public string Password {get; set;}
        public string FavouriteBook {get; set;}
        public string Image {get; set;}
        public string Address {get; set;}
    }
}