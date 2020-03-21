using WarOfEmpires.Database;
using System.Data.Entity;
using Auditing = WarOfEmpires.Domain.Auditing;
using Events = WarOfEmpires.Domain.Events;
using Markets = WarOfEmpires.Domain.Markets;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeWarContext : IWarContext {
        public IDbSet<Security.User> Users { get; set; } = new FakeDbSet<Security.User>();
        public int CallsToSaveChanges { get; private set; }
        public IDbSet<Auditing.CommandExecution> CommandExecutions { get ; set ; } = new FakeDbSet<Auditing.CommandExecution>();
        public IDbSet<Auditing.QueryExecution> QueryExecutions { get; set; } = new FakeDbSet<Auditing.QueryExecution>();
        public IDbSet<Markets.Caravan> Caravans { get; set; } = new FakeDbSet<Markets.Caravan>();
        public IDbSet<Players.Player> Players { get; set; } = new FakeDbSet<Players.Player>();
        public IDbSet<Events.ScheduledTask> ScheduledTasks { get; set; } = new FakeDbSet<Events.ScheduledTask>();

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