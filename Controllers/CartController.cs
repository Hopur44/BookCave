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
using System.Security.Claims;

namespace BookCave.Controllers
{    
    public class CartController : Controller
    {
        private AccountService _accountService;
        private CartService _cartService;

        public CartController()
        {
            _cartService = new CartService();
            _accountService = new AccountService();
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
        public IActionResult LoggedInUserAdd([FromBody] CartViewModel item)
        {
            // get items
            string email = ((ClaimsIdentity) User.Identity).Name;
            int id = _accountService.GetAccountId(email);
            Console.WriteLine("this item:" + item.ItemId);
            
            //adds the item to the accounts cart
            _cartService.InsertToCart(item,id);

            var toReturn = true;
            return Json(toReturn);
        }

        [HttpPost]
        public IActionResult Buy([FromBody] IEnumerable<CartViewModel> items)
        {
            Console.WriteLine("Cart/Buy Post");

            string email = ((ClaimsIdentity) User.Identity).Name;
            Console.WriteLine(email);
            var isLoggedIn = false;
            if(string.IsNullOrEmpty(email)) 
            {
                Console.WriteLine("im not logged in");
                //return RedirectToAction("Login", "Account");
                
                return Json(isLoggedIn);
            } 
            else 
            {   
                isLoggedIn = true;

                // add to Cart table in database...
                return Json(isLoggedIn);
                //Console.WriteLine("im logged in");
            }
            // are you logged in?

                // if yes.. then add to cart table
                // and re-direct to checkout

                // if no.. then redirect to log-in page
            
            
            
            /*
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine(i);
            }
            */
            // var book = _bookService.GetBooksByID(Id);
            //var sortedList = items.Sort(i => i.ItemId).ToList();
            /* 
            List<CartViewModel> SortedList = items.OrderBy(o => o.ItemId).ToList();
            foreach(var item in SortedList)
            {
                var book = _bookService.GetBooksByID(item.ItemId);

                book.Quantity = item.Quantity;
                Console.WriteLine("Item id: " + item.ItemId);
                allCartItems.Add(book);
                Console.WriteLine("BoodId: " + item.ItemId + " Quantity: " + item.Quantity + " Price: " + item.Price);
            }
            */
            
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
            
            //return Json(allCartItems);
        }
        [HttpPost]
        public IActionResult Index([FromBody] IEnumerable<CartViewModel> items)
        {
            Console.WriteLine("Cart Post");

            string email = ((ClaimsIdentity) User.Identity).Name;
            Console.WriteLine(email);
            /* if the user is not logged in then show him what is in localStorage cart
            else show the logged in user what is in his cart from the database */
            if(string.IsNullOrEmpty(email)) 
            {
                Console.WriteLine("im not logged in");
                List<CartViewModel> SortedList = items.OrderBy(o => o.ItemId).ToList();
                foreach(var item in SortedList)
                {
                    var book = _cartService.GetBooksByID(item.ItemId);
                    book.Quantity = item.Quantity;
                    Console.WriteLine("Item id: " + item.ItemId);
                    allCartItems.Add(book);
                    Console.WriteLine("BoodId: " + item.ItemId + " Quantity: " + item.Quantity + " Price: " + item.Price);
                }
            } 
            else 
            {   
                // sækja úr database úr cart-table og senda sem json til baka.
                var results = new List<CartViewModel>()
                {
                    new CartViewModel { ItemId = 1, Quantity = 1, Price = 1000, Title = "SomeBook"  },
                    new CartViewModel { ItemId = 2, Quantity = 2, Price = 2000, Title = "AnotherBook" }
                };
                return Json(results);
                // if the table cart from db is empty then show him the localStorage stuff.
            }
            return Json(allCartItems);
        }
    }
}