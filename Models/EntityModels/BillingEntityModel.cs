namespace BookCave.Models.EntityModels
{
    public class BillingEntityModel
    {
        public int Id {get; set;}
        public int AccountId{get;set;}
        public string StreetAddress {get; set;}

        public string  City {get; set;}

        public string Country {get; set;}

        public int ZipCode {get; set;}
    }
}