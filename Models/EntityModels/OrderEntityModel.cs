namespace BookCave.Models.EntityModels
{
    public class OrderEntityModel
    {
        public int Id {get; set;}

        public int BillingId {get; set;}

        public int CustomerId {get; set;}

        public int BookId {get; set;}

        public int Quantity {get; set;}
        public int Price {get; set;}
    }
}