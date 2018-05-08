namespace BookCave.Models.EntityModels
{
    public class BillingEntityModel
    {
        public int Id {get; set;}
        public int AccountId{get;set;}
        public string StreetAddress {get; set;}
        public string  City {get; set;}
        public string Country {get; set;}
        public int? ZipCode {get; set;}
        public bool Finished{get; set;}
        public string CardOwner{get; set;}
        public string CardNumber{get; set;}
        public string CvCode {get; set;}
        public string ExpireDate {get; set;}
    }
}