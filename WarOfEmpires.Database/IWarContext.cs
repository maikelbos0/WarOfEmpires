using System;
using System.Data.Entity;
using Security = WarOfEmpires.Domain.Security;
using Auditing = WarOfEmpires.Domain.Auditing;

namespace WarOfEmpires.Database {
    public interface IWarContext : IDisposable {
        IDbSet<Security.User> Users { get; set; }
        IDbSet<Auditing.CommandExecution> CommandExecutions { get; set; }
        IDbSet<Auditing.QueryExecution> QueryExecutions { get; set; }
        int SaveChanges();
    }
}