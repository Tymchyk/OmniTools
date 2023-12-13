using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Omnitool.Models;

namespace Omnitool.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chat>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId);
            
            modelBuilder.Entity<MyTask>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId);
        }
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<Finance> Finances { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<MyTask> MyTasks { get; set; }
    }
    
}