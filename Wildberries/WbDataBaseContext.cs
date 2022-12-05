using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wildberries.Shared.Domain.Entity;

namespace Wildberries
{
    public class WbDataBaseContext : DbContext
    {
        public DbSet<CardEntity> Card { get; set; }
        public DbSet<UserEntity> User { get; set; }
        private string _connectionString;

        public WbDataBaseContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        public WbDataBaseContext()
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
            modelBuilder.Entity<UserEntity>()
               .HasKey(e => e.Id);

            modelBuilder.Entity<CardEntity>()
               .HasKey(e => e.Id);

            modelBuilder.Entity<TimePoint>()
               .HasKey(e => e.Id);

            modelBuilder.Entity<UserEntity>()
                .HasMany(x => x.UserProduct)
                .WithOne(x => x.User)
                //.HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CardEntity>()
                .HasMany(x => x.TimePoint)
                .WithOne(x => x.Card)
                //.HasForeignKey(x =>x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
