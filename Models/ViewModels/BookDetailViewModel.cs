using System.Collections.Generic;

namespace BookCave.Models.ViewModels
{
    public class BookDetailViewModel
    {
        public int Id {get; set;}

        public string Image {get; set;}

        public string Title {get; set;}

        public int Price {get; set;}

        public int Rating {get; set;}

        public string Author {get; set;}
        public long Description { get; set; }
        public string Publisher { get; set; }
        public int NumberOfPage { get; set; }
        public List<ReviewViewModel> ReviewList {get; set;}
    }
}