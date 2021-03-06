﻿using System;
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
            if(TempData["alert"] != null)
            {
                ViewBag.Message = TempData["alert"].ToString();
            }
            var books = _bookService.GetBooksByString(SearchString);
            return View(books);
        }

        [HttpGet]
        public IActionResult Details(int? Id)
        {
            var book = _bookService.GetBooksByID(Id);
            return View(book);
        }
        
        [HttpPost]
        public IActionResult AddComment([FromBody] ReviewInputModel review)
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            int accountId = _accountService.GetAccountId(email);
            Console.WriteLine("did we go here? " + review.Id + " " + review.Rating + " " + review.Comment);
            if(!ModelState.IsValid)
            {
                var objs2 = new { user = email, comment = "r", rating = 1, firstComment = true  };
                return Json(objs2);
            }
            
            if(_bookService.PostBookReview(review, accountId) == true)
            {

                var objs = new { user = email, comment = review.Comment, rating = review.Rating, firstComment = true  };
                return Json(objs);
            }
            
            var obj = new { user = email, comment = review.Comment, rating = review.Rating, firstComment = false };
            return Json(obj);
        }

        [HttpPost]
        public IActionResult AllReviews([FromBody] int id)
        {   
            var bookReviews = _bookService.GetReviewsByBookID(id);
            
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
