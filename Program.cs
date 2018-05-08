﻿using System;
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
            //if(!db.Books.Any())
            {
                var initialBooks = new List<BookEntityModel>()
                {
                    /*new BookEntityModel
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
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/a/a4/Ready_Player_One_cover.jpg",
                        PublishDate = "2011",
                        PageNumber = 384
                    },  
                     new BookEntityModel 
                    {
                        Price = 2999,
                        Title = "Origin: A Novel",
                        Genre = "Mystery",
                        Description ="“Origin” marks the fifth outing for Harvard professor Robert Langdon, the symbologist who uncovered stunning secrets and shocking conspiracies in “The Da Vinci Code” and Brown’s other phenomenally best-selling novels.",
                        Author = "‎Dan Brown",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/6/67/Origin_%28Dan_Brown_novel_cover%29.jpg",
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
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/e/ed/The_Midnight_Line_-_book_cover.jpg",
                        PublishDate = "2017",
                        PageNumber = 384
                    },
                    new BookEntityModel 
                    {
                        Price = 1999,
                        Title = "Killing Floor",
                        Genre = "Mystery",
                        Description ="Ex-military policeman Jack Reacher finds himself in Margrave, Georgia, where he is almost immediately arrested for murder; after being released, he joins with a straight-arrow detective and a beautiful lady cop to unearth a conspiracy that stretches through the small town and beyond.",
                        Author = "‎Lee Child",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/c/c9/Killing_Floor_Cover.jpg",
                        PublishDate = "1997",
                        PageNumber = 552
                    },
                    new BookEntityModel 
                    {
                        Price = 1999,
                        Title = "Die Trying",
                        Genre = "Mystery",
                        Description ="In a plot to overthrow the U.S. government, Montana neo-Nazis abduct the daughter of the nation's top general and her male companion, Jack Reacher, a former military policeman who turns the tables on the captors.",
                        Author = "‎Lee Child",
                        ImageLink = "https://d30a6s96kk7rhm.cloudfront.net/original/readings/978/055/350/9780553505412.jpg",
                        PublishDate = "1998",
                        PageNumber = 374
                    },
                    new BookEntityModel 
                    {
                        Price = 1999,
                        Title = "Tripwire",
                        Genre = "Mystery",
                        Description ="Ex military policeman Jack Reacher is enjoying the lazy anonymity of Key West when a stranger shows up asking for him. He’s got a lot of questions. Reacher does too, especially after the guy turns up dead. The answers lead Reacher on a cold trail back to New York, to the tenuous confidence of an alluring woman, and the dangerous corners of his own past.",
                        Author = "‎Lee Child",
                        ImageLink = "https://moly.hu/system/covers/big/covers_250363.jpg",
                        PublishDate = "1999",
                        PageNumber = 343
                    },
                    new BookEntityModel 
                    {
                        Price = 1999,
                        Title = "The Visitor",
                        Genre = "Mystery",
                        Description ="Sergeant Amy Callan and Lieutenant Caroline Cooke have a lot in common. Both were army high-flyers. Both were aquainted with Jack Reacher. Both were forced to resign from the service. Now they're both dead. Found in their own homes, naked, in a bath full of paint. Apparent victims of an army man. A loner, a smart guy with a score to settle, a ruthless vigilante. A man just like Jack Reacher.",
                        Author = "‎Lee Child",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/4/48/The_visitor_book.jpg",
                        PublishDate = "2000",
                        PageNumber = 512
                    },
                    new BookEntityModel 
                    {
                        Price = 1999,
                        Title = "The Visitor",
                        Genre = "Mystery",
                        Description ="Sergeant Amy Callan and Lieutenant Caroline Cooke have a lot in common. Both were army high-flyers. Both were aquainted with Jack Reacher. Both were forced to resign from the service. Now they're both dead. Found in their own homes, naked, in a bath full of paint. Apparent victims of an army man. A loner, a smart guy with a score to settle, a ruthless vigilante. A man just like Jack Reacher.",
                        Author = "‎Lee Child",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/4/48/The_visitor_book.jpg",
                        PublishDate = "2000",
                        PageNumber = 512
                    },*/
                    new BookEntityModel 
                    {
                        Price = 2999,
                        Title = "A Game of Thrones",
                        Genre = "Fantasy",
                        Description ="Long ago, in a time forgotten, a preternatural event threw the seasons out of balance. In a land where summers can last decades and winters a lifetime, trouble is brewing. The cold is returning, and in the frozen wastes to the north of Winterfell, sinister forces are massing beyond the kingdom's protective Wall. To the south, the king's powers are failing - his most trusted adviser dead under mysterious circumstances and his enemies emerging from the shadows of the throne. The first book in the 'a song of ice and fire' series",
                        Author = "‎George R. R. Martin",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/9/93/AGameOfThrones.jpg",
                        PublishDate = "1996",
                        PageNumber = 694
                    },
                    new BookEntityModel 
                    {
                        Price = 2999,
                        Title = "A Clash of Kings",
                        Genre = "Fantasy",
                        Description ="A Clash of Kings depicts the Seven Kingdoms of Westeros in civil war, while the Night's Watch mounts a reconnaissance to investigate the mysterious people known as wildlings. Meanwhile, Daenerys Targaryen continues her plan to reconquer the Seven Kingdoms.",
                        Author = "‎George R. R. Martin",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/3/39/AClashOfKings.jpg",
                        PublishDate = "1998",
                        PageNumber = 768
                    },
                    new BookEntityModel 
                    {
                        Price = 2999,
                        Title = "A Storm Of Swords",
                        Genre = "Fantasy",
                        Description ="Of the five contenders for power, one is dead, another in disfavor, and still the wars rage, as alliances are made and broken. Joffrey sits on the Iron Throne, the uneasy ruler of the Seven Kingdoms. His most bitter rival, Lord Stannis, stands defeated and disgraced, victim of the sorceress who holds him in her thrall. Young Robb still rules the North from the fortress of Riverrun. Meanwhile, making her way across a blood-drenched continent is the exiled queen, Daenerys, mistress of the only three dragons left in the world. And as opposing forces maneuver for the final showdown, an army of barbaric wildlings arrives from the outermost limits of civilization, accompanied by a horde of mythical Others—a supernatural army of the living dead whose animated corpses are unstoppable. As the future of the land hangs in the balance, no one will rest until the Seven Kingdoms have exploded in a veritable storm of swords.",
                        Author = "‎George R. R. Martin",
                        ImageLink = "https://upload.wikimedia.org/wikipedia/en/2/24/AStormOfSwords.jpg",
                        PublishDate = "2000",
                        PageNumber = 973
                    },
                    new BookEntityModel 
                    {
                        Price = 2999,
                        Title = "A Feast for Crows",
                        Genre = "Fantasy",
                        Description ="The Lannisters are in power on the Iron Throne. The war in the Seven Kingdoms has burned itself out, but in its bitter aftermath new conflicts spark to life. The Martells of Dorne and the Starks of Winterfell seek vengeance for their dead. Euron Crow's Eye, as black a pirate as ever raised a sail, returns from the smoking ruins of Valyria to claim the Iron Isles.",
                        Author = "‎George R. R. Martin",
                        ImageLink = "https://d1w7fb2mkkr3kw.cloudfront.net/assets/images/book/lrg/9780/0064/9780006486121.jpg",
                        PublishDate = "2005",
                        PageNumber = 753
                    },
                       new BookEntityModel 
                    {
                        Price = 2999,
                        Title = "A Dance with Dragons",
                        Genre = "Fantasy",
                        Description ="The last of the Targaryons, Daenerys Stormborn, the Unburnt, has brought the young dragons in her care to their terrifying maturity. Now the war-torn landscape of the Seven Kingdoms is threatened by destruction as vast as in the violent past. Tyrion Lannister, a dwarf with half a nose and a scar from eye to chin, has slain his father and escaped the Red Keep in King\'s Landing to wage war from the Free Cities beyond the narrow sea. The last war fought with dragons was a cataclysm powerful enough to shatter the Valyrian peninsula into a smoking, demon-haunted ruin half drowned by the sea. A DANCE WITH DRAGONS brings to life dark magic, complex political intrigue and horrific bloodshed as events at the Wall and beyond the sea threaten the ancient land of Westeros.",
                        Author = "‎George R. R. Martin",
                        ImageLink = "https://d1w7fb2mkkr3kw.cloudfront.net/assets/images/book/lrg/9780/0022/9780002247399.jpg",
                        PublishDate = "2011",
                        PageNumber = 1040
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
