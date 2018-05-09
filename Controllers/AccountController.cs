using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using BookCave.Services;
using Microsoft.AspNetCore.Identity;
using BookCave.Models.ViewModels;
using System.Security.Claims;
using BookCave.Models.InputModels;
using Microsoft.AspNetCore.Authorization;

namespace BookCave.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private AccountService _accountService;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager; 
            _accountService = new AccountService();
        }
        
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if(!ModelState.IsValid) 
            {
                return View();
            }
            var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                //The user successfully registered
                // add the concatenated first and last name as fullName in claims
                TempData["alert"] = "<div class=\"alert alert-success\"" + "role="+"alert" +">"
                    +"Account Created"+
                    "<button type=\"button\""+"class=\"close\""+" data-dismiss=\"alert\""+" aria-label=\"Close\"" +">"+
                    "<span aria-hidden=\"true\">&times;</span>"+"</button></div>";
                    
                await _userManager.AddClaimAsync(user, new Claim("Name", $"{model.FirstName} {model.LastName}"));
                await _signInManager.SignInAsync(user, false);
                _accountService.CreateAccount(model);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize]
        public IActionResult MyAccount()
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            var account = _accountService.GetAccount(email);
            if(TempData["alert"] != null)
            {
                ViewBag.Message = TempData["alert"].ToString();
            }
            return View(account);
        }
        [Authorize]
        public IActionResult EditAccount()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditAccount(EditAccountInputModel model)
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            _accountService.EditAccount(model, email);
            TempData["alert"] = "<div class=\"alert alert-success\"" + "role="+"alert" +">"
                    +"Your Edit was successful"+
                    "<button type=\"button\""+"class=\"close\""+" data-dismiss=\"alert\""+" aria-label=\"Close\"" +">"+
                    "<span aria-hidden=\"true\">&times;</span>"+"</button></div>";
            return RedirectToAction("MyAccount");
        }
        [Authorize]
        public IActionResult History()
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            int accountId = _accountService.GetAccountId(email);
            var orders = _accountService.GetUsersOrders(accountId);
            return View(orders);
        }
        public IActionResult EditSuccess()
        {
            return Json("Changes Succesfully Made");
        }
    }
}