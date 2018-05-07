using BookCave.Models.ViewModels;
using BookCave.Models.InputModels;
using System.Collections.Generic;

namespace BookCave.Models.ViewModels
{
    public class OrderViewModel
    {
        public List<CartViewModel> CartList {get; set;}

        public BillingInputModel Billing {get; set;}
    }
}