using BookCave.Data;
using System.Linq;
using BookCave.Models.ViewModels;
using System.Collections.Generic;
using BookCave.Models.InputModels;
using BookCave.Models.EntityModels;
using System;

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
                Comment = review.Comment
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
                                Rating = GetAverageRatingOfBook(b.Id),
                                Image = b.ImageLink,
                                Author = b.Author
                            }).OrderBy(b => b.Title).ToList();
            return books;
        }
        public int GetAverageRatingOfBook(int? bookId)
        {
            var rating = (from r in _db.Reviews
            where r.BookId == bookId
            select r.Rating).ToList();
            if(rating.Count() == 0)
            {
                return 0;
            }
            return Convert.ToInt32(rating.Average());
        }
        public List<BookViewModel> GetBooksByString(string SearchString)
        {
            var books = (from b in _db.Books
                        select new BookViewModel
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Price = b.Price,
                            Image = b.ImageLink,
                            Author = b.Author,
                            Genre = b.Genre
                        }).ToList();
            /*
            foreach (var item in books)
            {
                item.Rating = GetAverageRatingOfBook(item.Id);
            }
            */
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
            var reviewList = GetReviewsByBookID(id);
            var bookRating = GetAverageRatingOfBook(id);
            
            var book = (from b in _db.Books
            where b.Id == id
                select new BookDetailViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    Description = b.Description,
                    Image = b.ImageLink,
                    Author = b.Author,
                    NumberOfPage = b.PageNumber,
                    ReviewList = reviewList,
                    Rating = bookRating,
                    PublishDate = b.PublishDate
                }).SingleOrDefault(); 
            return book;
        }
        public List<ReviewViewModel> GetReviewsByBookID(int? id)
        {
            var reviews = (from r in _db.Reviews
            join c in _db.Accounts on r.CustomerId equals c.Id
            where r.BookId == id
            select new ReviewViewModel
            {
                BookID = r.BookId,
                CustomerID = r.CustomerId,
                CustomerName = c.Name,
                Rating = r.Rating,
                Comment = r.Comment
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
                        Image = b.ImageLink,
                        Author = b.Author
                    }).OrderByDescending(b => b.Rating).Take(n).ToList();
            foreach (var item in books)
            {
                item.Rating = GetAverageRatingOfBook(item.Id);
            }

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
                        Image = b.ImageLink,
                        Author = b.Author
                    }).ToList();
            foreach (var item in books)
            {
                item.Rating = GetAverageRatingOfBook(item.Id);
            }
            return books;
        }
    }
}
