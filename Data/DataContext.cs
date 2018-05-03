using Microsoft.EntityFrameworkCore;
using BookCave.Models.EntityModels;

namespace BookCave.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AccountEntityModel> Accounts {get; set;}
        public DbSet<BillingEntityModel> Billings {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    "Server=tcp:verklegt2.database.windows.net,1433;Initial Catalog=VLN2_2018_H44;Persist Security Info=False;User ID=VLN2_2018_H44_usr;Password=pal3Feet27;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
                );
        }
    }
}