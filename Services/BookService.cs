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
        public List<BookViewModel> GetBooksByString(string SearchString)
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
                        if(!string.IsNullOrEmpty(SearchString))
                        {
                            books = books.Where(b => b.Title.ToLower().Contains(SearchString.ToLower()) || b.Author.ToLower().Contains(SearchString.ToLower())).ToList();
                                  
                       }
            return books;
        }
        public BookViewModel GetBooksByID(int id)
        {
            var book = (from b in _db.Books
            where b.Id == id
                select new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    Rating = 5,
                    Image = b.ImageLink,
                    Author = b.Author
                }).SingleOrDefault();
            return book;
        }
        
    }
}
