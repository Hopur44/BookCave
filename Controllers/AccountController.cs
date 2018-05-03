using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Services;

namespace BookCave.Controllers
{
    public class AccountController : Controller
    {
        private AccountService _accountService;

        public AccountController()
        {
            _accountService = new AccountService();
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult MyAccount()
        {
            var account = _accountService.GetAccount();
            return View(account);
        }
    }
}