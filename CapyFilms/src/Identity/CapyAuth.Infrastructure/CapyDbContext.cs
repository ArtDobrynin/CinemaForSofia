using Capy.Domain.Models.Auth;
using Capy.Domain.Models.Cinema;
using CapyAuth.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CapyAuth.Infrastructure
{
    public class CapyDbContext : DbContext, ICapyDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Films> Films { get; set; }

        public CapyDbContext(DbContextOptions<CapyDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Films)
                .WithMany(e => e.Users);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            Set<TEntity>().Entry(entity).State = EntityState.Modified;
            return base.Update(entity);
        }

        public bool GetFilmsToStatus(long? id)
        {
            var statusFilm = Films.FirstOrDefault(f => f.KinopoiskId == id);
            if (statusFilm is null)
            {
                return false;
            }
            return statusFilm.Status;
        }
    }
}
