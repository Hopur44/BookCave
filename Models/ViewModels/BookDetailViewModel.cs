using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookCave.Models.InputModels;

namespace BookCave.Models.ViewModels
{
    public class BookDetailViewModel
    {
        public int Id {get; set;}

        public string Image {get; set;}

        public string Title {get; set;}

        public int Price {get; set;}
        [Required]
        public int Rating {get; set;}
        public string Author {get; set;}
        public string Description { get; set; }

        public string PublishDate { get; set; }
        public int NumberOfPage { get; set; }
        public List<ReviewViewModel> ReviewList {get; set;}
        [Required]
        public string Comment {get; set;}
    }
}