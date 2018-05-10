using System.ComponentModel.DataAnnotations;

namespace BookCave.Models.InputModels
{
    public class LoginInputModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Required valid email address something@something.com")]
        public string Email {get; set;}
        [Required]
        public string Password {get; set;}

        public bool RememberMe {get; set;}
    }
}