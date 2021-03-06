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
        private CartService _cartService;
        public CheckoutService()
        {
            _db = new DataContext();
            _cartService = new CartService();
        }
        public int GetBillingId(int accountId)
        {
            return (from b in _db.Billings
                    where b.AccountId == accountId && b.Finished == false
                    select b.Id).FirstOrDefault();
        }
        
        public bool BillingIdExist(int accountId)
        {
            return (from b in _db.Billings
                    where b.AccountId == accountId && b.Finished == false
                    select b.Id).Any();
        }

        public bool OrderExist(int accountId)
        {
            return (from b in _db.Billings
                    where b.AccountId == accountId && b.Finished == true
                    select b.Id).Any();

        }
        public BillingInputModel GetBilling(int billingId)
        {
            return (from b in _db.Billings
                    where b.Id == billingId
                    select new BillingInputModel
                    {
                        StreetAddress = b.StreetAddress,
                        City = b.City,
                        Country = b.Country,
                        ZipCode = b.ZipCode,
                        CardNumber = b.CardNumber,
                        CardOwner = b.CardOwner,
                        CvCode = b.CvCode,
                        ExpireYear = b.ExpireDate.Substring(3,2),
                        ExpireMonth = b.ExpireDate.Substring(0,2)
                    }).FirstOrDefault();
        }
        //Gets the order to display during checkout
        public OrderViewModel GetReviewOrder(BillingInputModel billing, List<CartViewModel> userCart)
        {
            var price = new List<int>();
            foreach (var item in userCart)
            {
                if(item.Quantity != 1)
                {
                    price.Add(item.Price * item.Quantity);
                }
                else
                {
                    price.Add(item.Price);                    
                }

            }
            return new OrderViewModel
            {
                CartList = userCart,
                Billing = billing,
                TotalPrice = price.Sum()
            };
        }
        //function creates a new billing
        public void CreateBillingHelperFunction(BillingInputModel billing, int accountId)
        {
            var billingInput = new BillingEntityModel
                {
                    AccountId = accountId,
                    StreetAddress = billing.StreetAddress,
                    City = billing.City,
                    Country = billing.Country,
                    ZipCode = billing.ZipCode,
                    Finished = false,
                    CardOwner = billing.CardOwner,
                    CardNumber = billing.CardNumber,
                    ExpireDate = billing.ExpireMonth+"/"+billing.ExpireYear,
                    CvCode = billing.CvCode
                };
            _db.Add(billingInput);
            _db.SaveChanges();
        }

        public void CreateBilling(BillingInputModel billing, int accountId)
        {
            if(BillingIdExist(accountId))
            {
                //check wether the the existing billing has an order attached to it
                // if it doesnt exist built a new billing
                if(!OrderExist(accountId))
                {
                    CreateBillingHelperFunction(billing, accountId);
                }
                //else it updates the the old billing
                else
                {
                    var billingInput = new BillingEntityModel
                    {
                        Id = GetBillingId(accountId),
                        AccountId = accountId,
                        StreetAddress = billing.StreetAddress,
                        City = billing.City,
                        Country = billing.Country,
                        Finished = false,
                        ZipCode = billing.ZipCode,
                        CardOwner = billing.CardOwner,
                        CardNumber = billing.CardNumber,
                        ExpireDate = billing.ExpireMonth+"/"+billing.ExpireYear,
                        CvCode = billing.CvCode
                    };
                    _db.Update(billingInput);
                    _db.SaveChanges();    
                }     
            }
            else
            {
                CreateBillingHelperFunction(billing,accountId);
            }         
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
                //deletes the cart
                _cartService.RemoveCart(item,accountId);
            }
            var billing = GetBilling(billingId);
            //updates the cart to finished so this billing wont be changed
            var billingInput = new BillingEntityModel
            {
                Id = billingId,
                AccountId = accountId,
                StreetAddress = billing.StreetAddress,
                City = billing.City,
                Country = billing.Country,
                Finished = true,
                ZipCode = billing.ZipCode,
                CardOwner = billing.CardOwner,
                CardNumber = billing.CardNumber,
                ExpireDate = billing.ExpireMonth+"/"+billing.ExpireYear,
                CvCode = billing.CvCode
            };
            _db.Update(billingInput);
            _db.SaveChanges();
        }
    }
}