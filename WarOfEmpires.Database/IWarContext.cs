using Microsoft.EntityFrameworkCore;

namespace WarOfEmpires.Database {
    public interface IWarContext : IReadOnlyWarContext {
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}