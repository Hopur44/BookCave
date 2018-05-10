namespace BookCave.Models.ViewModels
{
    public class ReviewViewModel
    {
        public int BookID { get; set; }
        public int CustomerID { get; set; }

        public string CustomerName {get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

    }
}