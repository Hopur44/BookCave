namespace BookCave.Models.EntityModels
{
    public class ReviewEntityModel
    {
        public int Id {get; set;}

        public int CustomerId {get; set;}

        public int BookId {get; set;}

        public int Rating {get; set;}

        public string Comment {get; set;}
    }
}