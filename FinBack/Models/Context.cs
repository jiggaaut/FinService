using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinBack.Models
{
    public class Context : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Balance> Balances { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
      
        public static void Initialize(Context db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            Client c1 = new Client { Name = "Ilya", Surname = "Martynenko", MiddleName = "Gennadievich", BirthDate = new DateTime(1995, 12, 17) };

            db.Clients.AddRange(new List<Client> { c1 });
            db.SaveChanges();

            Balance b1 = new Balance { AccountName = "Broker", Amount = 1000, ClientId = c1.Id };
            Balance b2 = new Balance { AccountName = "Credit Card", Amount = 5000, ClientId = c1.Id };
            db.Balances.AddRange(new List<Balance> { b1, b2 });
            db.SaveChanges();
        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=db;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }*/
    }
}
