using WarOfEmpires.Database.Migrations;
using WarOfEmpires.Database.ReferenceEntities;
using WarOfEmpires.Utilities.Container;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Attacks = WarOfEmpires.Domain.Attacks;
using Auditing = WarOfEmpires.Domain.Auditing;
using Events = WarOfEmpires.Domain.Events;
using Empires = WarOfEmpires.Domain.Empires;
using Markets = WarOfEmpires.Domain.Markets;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;
using Siege = WarOfEmpires.Domain.Siege;

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
        public IDbSet<Players.Player> Players { get; set; }
        public IDbSet<Events.ScheduledTask> ScheduledTasks { get; set; }

        public void Remove<TEntity>(TEntity entity) where TEntity : class {
            Set<TEntity>().Remove(entity);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            OnAuditingModelCreating(modelBuilder);
            OnEventsModelCreating(modelBuilder);
            OnPlayersModelCreating(modelBuilder);
            OnAttacksModelCreating(modelBuilder);
            OnSecurityModelCreating(modelBuilder);
        }

        private void OnAuditingModelCreating(DbModelBuilder modelBuilder) {
            var commandExecutions = modelBuilder.Entity<Auditing.CommandExecution>().ToTable("CommandExecutions", "Auditing").HasKey(e => e.Id);
            commandExecutions.Property(e => e.Date).IsRequired();
            commandExecutions.Property(e => e.CommandType).IsRequired().HasMaxLength(255);
            commandExecutions.Property(e => e.CommandData).IsRequired().IsMaxLength();

            var queryExecutions = modelBuilder.Entity<Auditing.QueryExecution>().ToTable("QueryExecutions", "Auditing").HasKey(e => e.Id);
            queryExecutions.Property(e => e.Date).IsRequired();
            queryExecutions.Property(e => e.QueryType).IsRequired().HasMaxLength(255);
            queryExecutions.Property(e => e.QueryData).IsRequired().IsMaxLength();
        }

        private void OnEventsModelCreating(DbModelBuilder modelBuilder) {
            var scheduledTasks = modelBuilder.Entity<Events.ScheduledTask>().ToTable("ScheduledTasks", "Events").HasKey(t => t.Id);
            scheduledTasks.Property(e => e.EventType).IsRequired();
        }

        private void OnPlayersModelCreating(DbModelBuilder modelBuilder) {
            var buildingTypes = modelBuilder.Entity<BuildingTypeEntity>().ToTable("BuildingTypes", "Empires").HasKey(t => t.Id);
            buildingTypes.HasMany(t => t.Buildings).WithRequired().HasForeignKey(b => b.Type);
            buildingTypes.Property(t => t.Name).IsRequired();

            var siegeWeaponTypes = modelBuilder.Entity<SiegeWeaponTypeEntity>().ToTable("SiegeWeaponTypes", "Siege").HasKey(t => t.Id);
            siegeWeaponTypes.HasMany(t => t.SiegeWeapons).WithRequired().HasForeignKey(b => b.Type);
            siegeWeaponTypes.Property(t => t.Name).IsRequired();

            var players = modelBuilder.Entity<Players.Player>().ToTable("Players", "Players").HasKey(p => p.Id);
            players.HasRequired(p => p.User).WithOptional();
            players.HasMany(p => p.Workers).WithRequired();
            players.HasMany(p => p.Troops).WithRequired();
            players.HasMany(p => p.SiegeWeapons).WithRequired();
            players.HasMany(p => p.Buildings).WithRequired();
            players.HasMany(p => p.SentMessages).WithRequired(m => m.Sender).WillCascadeOnDelete(false);
            players.HasMany(p => p.ReceivedMessages).WithRequired(m => m.Recipient);
            players.HasMany(p => p.ExecutedAttacks).WithRequired(a => a.Attacker).WillCascadeOnDelete(false);
            players.HasMany(p => p.ReceivedAttacks).WithRequired(a => a.Defender).WillCascadeOnDelete(false);
            players.HasMany(p => p.BuyTransactions).WithRequired().Map(a => a.MapKey("Buyer_Id"));
            players.HasMany(p => p.SellTransactions).WithRequired().Map(a => a.MapKey("Seller_Id")).WillCascadeOnDelete(false);
            players.HasMany(p => p.Caravans).WithRequired(a => a.Player);
            players.Property(p => p.DisplayName).IsRequired().HasMaxLength(25);
            players.Property(p => p.Resources.Gold).HasColumnName("Gold");
            players.Property(p => p.Resources.Food).HasColumnName("Food");
            players.Property(p => p.Resources.Wood).HasColumnName("Wood");
            players.Property(p => p.Resources.Stone).HasColumnName("Stone");
            players.Property(p => p.Resources.Ore).HasColumnName("Ore");
            players.Property(p => p.BankedResources.Gold).HasColumnName("BankedGold");
            players.Property(p => p.BankedResources.Food).HasColumnName("BankedFood");
            players.Property(p => p.BankedResources.Wood).HasColumnName("BankedWood");
            players.Property(p => p.BankedResources.Stone).HasColumnName("BankedStone");
            players.Property(p => p.BankedResources.Ore).HasColumnName("BankedOre");

            var workerTypes = modelBuilder.Entity<WorkerTypeEntity>().ToTable("WorkerTypes", "Empires").HasKey(t => t.Id);
            workerTypes.HasMany(w => w.Workers).WithRequired().HasForeignKey(w => w.Type);
            workerTypes.Property(w => w.Name).IsRequired();

            modelBuilder.Entity<Empires.Workers>().ToTable("Workers", "Empires").HasKey(t => t.Id);

            modelBuilder.Entity<Attacks.Troops>().ToTable("Troops", "Attacks").HasKey(t => t.Id);

            modelBuilder.Entity<Empires.Building>().ToTable("Buildings", "Empires").HasKey(b => b.Id);

            modelBuilder.Entity<Siege.SiegeWeapon>().ToTable("SiegeWeapons", "Siege").HasKey(b => b.Id);

            var messages = modelBuilder.Entity<Players.Message>().ToTable("Messages", "Players").HasKey(m => m.Id);
            messages.Property(m => m.Subject).IsRequired().HasMaxLength(100);
            messages.Property(m => m.Body).IsMaxLength();

            var caravans = modelBuilder.Entity<Markets.Caravan>().ToTable("Caravans", "Markets");
            caravans.HasMany(c => c.Merchandise).WithRequired();

            modelBuilder.Entity<Markets.Merchandise>().ToTable("Merchandise", "Markets");

            var merchandiseTypes = modelBuilder.Entity<MerchandiseTypeEntity>().ToTable("MerchandiseTypes", "Markets").HasKey(t => t.Id);
            merchandiseTypes.HasMany(m => m.Merchandise).WithRequired().HasForeignKey(m => m.Type);
            merchandiseTypes.Property(m => m.Name).IsRequired();

            modelBuilder.Entity<Markets.Transaction>().ToTable("Transactions", "Markets");
        }

        private void OnAttacksModelCreating(DbModelBuilder modelBuilder) {
            var attackResults = modelBuilder.Entity<AttackResultEntity>().ToTable("AttackResults", "Attacks").HasKey(r => r.Id);
            attackResults.HasMany(r => r.Attacks).WithRequired().HasForeignKey(a => a.Result);
            attackResults.Property(r => r.Name).IsRequired();

            var attackTypes = modelBuilder.Entity<AttackTypeEntity>().ToTable("AttackTypes", "Attacks").HasKey(r => r.Id);
            attackTypes.HasMany(r => r.Attacks).WithRequired().HasForeignKey(a => a.Type);
            attackTypes.Property(r => r.Name).IsRequired();

            var attacks = modelBuilder.Entity<Attacks.Attack>().ToTable("Attacks", "Attacks").HasKey(a => a.Id);
            attacks.HasMany(a => a.Rounds).WithRequired();
            attacks.Property(a => a.Resources.Gold).HasColumnName("Gold");
            attacks.Property(a => a.Resources.Food).HasColumnName("Food");
            attacks.Property(a => a.Resources.Wood).HasColumnName("Wood");
            attacks.Property(a => a.Resources.Stone).HasColumnName("Stone");
            attacks.Property(a => a.Resources.Ore).HasColumnName("Ore");

            var troopTypes = modelBuilder.Entity<TroopTypeEntity>().ToTable("TroopTypes", "Attacks").HasKey(t => t.Id);
            troopTypes.HasMany(t => t.AttackRounds).WithRequired().HasForeignKey(r => r.TroopType);
            troopTypes.HasMany(t => t.Troops).WithRequired().HasForeignKey(t => t.Type);
            troopTypes.Property(t => t.Name).IsRequired();

            modelBuilder.Entity<Attacks.AttackRound>().ToTable("AttackRounds", "Attacks").HasKey(r => r.Id);

            modelBuilder.Entity<Attacks.Casualties>().ToTable("Casualties", "Attacks").HasKey(c => c.Id);
        }

        private void OnSecurityModelCreating(DbModelBuilder modelBuilder) {
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

            var userEvents = modelBuilder.Entity<Security.UserEvent>().ToTable("UserEvents", "Security").HasKey(e => e.Id);
            userEvents.Property(e => e.Type).HasColumnName("UserEventType_Id");
        }

        public void FixEfProviderServicesProblem() {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
        }
    }
}