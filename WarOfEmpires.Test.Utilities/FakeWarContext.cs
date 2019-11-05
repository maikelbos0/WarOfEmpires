using WarOfEmpires.Database;
using System.Data.Entity;
using Security = WarOfEmpires.Domain.Security;
using Auditing = WarOfEmpires.Domain.Auditing;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeWarContext : IWarContext {
        public IDbSet<Security.User> Users { get; set; } = new FakeDbSet<Security.User>();
        public int CallsToSaveChanges { get; private set; }
        public IDbSet<Auditing.CommandExecution> CommandExecutions { get ; set ; } = new FakeDbSet<Auditing.CommandExecution>();
        public IDbSet<Auditing.QueryExecution> QueryExecutions { get; set; } = new FakeDbSet<Auditing.QueryExecution>();

        public void Dispose() {
            SaveChanges();
        }

        public int SaveChanges() {
            CallsToSaveChanges++;

            // For now, implementation is not needed
            return 0;
        }
    }
}