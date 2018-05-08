using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Services;
using Microsoft.AspNetCore.Authorization;
using BookCave.Models.InputModels;
using BookCave.Models.ViewModels;
using System.Security.Claims;

namespace BookCave.Controllers
{
    public class HomeController : Controller
    {
        private BookService _bookService;
        private AccountService _accountService;
        public HomeController()
        {
            _bookService = new BookService();
            _accountService = new AccountService();
        }
        public IActionResult Index(string SearchString)
        {
            var books = _bookService.GetBooksByString(SearchString);
            return View(books);
        }
        [HttpGet]
        public IActionResult Details(int? Id)
        {
            var book = _bookService.GetBooksByID(Id);
            return View(book);
        }
        /*
        [HttpPost]
        public IActionResult Details(ReviewInputModel review)
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
           int accountId = _accountService.GetAccountId(email);

            _bookService.PostBookReview(review, accountId);
            return RedirectToAction("Details", review.Id);
        }
        */

        // breyta reviewInputModel-inu til að taka json from body...
        // gera eins og ég gerði áður...
        [HttpPost]
        public IActionResult AddComment([FromBody] ReviewInputModel review)
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            int accountId = _accountService.GetAccountId(email);
            Console.WriteLine("did we go here? " + review.Id + " " + review.Rating + " " + review.Comment);
            
            _bookService.PostBookReview(review, accountId);
            //var item = true;

            var obj = new { user = email, comment = review.Comment, rating = review.Rating  };
            return Json(obj);
        }

        [HttpPost]
        public IActionResult AllReviews([FromBody] int id)
        {   


            /*
            string email = ((ClaimsIdentity) User.Identity).Name;
            int accountId = _accountService.GetAccountId(email);
            */
            //Console.WriteLine("did we go here? " + id);
            
            //_bookService.PostBookReview(review, accountId);
            var bookReviews = _bookService.GetReviewsByBookID(id);
            // sækja öll comment fyrir þessa bók..
            // og skila þeim

            //var item = true;

            //var obj = new { user = "someuser", comment = "ss", rating = 22  };
            return Json(bookReviews);
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult TopTen()
        {
            var topTenBooks = _bookService.GetTopBooks(10);
            return View(topTenBooks);
        }
        [HttpGet]
        public IActionResult FilterByGenre([FromQuery] string genre)
        {
            var genreBooks = _bookService.GetBooksByGenre(genre);
            return Json(genreBooks);
        }

        
        

    }
}
