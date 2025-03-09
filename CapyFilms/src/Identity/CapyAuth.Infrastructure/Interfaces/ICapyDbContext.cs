using Capy.Domain.Models.Auth;
using Capy.Domain.Models.Cinema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CapyAuth.Infrastructure.Interfaces
{
    public interface ICapyDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Films> Films { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public EntityEntry Update(object entity);
        public bool GetFilmsToStatus(long? id);
    }
}
