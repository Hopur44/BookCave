using System;

namespace BookCave.Models.EntityModels
{
    public class BookEntityModel
    {
        public int Id {get; set;}

        public int Price {get; set;}

        public string Title {get; set;}

        public string Genre {get; set;}

        public string Description {get; set;}

        public int Rating{get; set;}

        public string Author {get; set;}

        public string ImageLink {get; set;}

        public string PublishDate {get; set;}

        public int PageNumber {get; set;}
    }
}