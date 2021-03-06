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
        private DataContext reviewDB;
        public BookService()
        {
            _db = new DataContext();
            reviewDB = new DataContext();
        }
        public bool PostBookReview(ReviewInputModel review, int accountId)
        {
            var accountIdexist = (from r in _db.Reviews
            where r.CustomerId == accountId && r.BookId == review.Id
            select r.CustomerId).Any();
            if(accountIdexist)
            {
                return true;
            }
            var newReview = new ReviewEntityModel
            {
                BookId = review.Id,
                CustomerId = accountId,
                Rating = review.Rating,
                Comment = review.Comment
            };
            UpdateBookRating(review.Id, review.Rating);
            _db.Add(newReview);
            _db.SaveChanges();
            return false;
        }
        public void UpdateBookRating(int bookId, int rating)
        {
            var bookRating = (from r in _db.Reviews
                        where r.BookId == bookId
                        select r.Rating).ToList();

            bookRating.Add(rating);

            var newRating = Convert.ToInt32(bookRating.Average());
            var book = (from b in _db.Books
                        where b.Id == bookId
                        select new BookEntityModel
                        {
                            Id = b.Id,
                            Price = b.Price,
                            Title = b.Title,
                            Genre = b.Genre,
                            Description = b.Description,
                            Rating = newRating,
                            Author = b.Author,
                            ImageLink = b.ImageLink, 
                            PublishDate = b.PublishDate,
                            PageNumber = b.PageNumber
                        }).SingleOrDefault();
                        _db.Update(book);
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
                                Rating = b.Rating,
                                Image = b.ImageLink,
                                Author = b.Author
                            }).OrderBy(b => b.Title).ToList();
            return books;
        }
        public int GetAverageRatingOfBook(int? bookId)
        {
            var rating = (from r in reviewDB.Reviews
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
                            Rating = b.Rating,
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
                        Rating = b.Rating,
                        Author = b.Author
                    }).ToList();
            
            return books.OrderByDescending(b => b.Rating).Take(n).ToList();

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
                        Author = b.Author,
                        Rating = b.Rating
                    }).ToList();
            return books;
        }
    }
}
