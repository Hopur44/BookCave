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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid) {return View();}
            var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
            var result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                //The user successfully registered
                // add the concatenated first and last name as fullName in claims
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid) {return View();}
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if(result.Succeeded)
            {
                return RedirectToAction("MyAccount", "Account");
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

        public IActionResult MyAccount()
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            var account = _accountService.GetAccount(email);
            return View(account);
        }

        public IActionResult EditAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EditAccount(EditAccountInputModel model)
        {
            string email = ((ClaimsIdentity) User.Identity).Name;
            _accountService.EditAccount(model, email);
            return RedirectToAction("MyAccount");
        }

        public IActionResult EditSuccess()
        {
            return Json("Changes Succesfully Made");
        }
    }
}