using BookCave.Data;
using BookCave.Models.EntityModels;
using BookCave.Models.InputModels;

namespace BookCave.Services
{
    public class CheckoutService
    {
        private DataContext _db;
        public CheckoutService()
        {
            _db = new DataContext();
        }
        public void CreateBilling(BillingInputModel billing)
        {
            var billingInput = new BillingEntityModel
            {
                StreetAddress = billing.StreetAddress,
                City = billing.City,
                Country = billing.Country,
                ZipCode = billing.ZipCode
            };
            _db.Add(billingInput);
            _db.SaveChanges();
        }
    }
}