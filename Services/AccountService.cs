using BookCave.Data;
using System.Linq;
using BookCave.Models.ViewModels;
using BookCave.Models.InputModels;
using System.Collections.Generic;
using BookCave.Models.EntityModels;
using System;

namespace BookCave.Services
{
    public class AccountService
    {
        private DataContext _db;
        public AccountService()
        {
            _db = new DataContext();
        }

        public AccountViewModel GetAccount(string email)
        {
            var account = (from a in _db.Accounts
                            where a.Email == email
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
         public int GetAccountId(string email)
         {
             var id = (from a in _db.Accounts
                            where a.Email == email
                            select a.Id).FirstOrDefault();
            return id;
         }
        public void CreateAccount(RegisterViewModel createAccount)
        {
                    string name = createAccount.FirstName + " " + createAccount.LastName;
                    var newAccount = new AccountEntityModel
                    {
                        Name = name, 
                        Address = "", 
                        Image = "http://i0.kym-cdn.com/entries/icons/original/000/022/713/4.png",
                        Email = createAccount.Email,
                        FavouriteBook = ""
                    };
                    _db.Add(newAccount);
                    _db.SaveChanges();
                    Console.WriteLine("new account Success");
        }

         public void EditAccount(EditAccountInputModel editAccount, string email)
        {
                    var account = GetAccount(email);
                    var editedAccount = new AccountEntityModel
                    {
                        Id = account.Id,
                        Name = account.Name,
                        Email = account.Email,
                        Address = editAccount.Address, 
                        Image = editAccount.Image,
                        FavouriteBook = editAccount.FavouriteBook
                    };
                    _db.Update(editedAccount);
                    _db.SaveChanges();
                    Console.WriteLine("Account was Successfully Edited");
        }
    }
}