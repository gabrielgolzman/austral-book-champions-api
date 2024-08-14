using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        public BookDbContext() { }

        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
           
        }
    }
}