using cSharp_LibrarySystemWebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace cSharp_LibrarySystemWebAPI
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<Patron> Patron { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<BorrowingTransaction> BorrowingTransaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=(local);Initial Catalog=LibrarySysem; Integrated Security=true; TrustServerCertificate=True");
        }

    }
}
