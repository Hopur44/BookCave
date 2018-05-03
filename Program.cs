using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BookCave.Data;
using BookCave.Models.EntityModels;

namespace BookCave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            SeedData();
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        public static void SeedData()
        {
            var db = new DataContext();
            if(!db.Accounts.Any())
            {
                var initialAccounts = new List<AccountEntityModel>()
                {
                    new AccountEntityModel
                    {
                        Name = "Viktor Ingi Kárason", 
                        Address = "Heimili 22", 
                        Image = "http://i0.kym-cdn.com/entries/icons/original/000/022/713/4.png",
                        Email = "chunkylover69@hotmail.com",
                        Password = "MonkaS1",
                        FavouriteBook = "Harry Potter Series"
                    },
                    new AccountEntityModel
                    {
                        Name = "Sævaldur Viðarsson", 
                        Address = "Heimili 21", 
                        Image = "http://i0.kym-cdn.com/entries/icons/original/000/022/713/4.png",
                        Email = "seapower@gmail.com",
                        Password = "MonkaS2",
                        FavouriteBook = "Game of Thrones"
                    }
                };

                db.AddRange(initialAccounts);
                db.SaveChanges();
            }
        }
    }
}
