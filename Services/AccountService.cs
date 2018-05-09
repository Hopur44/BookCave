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
        public List<UserOrderViewModel> GetUsersOrders(int accountId)
        {
            return (from b in _db.Books
                    join o in _db.Orders on b.Id equals o.BookId
                    join a in _db.Accounts on o.CustomerId equals a.Id
                    where a.Id == accountId
                    select new UserOrderViewModel
                    {
                        OrderId = o.Id,
                        Quantity = o.Quantity,
                        Title = b.Title,
                        Price = b.Price
                    }).ToList();
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
        public void CreateAccount(RegisterInputModel createAccount)
        {
            string name = createAccount.FirstName + " " + createAccount.LastName;
            var newAccount = new AccountEntityModel
            {
                Name = name, 
                Address = createAccount.Address, 
                Image = createAccount.Image,
                Email = createAccount.Email,
                FavouriteBook = createAccount.FavouriteBook
            };
            _db.Add(newAccount);
            _db.SaveChanges();
            Console.WriteLine("new account Success");
        }

         public void EditAccount(EditAccountInputModel editAccount, string email)
        {
            var account = GetAccount(email);
            /*three if statements to check if string from the textboxes are empty
                and gives them their right values if they are empty
            */ 
            if(editAccount.Address == null)
            {
                editAccount.Address = account.Address;
            }
            if(editAccount.Image == null)
            {
                editAccount.Image = account.Image;
            }
            if(editAccount.FavouriteBook == null)
            {
                editAccount.FavouriteBook = account.FavouriteBook;
            }
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