using Microsoft.EntityFrameworkCore;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Database {
    [TransientServiceImplementation(typeof(ILazyWarContext))]
    public class LazyWarContext : BaseWarContext<LazyWarContext>, ILazyWarContext {
        public LazyWarContext(AppSettings appSettings)
            : base(new DbContextOptionsBuilder<LazyWarContext>()
                  .UseSqlServer(appSettings.DatabaseConnectionString)
                  .UseLazyLoadingProxies()
                  .Options) { }
    }
}