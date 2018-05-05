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

namespace BookCave.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private CheckoutService _checkoutService;

        public CheckoutController()
        {
            _checkoutService = new CheckoutService();
        }

        [HttpGet]
        public IActionResult Billing()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Billing(BillingInputModel billing)
        {
            return RedirectToAction("Review", billing);
        }
        public IActionResult Review(BillingInputModel billing)
        {
            _checkoutService.CreateBilling(billing);
            return RedirectToAction("Index","Home");
        }
    }
}