namespace BookCave.Models.ViewModels
{
    public class ReviewViewModel
    {
        public int bookID { get; set; }
        public int customerID { get; set; }
        public int rating { get; set; }
        public long comment { get; set; }
    }
}