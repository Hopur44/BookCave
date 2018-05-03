using BookCave.Data;
using System.Linq;
using BookCave.Models.ViewModels;
using System.Collections.Generic;

namespace BookCave.Services
{
    public class BookService
    {
        private DataContext _db;
        public BookService()
        {
            _db = new DataContext();
        }

        public List<BookViewModel> GetAllBooks()
        {
            var books = (from b in _db.Books
                            select new BookViewModel
                            {
                                Id = b.Id,
                                Title = b.Title,
                                Price = b.Price,
                                Rating = 5,
                                Image = b.ImageLink,
                                Author = b.Author
                            }).ToList();
            return books;
        }
    }
}
