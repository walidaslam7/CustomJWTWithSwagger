using Blogifier.Shared.DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace Blogifier.Providers.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Analytics> Analytics { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Blogs> Blogs { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<ExceptionLogs> ExceptionLogs { get; set; }
    }
}