using System;
using System.Data.Entity;
using Auditing = WarOfEmpires.Domain.Auditing;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Database {
    public interface IWarContext : IDisposable {
        IDbSet<Security.User> Users { get; set; }
        IDbSet<Auditing.CommandExecution> CommandExecutions { get; set; }
        IDbSet<Auditing.QueryExecution> QueryExecutions { get; set; }
        IDbSet<Players.Player> Players { get; set; }
        int SaveChanges();
    }
}