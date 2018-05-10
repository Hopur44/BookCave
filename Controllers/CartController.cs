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
            string email = ((ClaimsIdentity) User.Identity).Name;
            int id = _accountService.GetAccountId(email);

            allCartItems = _cartService.GetUserCartItems(id);
            return View(allCartItems);
        }

        [HttpPost]
        public IActionResult Index([FromBody] IEnumerable<CartViewModel> items)
        {
            //Console.WriteLine("Cart Post for logged in user");
            string email = ((ClaimsIdentity) User.Identity).Name;
            //Console.WriteLine(email);
            //if the user is not logged in then show him what is in localStorage cart
            //else show the logged in user what is in his cart from the database
            if(string.IsNullOrEmpty(email)) 
            {
                //Console.WriteLine("im not logged in");
                List<CartViewModel> SortedList = items.OrderBy(o => o.ItemId).ToList();
                foreach(var item in SortedList)
                {
                    var book = _cartService.GetBooksByID(item.ItemId);
                    book.Quantity = item.Quantity;
                    //Console.WriteLine("Item id: " + item.ItemId);
                    allCartItems.Add(book);
                    //Console.WriteLine("BoodId: " + item.ItemId + " Quantity: " + item.Quantity + " Price: " + item.Price);
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
                //bool item = true;
                return Json(results);
                // if the table cart from db is empty then show him the localStorage stuff.
            }
            return Json(allCartItems);
        } 

        /*
        [HttpPost]
        public IActionResult LoggedInUserCheckCart([FromBody] CartViewModel item)
        {
            // get items
            string email = ((ClaimsIdentity) User.Identity).Name;
            int id = _accountService.GetAccountId(email);
            if(string.IsNullOrEmpty(email)) {
                return Json(false);
            }

            var toReturn = true;
            return Json(toReturn);
        }
        */
        // Logged in user sends item through ajax to add it, remove it or clear his cart
        [HttpPost]
        public IActionResult LoggedInUserCartAction([FromBody] CartViewModel item)
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            int id = _accountService.GetAccountId(email);
            if(string.IsNullOrEmpty(email)) {
                return Json(false);
            }
            
            //adds the item to the accounts cart
            if(item.Action == true) {
                 _cartService.InsertToCart(item, id);
            }
            else 
            {
                _cartService.RemoveOneFromCart(item, id);
            }
           
            var toReturn = true;
            return Json(toReturn);
        }

        // Logged in user clearing his cart
        [HttpPost]
        public IActionResult LoggedInUserCartClear()
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            int id = _accountService.GetAccountId(email);
            if(string.IsNullOrEmpty(email)) {
                return Json(false);
            }
            
            var userCart = _cartService.GetUserCartItems(id);

            foreach(var item in userCart)
            {
                _cartService.RemoveCart(item, id);
            }
            
            var toReturn = true;
            return Json(toReturn);
        }

        // logged in user
        // adding everything from localStorage to database
        [HttpPost]
        public IActionResult AddAllCartItems([FromBody] List<CartViewModel> items)
        {
            Console.WriteLine("Cart/AddAllCartItems Post");

            string email = ((ClaimsIdentity) User.Identity).Name;
            int id = _accountService.GetAccountId(email);
            Console.WriteLine(email);
            var isLoggedIn = false;
            if(string.IsNullOrEmpty(email)) 
            {
                return Json(isLoggedIn);
            } 
            else 
            {   
                Console.WriteLine("im not logged in and i'm adding everything from local");
                isLoggedIn = true;

                _cartService.InsertAllItems(items, id);
    
                // add to Cart table in database...
                return Json(isLoggedIn);
            }

        }
        
        public IActionResult GetTotalQuantity()
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            int id = _accountService.GetAccountId(email);
            var userCart = _cartService.GetUserCartItems(id);

            int Quantity = _cartService.GetTotalCartItems(userCart);

            return Json(Quantity);
        }
    }
}