using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using Microsoft.AspNetCore.Authorization;
using BookCave.Services;
using BookCave.Models.InputModels;
using BookCave.Models.ViewModels;
using System.Security.Claims;

namespace BookCave.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private CheckoutService _checkoutService;
        private AccountService _accountService;
        private CartService _cartService;

        public CheckoutController()
        {
            _checkoutService = new CheckoutService();
            _accountService = new AccountService();
            _cartService = new CartService();
        }

        [HttpGet]
        public IActionResult Billing()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Billing(BillingInputModel billing)
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            int accountId = _accountService.GetAccountId(email);
            var userCart = _cartService.GetUserCartItems(accountId);

            _checkoutService.CreateBilling(billing,accountId);

            var reviewOrder = _checkoutService.GetReviewOrder(billing, userCart);

            return View("Review", reviewOrder);
        }
        [HttpPost]
        public IActionResult Review(OrderViewModel order)
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            int accountId = _accountService.GetAccountId(email);
            var userCart = _cartService.GetUserCartItems(accountId);
            int billingId = _checkoutService.GetBillingId(accountId);

            _checkoutService.CreateOrder(userCart,accountId,billingId);
            var billing = _checkoutService.GetBilling(billingId);
            var confirmedOrder = _checkoutService.GetReviewOrder(billing, userCart);
            
            return View("Confirmed", confirmedOrder);   
        }

        public IActionResult Confirmed(OrderViewModel order)
        {
            return View(order);
        }
        public IActionResult Order()
        {
            return View();
        }
    }
}