using WarOfEmpires.Database.Migrations;
using WarOfEmpires.Database.ReferenceEntities;
using WarOfEmpires.Utilities.Container;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Security = WarOfEmpires.Domain.Security;
using Auditing = WarOfEmpires.Domain.Auditing;

namespace WarOfEmpires.Database {
    [InterfaceInjectable]
    public sealed class WarContext : DbContext, IWarContext {
        static WarContext() {
            // When adding migrations the context can not be instantiated
            // So far I have not worked out how to detect that the model has changed manually
            try {
                using (var context = new WarContext()) {
                    context.Database.Initialize(false);
                }
            }
            catch (Exception) { }

            // The update command also tries to instantiate the context
            try {
                new DbMigrator(new Configuration()).Update();
            }
            catch (Exception) { }
        }

        public IDbSet<Security.User> Users { get; set; }
        public IDbSet<Auditing.CommandExecution> CommandExecutions { get; set; }
        public IDbSet<Auditing.QueryExecution> QueryExecutions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            var userEventTypes = modelBuilder.Entity<UserEventTypeEntity>().ToTable("UserEventTypes", "Security").HasKey(t => t.Id);

            userEventTypes.HasMany(t => t.UserEvents).WithRequired().HasForeignKey(e => e.Type);
            userEventTypes.Property(t => t.Name).IsRequired();

            var userStatus = modelBuilder.Entity<UserStatusEntity>().ToTable("UserStatus", "Security").HasKey(s => s.Id);

            userStatus.HasMany(t => t.Users).WithRequired().HasForeignKey(u => u.Status);
            userStatus.Property(t => t.Name).IsRequired();
            
            var users = modelBuilder.Entity<Security.User>().ToTable("Users", "Security").HasKey(u => u.Id);

            users.HasMany(u => u.UserEvents).WithRequired(e => e.User);
            users.HasIndex(u => u.Email).IsUnique();
            users.Property(u => u.Status).HasColumnName("UserStatus_Id");
            users.Property(u => u.Email).IsRequired().HasMaxLength(255);
            users.Property(u => u.Password.Salt).IsRequired().HasMaxLength(20);
            users.Property(u => u.Password.Hash).IsRequired().HasMaxLength(20);
            users.Property(u => u.Password.HashIterations).IsRequired();
            users.Property(u => u.PasswordResetToken.Salt).HasMaxLength(20);
            users.Property(u => u.PasswordResetToken.Hash).HasMaxLength(20);
            users.Property(u => u.NewEmail).HasMaxLength(255);
            users.Property(u => u.DisplayName).HasMaxLength(50);
            users.Property(u => u.Description).IsMaxLength();

            var userEvents = modelBuilder.Entity<Security.UserEvent>().ToTable("UserEvents", "Security").HasKey(e => e.Id);

            userEvents.Property(e => e.Type).HasColumnName("UserEventType_Id");

            var commandExecutions = modelBuilder.Entity<Auditing.CommandExecution>().ToTable("CommandExecutions", "Auditing").HasKey(e => e.Id);

            commandExecutions.Property(e => e.Date).IsRequired();
            commandExecutions.Property(e => e.CommandType).IsRequired().HasMaxLength(255);
            commandExecutions.Property(e => e.CommandData).IsRequired().IsMaxLength();

            var queryExecutions = modelBuilder.Entity<Auditing.QueryExecution>().ToTable("QueryExecutions", "Auditing").HasKey(e => e.Id);

            queryExecutions.Property(e => e.Date).IsRequired();
            queryExecutions.Property(e => e.QueryType).IsRequired().HasMaxLength(255);
            queryExecutions.Property(e => e.QueryData).IsRequired().IsMaxLength();
        }

        public void FixEfProviderServicesProblem() {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }
    }
}