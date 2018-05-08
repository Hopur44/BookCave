using BookCave.Data;
using System.Linq;
using BookCave.Models.ViewModels;
using System.Collections.Generic;
using BookCave.Models.InputModels;
using BookCave.Models.EntityModels;

namespace BookCave.Services
{
    public class BookService
    {
        private DataContext _db;
        public BookService()
        {
            _db = new DataContext();
        }
        public void PostBookReview(ReviewInputModel review, int accountId)
        {
            var newReview = new ReviewEntityModel
            {
                BookId = review.Id,
                CustomerId = accountId,
                Rating = review.Rating,
                comment = review.Comment
            };
            _db.Add(newReview);
            _db.SaveChanges();
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
                            }).OrderBy(b => b.Title).ToList();
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
                                Author = b.Author,
                                Genre = b.Genre
                            }).ToList();
                        if(!string.IsNullOrEmpty(SearchString))
                        {
                            books = books.Where(b => b.Title.ToLower().Contains(SearchString.ToLower()) 
                            || b.Author.ToLower().Contains(SearchString.ToLower()) 
                            || b.Genre.ToLower().Contains(SearchString.ToLower())).ToList();      
                       }
            return books;
        }
        public BookDetailViewModel GetBooksByID(int? id)
        {
            var book = (from b in _db.Books
            where b.Id == id
                select new BookDetailViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    Rating = 5,
                    Image = b.ImageLink,
                    Author = b.Author,
                    Description = b.Description,
                    ReviewList = GetReviewsByBookID(id)
                }).SingleOrDefault(); 
            return book;
        }
        public List<ReviewViewModel> GetReviewsByBookID(int? id)
        {
            var reviews = (from r in _db.Reviews
            where r.BookId == id
            select new ReviewViewModel
            {
                bookID = r.BookId,
                customerID = r.CustomerId,
                rating = r.Rating,
                comment = r.comment
            }).ToList();
            return reviews;
        }  
        public List<BookViewModel> GetTopBooks(int n)
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
                    }).OrderByDescending(b => b.Rating).Take(n).ToList();

            return books;

        }
        public List<BookViewModel> GetBooksByGenre(string genre)
        {
             var books = (from b in _db.Books
                where b.Genre == genre
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
