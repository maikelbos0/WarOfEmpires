using Microsoft.EntityFrameworkCore;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Database {

    [ScopedServiceImplementation(typeof(IWarContext))]
    public class WarContext : BaseWarContext<WarContext>, IWarContext {
        public WarContext(AppSettings appSettings) 
            : base(new DbContextOptionsBuilder<WarContext>()
                  .UseSqlServer(appSettings.DatabaseConnectionString)
                  .Options) { }
    }
}