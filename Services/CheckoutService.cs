using System.Collections.Generic;
using BookCave.Data;
using System.Linq;
using BookCave.Models.EntityModels;
using BookCave.Models.InputModels;
using BookCave.Models.ViewModels;

namespace BookCave.Services
{
    public class CheckoutService
    {
        private DataContext _db;
        public CheckoutService()
        {
            _db = new DataContext();
        }
        public int GetBillingId(int accountId)
        {
            return (from b in _db.Billings
                    where b.AccountId == accountId 
                    select b.Id).FirstOrDefault();
        }

        public bool BillingIdExist(int accountId)
        {
            return (from b in _db.Billings
                    where b.AccountId == accountId 
                    select b.Id).Any();
        }

        public OrderViewModel GetReviewOrder(BillingInputModel billing, List<CartViewModel> userCart)
        {
            return new OrderViewModel
            {
                CartList = userCart,
                Billing = billing
            };
        }

        public void CreateBilling(BillingInputModel billing, int accountId)
        {
            if(BillingIdExist(accountId))
            {
                var billingInput = new BillingEntityModel
                {
                    Id = GetBillingId(accountId),
                    AccountId = accountId,
                    StreetAddress = billing.StreetAddress,
                    City = billing.City,
                    Country = billing.Country,
                    ZipCode = billing.ZipCode
                };
                _db.Update(billingInput);               
            }
            else
            {
                var billingInput = new BillingEntityModel
                {
                    AccountId = accountId,
                    StreetAddress = billing.StreetAddress,
                    City = billing.City,
                    Country = billing.Country,
                    ZipCode = billing.ZipCode
                };
            _db.Add(billingInput);
            }
            _db.SaveChanges();
        }
        public void CreateOrder(List<CartViewModel> cart, int accountId, int billingId)
        {
            foreach(var item in cart)
            {
                var newOrder = new OrderEntityModel
                {
                    CustomerId = accountId,
                    BookId = item.ItemId,
                    Quantity = item.Quantity,
                    BillingId = billingId,
                    Price = item.Quantity * item.Price
                };
                _db.Add(newOrder);
                //required to either delete the Cart or changes cart Finished to True
            }
            _db.SaveChanges();
        }
    }
}