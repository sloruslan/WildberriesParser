using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildberries;

namespace Server.Infrastructure
{
    public class CardContext : DbContext
    {
        public DbSet<CardEntity> Card { get; set; }

        private string _connectionString;

        public CardContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        public CardContext()
        {
            Database.EnsureCreated();
        }
        //public TestContext(DbContextOptions<TestContext> contextOptions) : base(contextOptions)
        //{
        //    Database.EnsureCreated();
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Wildberries;Username=postgres;Password=32167");
            //optionsBuilder.UseSqlite(_connectionString);
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardEntity>()
               .HasKey(e => e.Id);
        }
    }
}
