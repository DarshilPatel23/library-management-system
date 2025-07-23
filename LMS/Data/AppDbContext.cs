using LMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<IssuedBook> IssuedBooks { get; set; }
        public DbSet<ReturnedBook> ReturnedBooks { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<AllTimeTransaction> AllTimeTransactions { get; set; }

    }
}