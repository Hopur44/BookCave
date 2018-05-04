using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookCave.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookCave.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }        
    }
}