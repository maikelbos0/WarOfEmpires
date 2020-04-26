using WarOfEmpires.Database;
using System.Data.Entity;
using Alliances = WarOfEmpires.Domain.Alliances;
using Auditing = WarOfEmpires.Domain.Auditing;
using Events = WarOfEmpires.Domain.Events;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;
using System.Linq;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeWarContext : IWarContext {
        public IDbSet<Security.User> Users { get; set; } = new FakeDbSet<Security.User>();
        public int CallsToSaveChanges { get; private set; }
        public IDbSet<Auditing.CommandExecution> CommandExecutions { get ; set ; } = new FakeDbSet<Auditing.CommandExecution>();
        public IDbSet<Alliances.Alliance> Alliances { get ; set ; } = new FakeDbSet<Alliances.Alliance>();
        public IDbSet<Auditing.QueryExecution> QueryExecutions { get; set; } = new FakeDbSet<Auditing.QueryExecution>();
        public IDbSet<Players.Player> Players { get; set; } = new FakeDbSet<Players.Player>();
        public IDbSet<Events.ScheduledTask> ScheduledTasks { get; set; } = new FakeDbSet<Events.ScheduledTask>();

        public void Dispose() {
            SaveChanges();
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class {
            var entityProperty = typeof(FakeWarContext).GetProperties().SingleOrDefault(p => p.PropertyType == typeof(IDbSet<TEntity>));

            if (entityProperty != null) {
                ((IDbSet<TEntity>)entityProperty.GetValue(this)).Remove(entity);
            }
        }

        public int SaveChanges() {
            CallsToSaveChanges++;

            // For now, implementation is not needed
            return 0;
        }
    }
}