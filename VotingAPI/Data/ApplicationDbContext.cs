using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace VotingAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Option> Options { get; set; }
    }
}
