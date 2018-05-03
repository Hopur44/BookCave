using BookCave.Data;
using System.Linq;
using BookCave.Models.ViewModels;
using System.Collections.Generic;

namespace BookCave.Services
{
    public class AccountService
    {
        private DataContext _db;
        public AccountService()
        {
            _db = new DataContext();
        }

        public AccountViewModel GetAccount()
        {
            var account = (from a in _db.Accounts
                            where a.Id == 1
                            select new AccountViewModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Address = a.Address,
                                Image = a.Image,
                                Email = a.Email,
                                FavouriteBook = a.FavouriteBook
                            }).FirstOrDefault();
            return account;
        }
    }
}