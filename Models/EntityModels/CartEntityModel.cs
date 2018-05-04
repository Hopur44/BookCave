namespace BookCave.Models.EntityModels
{
    public class CartEntityModel
    {
        public int Id {get; set;}

        public int AccountId {get; set;}

        public int BookId {get; set;}

        public bool Finished {get; set;}

        public int Quantity {get; set;}
    }
}