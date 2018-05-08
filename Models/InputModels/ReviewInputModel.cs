using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookCave.Models.ViewModels;
using Newtonsoft.Json;

namespace BookCave.Models.InputModels
{
    public class ReviewInputModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id {get; set;}

        [JsonProperty(PropertyName = "rating")]
        public int Rating {get; set;}  

        [JsonProperty(PropertyName = "comment")]
        public string Comment {get; set;}
    }
}