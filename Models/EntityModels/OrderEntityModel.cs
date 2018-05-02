namespace BookCave.Models.EntityModels
{
    public class OrderEntityModel
    {
        public int Id {get; set;}

        public int BillingId {get; set;}

        public int CustomerId {get; set;}

        public int BookId {get; set;}
    }
}