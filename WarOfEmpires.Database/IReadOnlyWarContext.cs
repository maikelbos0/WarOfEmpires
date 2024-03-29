﻿using Microsoft.EntityFrameworkCore;
using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Database {
    [TransientService(typeof(WarContext))]    
    public interface IReadOnlyWarContext {
        DbSet<ActionExecution> ActionExecutions { get; }
        DbSet<Alliance> Alliances { get; }
        DbSet<GameStatus> GameStatus { get; }
        DbSet<Player> Players { get; }
        DbSet<ScheduledTask> ScheduledTasks { get; }
        DbSet<User> Users { get; }
    }
}
