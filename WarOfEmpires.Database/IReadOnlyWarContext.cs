using Microsoft.EntityFrameworkCore;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Database {
    [ScopedService(typeof(WarContext))]    
    public interface IReadOnlyWarContext {
        DbSet<Alliance> Alliances { get; }
        DbSet<CommandExecution> CommandExecutions { get; }
        DbSet<GameStatus> GameStatus { get; }
        DbSet<Player> Players { get; }
        DbSet<QueryExecution> QueryExecutions { get; }
        DbSet<ScheduledTask> ScheduledTasks { get; }
        DbSet<User> Users { get; }
    }
}