using Microsoft.EntityFrameworkCore;
using SignalRLab3.Entities;

namespace SignalRLab3.DataAccess
{
    public class SignalRLab3DbContext : DbContext
    {
        public SignalRLab3DbContext(DbContextOptions<SignalRLab3DbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #region Entities

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }

        #endregion

    }
}
