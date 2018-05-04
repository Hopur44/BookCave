using Newtonsoft.Json;

namespace BookCave.Models.ViewModels
{
    public class CartViewModel
    {   
        [JsonProperty(PropertyName = "itemId")]
        public int ItemId { get; set; }
        
        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }
        
        [JsonProperty(PropertyName = "price")]
        public int Price { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}