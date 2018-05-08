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
                        FavouriteBook = "Harry Potter Series"
                    },
                    new AccountEntityModel
                    {
                        Name = "Sævald Viðarsson", 
                        Address = "Heimili 21", 
                        Image = "http://i0.kym-cdn.com/entries/icons/original/000/022/713/4.png",
                        Email = "seapower@gmail.com",
                        FavouriteBook = "Game of Thrones"
                    }
                };

                db.AddRange(initialAccounts);
                db.SaveChanges();
            }
            if(!db.Books.Any())
            {
                var initialBooks = new List<BookEntityModel>()
                {
                    new BookEntityModel
                    {
                        Price = 2000,
                        Title = "Harry Potter and the Philosopher's Stone",
                        Genre = "Adventure",
                        Description ="On his eleventh birthday, Harry Potter discovers that he is no ordinary boy. Hagrid, a beetle-eyed giant, tells Harry that he is a wizard and has a place at Hogwarts School of Witchcraft and Wizardry.",
                        Author = "J.K. Rowling",
                        ImageLink = "https://vignette.wikia.nocookie.net/harrypotter/images/c/cb/Philosoper%27s_Stone_New_UK_Cover.jpg/revision/latest/scale-to-width-down/334?cb=20170109041611",
                        PublishDate = "1997",
                        PageNumber = 223
                    },
                    new BookEntityModel
                    {
                        Price = 2000,
                        Title = "Harry Potter and the Chamber of Secrets",
                        Genre = "Adventure",
                        Description = "Harry, it turns out, is a Parsel-tongue. This means that he is able to speak/understand snakes. Everyone thinks that it's him that has opened the Chamber of Secrets because that is what Slytherin was famous for. Harry Potter returns to Hogwarts School of Wizardry for his second year.",
                        Author = "J.K. Rowling",
                        ImageLink = "http://img.timeinc.net/time/2007/harry_potter/hp_books/chamber_of_secrets.jpg",
                        PublishDate = "1998",
                        PageNumber = 251
                    },
                    new BookEntityModel
                    {
                        Price = 2000,
                        Title = "Harry Potter and the Prisoner of Azkaban",
                        Genre = "Adventure",
                        Description = "For twelve long years, the dread fortress of Azkaban held an infamous prisoner named Sirius Black. Convicted of killing thirteen people with a single curse, he was said to be the heir apparent to the Dark Lord, Voldemort.",
                        Author = "J.K. Rowling",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/a/a0/Harry_Potter_and_the_Prisoner_of_Azkaban.jpg",
                        PublishDate = "1999",
                        PageNumber = 317
                    },
                    new BookEntityModel
                    {
                        Price = 2999,
                        Title = "The Hitchhiker’s Guide to the Galaxy",
                        Genre = "Comedy",
                        Description = "Adams was hitchhiking through Europe when he came up with the idea for this book, and the result is a beautifully inventive blend of Monty Python and Isaac Asimov – a teeming universe of absurdity, singularity and satirical philosophy, where planets are demolished to make way for galactic motorways.",
                        Author = "Douglas Adams",
                        ImageLink = "https://images.penguinrandomhouse.com/cover/9781400052929",
                        PublishDate = "1987",
                        PageNumber = 208
                    },
                    new BookEntityModel
                    {
                        Price = 2000,
                        Title = "Harry Potter and the Goblet of Fire",
                        Genre = "Adventure",
                        Description = "On halloween night, the Goblet of Fire spits out the names of the champions who will compete in the Triwizard Tournament; along with Cedric Diggory, Fleur Delacour, and Viktor Krum, Harry Potter is selected. Mass chaos ensues, since Harry is too young.",
                        Author = "J.K. Rowling",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/c/c7/Harry_Potter_and_the_Goblet_of_Fire.jpg",
                        PublishDate = "2000",
                        PageNumber = 636
                    },
                    new BookEntityModel
                    {
                        Price = 3999,
                        Title = "Pride and Prejudice",
                        Genre = "Romance",
                        Description = "In a remote Hertfordshire village, far off the good coach roads of George III's England, a country squire of no great means must marry off his five vivacious daughters. At the heart of this all-consuming enterprise are his headstrong second daughter Elizabeth Bennet and her aristocratic suitor Fitzwilliam Darcy — two lovers whose pride must be humbled and prejudices.",
                        Author = "Jane Austen",
                        ImageLink = "https://prodimage.images-bn.com/pimages/9781435160514_p0_v1_s550x406.jpg",
                        PublishDate = "1813",
                        PageNumber = 213
                    },
                    new BookEntityModel
                    {
                        Price = 3999,
                        Title = "Lord Of The Rings: The Fellowship of the Ring",
                        Genre = "Fantasy",
                        Description = "Frodo sets forth from Rivendell with eight companions: two Men, Aragorn and Boromir; Legolas; Gandalf; Gimli the Dwarf, the son of Glóin; and Frodo's three Hobbit companions. These Nine Walkers (called the Fellowship of the Ring) are chosen to represent all the free races of Middle-earth and as a balance to the Nazgûl.",
                        Author = "‎J. R. R. Tolkien",
                        ImageLink = "https://i.pinimg.com/736x/65/75/59/657559342e32d10975bf99961ab084f0--fellowship-of-the-ring-lord-of-the-rings.jpg",
                        PublishDate = "1954	",
                        PageNumber = 479
                    },
                    new BookEntityModel
                    {
                        Price = 3999,
                        Title = "The Hobbit",
                        Genre = "Fantasy",
                        Description ="Bilbo Baggins is a hobbit who lives a quiet life, until it is upset by a visit from a wizard named Gandalf. He wants Bilbo to help a group of dwarves take back the Mountain from Smaug, a dragon. Bilbo is unsure he wants to help, but he is drawn into the adventure by Gandalf, who tells the dwarves Bilbo is a burglar.",
                        Author = "‎J. R. R. Tolkien",
                        ImageLink = "https://images-na.ssl-images-amazon.com/images/I/91b0C2YNSrL.jpg",
                        PublishDate = "1937",
                        PageNumber = 304
                    },
                    new BookEntityModel
                    {
                        Price = 1999,
                        Title = "Ender's Game",
                        Genre = "Science fiction",
                        Description ="Andrew 'Ender' Wiggin thinks he is playing computer simulated war games; he is, in fact, engaged in something far more desperate. The result of genetic experimentation, Ender may be the military genius Earth desperately needs in a war against an alien enemy seeking to destroy all human life.",
                        Author = "‎Orson Scott Card",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/e/e4/Ender%27s_game_cover_ISBN_0312932081.jpg",
                        PublishDate = "1985",
                        PageNumber = 324
                    },
                    new BookEntityModel
                    {
                        Price = 1999,
                        Title = "Magpie Murders",
                        Genre = "Mystery",
                        Description ="Conway's latest tale has Atticus Pünd investigating a murder at Pye Hall, a local manor house. Yes, there are dead bodies and a host of intriguing suspects, but the more Susan reads, the more she's convinced that there is another story hidden in the pages of the manuscript: one of real-life jealousy, greed, ruthless ambition, and murder. ",
                        Author = "‎Anthony Horowitz",
                        ImageLink = "https://images-na.ssl-images-amazon.com/images/I/41o4XTRBFVL._AC_US327_FMwebp_QL65_.jpg",
                        PublishDate = "2017",
                        PageNumber = 496
                    },  
                    new BookEntityModel 
                    {
                        Price = 3499,
                        Title = "The Fallen",
                        Genre = "Mystery",
                        Description ="Something sinister is going on in Baronville. The rust belt town has seen four bizarre murders in the space of two weeks. Cryptic clues left at the scenes--obscure bible verses, odd symbols--have the police stumped.",
                        Author = "‎David Baldacci",
                        ImageLink = "https://prodimage.images-bn.com/pimages/9781538761397_p0_v3_s600x595.jpg",
                        PublishDate = "2018",
                        PageNumber = 433
                    },    
                     new BookEntityModel 
                    {
                        Price = 3499,
                        Title = "Ready Player One",
                        Genre = "Science fiction",
                        Description ="In the year 2045, reality is an ugly place. The only time teenage Wade Watts really feels alive is when he's jacked into the virtual utopia known as the OASIS. Wade's devoted his life to studying the puzzles hidden within this world's digital confines, puzzles that are based on their creator's obsession with the pop culture of decades past and that promise massive power and fortune to whoever can unlock them. When Wade stumbles upon the first clue, he finds himself beset by players willing to kill to take this ultimate prize. The race is on, and if Wade's going to survive, he'll have to win—and confront the real world he's always been so desperate to escape.",
                        Author = "‎Ernest Cline",
                        ImageLink = "https://en.wikipedia.org/wiki/Ready_Player_One#/media/File:Ready_Player_One_cover.jpg",
                        PublishDate = "2011",
                        PageNumber = 384
                    },  
                     new BookEntityModel 
                    {
                        Price = 2999,
                        Title = "Origin: A Novel",
                        Genre = "Mystery",
                        Description ="",
                        Author = "‎Dan Brown",
                        ImageLink = "https://en.wikipedia.org/wiki/Origin_(Brown_novel)#/media/File:Origin_(Dan_Brown_novel_cover).jpg",
                        PublishDate = "2017",
                        PageNumber = 480
                    },       
                     new BookEntityModel 
                    {
                        Price = 3999,
                        Title = "Twisted Prey",
                        Genre = "Mystery",
                        Description ="Lucas Davenport confronts an old nemesis, now a powerful U.S. senator, in this thrilling #1 New York Times-bestselling new novel in the Prey series.",
                        Author = "‎John Sandford",
                        ImageLink = "https://prodimage.images-bn.com/pimages/9780525538547_p0_v1_s600x595.jpg",
                        PublishDate = "2018",
                        PageNumber = 400
                    },
                      new BookEntityModel 
                    {
                        Price = 3999,
                        Title = "The Midnight Line: A Jack Reacher Novel",
                        Genre = "Mystery",
                        Description ="Lee Child returns with a gripping new powerhouse thriller featuring Jack Reacher, “one of this century’s most original, tantalizing pop-fiction heroes”",
                        Author = "‎Lee Child",
                        ImageLink = "https://en.wikipedia.org/wiki/The_Midnight_Line#/media/File:The_Midnight_Line_-_book_cover.jpg",
                        PublishDate = "2017",
                        PageNumber = 384
                    }
                };
                    db.AddRange(initialBooks);
                    db.SaveChanges();  
                    Console.WriteLine("Books DONE");
            }
            if(!db.Billings.Any())
            {
                var initialBilling = new List<BillingEntityModel>()
                {
                    new BillingEntityModel
                    {
                        StreetAddress = "Heimili 21",
                        City = "Reykjavik",
                        Country = "Iceland",
                        ZipCode = 112                        
                    },
                    new BillingEntityModel
                    {
                        StreetAddress = "Heimili 22",
                        City = "Selfoss",
                        Country = "Iceland",
                        ZipCode = 800                        
                    }
                };
                    db.AddRange(initialBilling);
                    db.SaveChanges(); 
                    Console.WriteLine("Billing DONE");
            }

            if(!db.Reviews.Any())
            {
                var initialReviews = new List<ReviewEntityModel>()
                {
                    new ReviewEntityModel
                    {
                        CustomerId = 1,
                        BookId = 1,
                        Rating = 5,
                        comment = "Great Book, Great fun mumford and sons"
                    },
                    new ReviewEntityModel
                    {
                        CustomerId = 2,
                        BookId = 3,
                        Rating = 3,
                        comment = "I Read this book and found it to be above average"
                    }
                };
                    db.AddRange(initialReviews);
                    db.SaveChanges(); 
                    Console.WriteLine("Reviews DONE");
            }
            if(!db.Orders.Any())
            {
                var initialOrders = new List<OrderEntityModel>()
                {
                    new OrderEntityModel
                    {
                        BillingId = 1,
                        CustomerId = 1,
                        BookId = 1                        
                    },
                    new OrderEntityModel
                    {
                        BillingId = 2,
                        CustomerId = 2,
                        BookId = 3                        
                    }
                };
                    db.AddRange(initialOrders);
                    db.SaveChanges(); 
                    Console.WriteLine("Orders DONE");
            }
        }
    }
}
