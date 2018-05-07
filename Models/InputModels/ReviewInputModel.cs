using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookCave.Models.ViewModels;

namespace BookCave.Models.InputModels
{
    public class ReviewInputModel
    {
        public int Id {get; set;}

        public int Rating {get; set;}        
        
        public string Comment {get; set;}
    }
}