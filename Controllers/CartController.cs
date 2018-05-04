using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using Microsoft.AspNetCore.Authorization;
using BookCave.Models.ViewModels;
using BookCave.Services;

namespace BookCave.Controllers
{    
    public class CartController : Controller
    {
        private CartService _bookService;

        public CartController()
        {
            _bookService = new CartService();
        }
        private List<CartViewModel> allCartItems = new List<CartViewModel>();

        [HttpGet]
        public IActionResult Index()
        {
            //var cartItem1 = new CartViewModel() { ItemId = 1, Quantity = 10 };
            //var cartItem2 = new CartViewModel() { ItemId = 2, Quantity = 20 };
            //allCartItems.Add(cartItem1);
            //allCartItems.Add(cartItem2);
            return View(allCartItems);
        }

        [HttpPost]
        public IActionResult Index([FromBody] IEnumerable<CartViewModel> items)
        {
            Console.WriteLine("Cart Post");

            /*
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine(i);
            }
            */
            // var book = _bookService.GetBooksByID(Id);
            //var sortedList = items.Sort(i => i.ItemId).ToList();
            List<CartViewModel> SortedList = items.OrderBy(o => o.ItemId).ToList();
            foreach(var item in SortedList)
            {
                var book = _bookService.GetBooksByID(item.ItemId);

                book.Quantity = item.Quantity;
                Console.WriteLine("Item id: " + item.ItemId);
                allCartItems.Add(book);
                Console.WriteLine("BoodId: " + item.ItemId + " Quantity: " + item.Quantity + " Price: " + item.Price);
            }
            
            /*
            var cartItem1 = new CartViewModel() { ItemId = 1, Quantity = 10 };
            var cartItem2 = new CartViewModel() { ItemId = 2, Quantity = 20 };
            allCartItems.Add(cartItem1);
            allCartItems.Add(cartItem2);

            
            */
            /* 
            var results = new List<CartViewModel>()
            {
                new CartViewModel { ItemId = 1, Quantity = 1, Price = 1000, Title = "SomeBook"  },
                new CartViewModel { ItemId = 2, Quantity = 2, Price = 2000, Title = "AnotherBook" }
            };
            */
            //return Json(results);
            
            return Json(allCartItems);
        }
    }
}