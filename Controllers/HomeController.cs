using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Services;
using Microsoft.AspNetCore.Authorization;

namespace BookCave.Controllers
{
    public class HomeController : Controller
    {
        private BookService _bookService;
        public HomeController()
        {
            _bookService = new BookService();
        }
        public IActionResult Index(string SearchString)
        {
            var books = _bookService.GetBooksByString(SearchString);
            return View(books);
        }

        public IActionResult Details(int Id)
        {
            var book = _bookService.GetBooksByID(Id);
            return View(book);
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


    }
}
