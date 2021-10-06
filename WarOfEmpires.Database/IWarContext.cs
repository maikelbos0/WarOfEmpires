using Microsoft.EntityFrameworkCore;
using Alliances = WarOfEmpires.Domain.Alliances;
using Auditing = WarOfEmpires.Domain.Auditing;
using Events = WarOfEmpires.Domain.Events;
using Game = WarOfEmpires.Domain.Game;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Database {
    public interface IWarContext {
        DbSet<Game.GameStatus> GameStatus { get; }
        DbSet<Security.User> Users { get; }
        DbSet<Auditing.CommandExecution> CommandExecutions { get; }
        DbSet<Auditing.QueryExecution> QueryExecutions { get; }
        DbSet<Alliances.Alliance> Alliances { get; }
        DbSet<Players.Player> Players { get; }
        DbSet<Events.ScheduledTask> ScheduledTasks { get; }
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}