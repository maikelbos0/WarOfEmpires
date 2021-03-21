using Microsoft.EntityFrameworkCore;
using System;
using Alliances = WarOfEmpires.Domain.Alliances;
using Auditing = WarOfEmpires.Domain.Auditing;
using Events = WarOfEmpires.Domain.Events;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Database {
    public interface IWarContext : IDisposable {
        DbSet<Security.User> Users { get; set; }
        DbSet<Auditing.CommandExecution> CommandExecutions { get; set; }
        DbSet<Auditing.QueryExecution> QueryExecutions { get; set; }
        DbSet<Alliances.Alliance> Alliances { get; set; }
        DbSet<Players.Player> Players { get; set; }
        DbSet<Events.ScheduledTask> ScheduledTasks { get; set; }
        int SaveChanges();
    }
}