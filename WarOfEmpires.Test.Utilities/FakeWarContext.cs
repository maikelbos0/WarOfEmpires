using WarOfEmpires.Database;
using Microsoft.EntityFrameworkCore;
using Alliances = WarOfEmpires.Domain.Alliances;
using Auditing = WarOfEmpires.Domain.Auditing;
using Events = WarOfEmpires.Domain.Events;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {

    public sealed class FakeWarContext : IWarContext {
        public DbSet<Security.User> Users { get; set; } = new FakeDbSet<Security.User>();
        public DbSet<Auditing.CommandExecution> CommandExecutions { get; set; }
        public DbSet<Auditing.QueryExecution> QueryExecutions { get; set; }
        public DbSet<Alliances.Alliance> Alliances { get; set; }
        public DbSet<Players.Player> Players { get; set; }
        public DbSet<Events.ScheduledTask> ScheduledTasks { get; set; }

        public int CallsToSaveChanges { get; private set; }

        public int SaveChanges() {
            CallsToSaveChanges++;

            return 0;
        }

        public void Dispose() {
        }
    }
}