using BookCave.Data;
using System.Linq;
using BookCave.Models.ViewModels;
using BookCave.Models.EntityModels;
using System.Collections.Generic;
using System;

namespace BookCave.Services
{
    public class CartService
    {
        private DataContext _db;
        public CartService()
        {
            _db = new DataContext();
        }

        public List<CartViewModel> GetUserCartItems(int userId)
        {
            var userCart = (from c in _db.Cart
            join b in _db.Books on c.BookId equals b.Id 
            where c.AccountId == userId
                select new CartViewModel
                {
                    ItemId = c.BookId,
                    Title = b.Title,
                    Price = b.Price,
                    Quantity = c.Quantity
                }).ToList();
            return userCart;
        }
        public CartViewModel GetBooksByID(int id)
        {
            var book = (from b in _db.Books
            where b.Id == id
                select new CartViewModel
                {
                    ItemId = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                }).SingleOrDefault();
            return book;
        }

        public int FindQuanity(int accountId, int bookId)
        {
            return (from c in _db.Cart
            where c.AccountId == accountId && c.BookId == bookId && c.Finished == false
            select c.Quantity).FirstOrDefault();
        }

        public int FindCartId(int accountId, int bookId)
        {
            return (from c in _db.Cart
            where c.AccountId == accountId && c.BookId == bookId && c.Finished == false
            select c.Id).FirstOrDefault();
        }
        public void RemoveCart(CartViewModel model, int accountId)
        {
            int bookId = model.ItemId;
            var cartId = FindCartId(accountId, bookId);
            var updateCart = new CartEntityModel
            {
                Id = cartId,
                AccountId = accountId,
                BookId = model.ItemId,
                Quantity = model.Quantity,
                Finished = true
            };
            _db.Remove(updateCart);
            _db.SaveChanges();
        }
            
        public void InsertToCart(CartViewModel model, int accountId)
        {
            var quantity = FindQuanity(accountId,model.ItemId);
            Console.WriteLine("I'm inserting to cart table in database");
            Console.WriteLine("quantity is: " + quantity);
            
            if(quantity == 0) 
            {
                var newItemInCart = new CartEntityModel
                {
                    AccountId = accountId,
                    BookId = model.ItemId,
                    Quantity = 1,
                    Finished = false
                };
                _db.Add(newItemInCart);

            }
            else 
            {
                var cartId = FindCartId(accountId,model.ItemId);
                quantity++;
                var newItemInCart = new CartEntityModel
                {
                    Id = cartId,
                    AccountId = accountId,
                    BookId = model.ItemId,
                    Quantity = quantity,
                    Finished = false
                };
                _db.Update(newItemInCart);
            }
            _db.SaveChanges();
        }
        public void RemoveOneFromCart(CartViewModel model, int accountId)
        {
            //var quantity =  FindQuanity(accountId,model.ItemId);
            var quantity = model.Quantity;
            var cartId = FindCartId(accountId,model.ItemId);
            if(quantity == 0)
            {
                return;
            }
            Console.WriteLine("quantity is: " + quantity);
            
                quantity--;
                var newItemInCart = new CartEntityModel
                {
                    Id = cartId,
                    AccountId = accountId,
                    BookId = model.ItemId,
                    Quantity = quantity,
                    Finished = false
                };
                if(quantity == 0)
                {
                    _db.Remove(newItemInCart);
                }
                else
                {
                    _db.Update(newItemInCart);
                }
            _db.SaveChanges();
        }
        public int GetTotalCartItems(List<CartViewModel> model)
        {
            return (from c in model
                    select c.Quantity).Sum();
        }
        public void InsertAllItems(List<CartViewModel> model, int accountId)
        {
            foreach(var item in model)
            {
                var cartItem = new CartEntityModel
                {
                    AccountId = accountId,
                    BookId = item.ItemId,
                    Quantity = item.Quantity,
                    Finished = false
                };
                _db.Add(cartItem);
            }
            _db.SaveChanges();
        }


    }
}