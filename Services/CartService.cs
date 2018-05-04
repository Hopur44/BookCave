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
        public void InsertToCart(CartViewModel model, int id)
        {
            /* 
            if(!_db.Cart.Any()) {

            }
            */
            /*
            var quantity = (from c in _db.Cart
            where c.Id == id && model.ItemId == c.BookId 
            select c.Quantity).SingleOrDefault();
            Console.WriteLine("I'm inserting to cart table in database");
            Console.WriteLine("quantity is: " + quantity);
            */
            /*
            foreach( var item in quantity)
            {
                Console.WriteLine("this is inside quantity");
                Console.WriteLine(item);
            }
            */
            //quantity++;

            /*
            if(quantity == 0) 
            {
                var newItemInCart = new CartEntityModel
                {
                    AccountId = id,
                    BookId = model.ItemId,
                    Quantity = 1,
                    Finished = false
                };
                _db.Add(newItemInCart);

            } 
            else 
            {
                //int breyta = quantity.Count() + 1;
                var newItemInCart = new CartEntityModel
                {
                    AccountId = id,
                    BookId = model.ItemId,
                    Quantity = 99,
                    Finished = false
                };
                _db.Update(newItemInCart);
            }
            
            
            
            _db.SaveChanges();
            */
        }
    }
}
