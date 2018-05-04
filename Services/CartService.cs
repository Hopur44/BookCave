using BookCave.Data;
using System.Linq;
using BookCave.Models.ViewModels;
using System.Collections.Generic;

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
        
    }
}
