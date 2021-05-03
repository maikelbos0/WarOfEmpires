using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database.ReferenceEntities;
using WarOfEmpires.Utilities.Configuration;
using Alliances = WarOfEmpires.Domain.Alliances;
using Attacks = WarOfEmpires.Domain.Attacks;
using Auditing = WarOfEmpires.Domain.Auditing;
using Empires = WarOfEmpires.Domain.Empires;
using Events = WarOfEmpires.Domain.Events;
using Markets = WarOfEmpires.Domain.Markets;
using Players = WarOfEmpires.Domain.Players;
using Security = WarOfEmpires.Domain.Security;
using Siege = WarOfEmpires.Domain.Siege;

namespace WarOfEmpires.Database {
    [ScopedServiceImplementation(typeof(IWarContext))]
    public sealed class WarContext : DbContext, IWarContext {
        public DbSet<Security.User> Users { get; set; }
        public DbSet<Auditing.CommandExecution> CommandExecutions { get; set; }
        public DbSet<Auditing.QueryExecution> QueryExecutions { get; set; }
        public DbSet<Alliances.Alliance> Alliances { get; set; }
        public DbSet<Players.Player> Players { get; set; }
        public DbSet<Events.ScheduledTask> ScheduledTasks { get; set; }

        public WarContext(AppSettings appSettings) : base(new DbContextOptionsBuilder<WarContext>().UseSqlServer(appSettings.DatabaseConnectionString).Options) { }

        public override int SaveChanges() {
            DeleteOrphanedCaravans();
            DeleteOrphanedChatMessages();
            DeleteOrphanedInvites();
            DeleteOrphanedMerchandise();
            DeleteOrphanedNonAggressionPactRequests();
            DeleteOrphanedRoles();

            return base.SaveChanges();
        }

        private IEnumerable<TEntity> GetChangeTrackerEntities<TEntity>() {
            return ChangeTracker.Entries().Where(e => e.State != EntityState.Deleted).Select(e => e.Entity).OfType<TEntity>();
        }

        private void DeleteOrphanedCaravans() {
            var orphans = GetChangeTrackerEntities<Markets.Caravan>()
                .Except(GetChangeTrackerEntities<Players.Player>().SelectMany(p => p.Caravans));

            Set<Markets.Caravan>().RemoveRange(orphans);
        }

        private void DeleteOrphanedChatMessages() {
            var orphans = GetChangeTrackerEntities<Alliances.ChatMessage>()
                .Except(GetChangeTrackerEntities<Alliances.Alliance>().SelectMany(a => a.ChatMessages));

            Set<Alliances.ChatMessage>().RemoveRange(orphans);
        }

        private void DeleteOrphanedInvites() {
            var orphans = GetChangeTrackerEntities<Alliances.Invite>()
                .Except(GetChangeTrackerEntities<Alliances.Alliance>().SelectMany(a => a.Invites))
                .Except(GetChangeTrackerEntities<Players.Player>().SelectMany(p => p.Invites));

            Set<Alliances.Invite>().RemoveRange(orphans);
        }

        private void DeleteOrphanedMerchandise() {
            var orphans = GetChangeTrackerEntities<Markets.Merchandise>()
                .Except(GetChangeTrackerEntities<Markets.Caravan>().SelectMany(c => c.Merchandise));

            Set<Markets.Merchandise>().RemoveRange(orphans);
        }

        private void DeleteOrphanedNonAggressionPactRequests() {
            var orphans = GetChangeTrackerEntities<Alliances.NonAggressionPactRequest>()
                .Except(GetChangeTrackerEntities<Alliances.Alliance>().SelectMany(a => a.SentNonAggressionPactRequests))
                .Except(GetChangeTrackerEntities<Alliances.Alliance>().SelectMany(a => a.ReceivedNonAggressionPactRequests));

            Set<Alliances.NonAggressionPactRequest>().RemoveRange(orphans);
        }

        private void DeleteOrphanedRoles() {
            var orphans = GetChangeTrackerEntities<Alliances.Role>()
                .Except(GetChangeTrackerEntities<Alliances.Alliance>().SelectMany(a => a.Roles));

            Set<Alliances.Role>().RemoveRange(orphans);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            OnAuditingModelCreating(modelBuilder);
            OnEventsModelCreating(modelBuilder);
            OnPlayersModelCreating(modelBuilder);
            OnAllianceModelCreating(modelBuilder);
            OnAttacksModelCreating(modelBuilder);
            OnSecurityModelCreating(modelBuilder);
        }

        private void OnAuditingModelCreating(ModelBuilder modelBuilder) {
            var commandExecutions = modelBuilder.Entity<Auditing.CommandExecution>().ToTable("CommandExecutions", "Auditing");
            commandExecutions.HasKey(e => e.Id);
            commandExecutions.Property(e => e.Date).IsRequired();
            commandExecutions.Property(e => e.CommandType).IsRequired().HasMaxLength(255);
            commandExecutions.Property(e => e.CommandData).IsRequired();

            var queryExecutions = modelBuilder.Entity<Auditing.QueryExecution>().ToTable("QueryExecutions", "Auditing");
            queryExecutions.HasKey(e => e.Id);
            queryExecutions.Property(e => e.Date).IsRequired();
            queryExecutions.Property(e => e.QueryType).IsRequired().HasMaxLength(255);
            queryExecutions.Property(e => e.QueryData).IsRequired();
        }

        private void OnEventsModelCreating(ModelBuilder modelBuilder) {
            var executionModes = modelBuilder.Entity<TaskExecutionModeEntity>().ToTable("TaskExecutionModes", "Events");
            executionModes.HasKey(t => t.Id);
            executionModes.HasMany(m => m.ScheduledTasks).WithOne().IsRequired().HasForeignKey(e => e.ExecutionMode);
            executionModes.Property(m => m.Name).IsRequired();
            executionModes.HasData(ReferenceEntityExtensions.GetValues<Events.TaskExecutionMode, TaskExecutionModeEntity>());

            var scheduledTasks = modelBuilder.Entity<Events.ScheduledTask>().ToTable("ScheduledTasks", "Events");
            scheduledTasks.HasKey(t => t.Id);
            scheduledTasks.Property(e => e.EventType).IsRequired();
            scheduledTasks.HasData(
                Events.ScheduledTask.Create<Empires.RecruitTaskTriggeredEvent>(1, new TimeSpan(1, 0, 0), Events.TaskExecutionMode.ExecuteAllIntervals),
                Events.ScheduledTask.Create<Empires.TurnTaskTriggeredEvent>(2, new TimeSpan(0, 10, 0), Events.TaskExecutionMode.ExecuteAllIntervals),
                Events.ScheduledTask.Create<Empires.BankTurnTaskTriggeredEvent>(3, new TimeSpan(4, 0, 0), Events.TaskExecutionMode.ExecuteAllIntervals),
                Events.ScheduledTask.Create<Empires.UpdateRankTaskTriggeredEvent>(4, new TimeSpan(0, 2, 0), Events.TaskExecutionMode.ExecuteOnce)
            );
        }

        private void OnPlayersModelCreating(ModelBuilder modelBuilder) {
            var buildingTypes = modelBuilder.Entity<BuildingTypeEntity>().ToTable("BuildingTypes", "Empires");
            buildingTypes.HasKey(t => t.Id);
            buildingTypes.HasMany(t => t.Buildings).WithOne().IsRequired().HasForeignKey(b => b.Type);
            buildingTypes.Property(t => t.Name).IsRequired();
            buildingTypes.HasData(ReferenceEntityExtensions.GetValues<Empires.BuildingType, BuildingTypeEntity>());

            var siegeWeaponTypes = modelBuilder.Entity<SiegeWeaponTypeEntity>().ToTable("SiegeWeaponTypes", "Siege");
            siegeWeaponTypes.HasKey(t => t.Id);
            siegeWeaponTypes.HasMany(t => t.SiegeWeapons).WithOne().IsRequired().HasForeignKey(b => b.Type);
            siegeWeaponTypes.Property(t => t.Name).IsRequired();
            siegeWeaponTypes.HasData(ReferenceEntityExtensions.GetValues<Siege.SiegeWeaponType, SiegeWeaponTypeEntity>());

            var players = modelBuilder.Entity<Players.Player>().ToTable("Players", "Players");
            players.HasKey(p => p.Id);
            players.HasOne(p => p.User).WithOne().HasForeignKey<Players.Player>(p => p.Id);
            players.HasMany(p => p.Workers).WithOne().IsRequired();
            players.HasMany(p => p.Troops).WithOne().IsRequired();
            players.HasMany(p => p.SiegeWeapons).WithOne().IsRequired();
            players.HasMany(p => p.Buildings).WithOne().IsRequired();
            players.HasMany(p => p.SentMessages).WithOne(m => m.Sender).IsRequired().OnDelete(DeleteBehavior.NoAction);
            players.HasMany(p => p.ReceivedMessages).WithOne(m => m.Recipient).IsRequired();
            players.HasMany(p => p.ExecutedAttacks).WithOne(a => a.Attacker).IsRequired().OnDelete(DeleteBehavior.NoAction);
            players.HasMany(p => p.ReceivedAttacks).WithOne(a => a.Defender).IsRequired();
            players.HasMany(p => p.SellTransactions).WithOne().IsRequired().OnDelete(DeleteBehavior.NoAction).HasForeignKey("SellerId");
            players.HasMany(p => p.BuyTransactions).WithOne().IsRequired().HasForeignKey("BuyerId");
            players.HasMany(p => p.Caravans).WithOne(c => c.Player).IsRequired();
            players.HasMany(p => p.Invites).WithOne(i => i.Player).IsRequired().OnDelete(DeleteBehavior.NoAction);
            players.Property(p => p.DisplayName).IsRequired().HasMaxLength(25);
            players.OwnsOne(p => p.Resources, pr => {
                pr.Property(r => r.Gold).HasColumnName("Gold");
                pr.Property(r => r.Food).HasColumnName("Food");
                pr.Property(r => r.Wood).HasColumnName("Wood");
                pr.Property(r => r.Stone).HasColumnName("Stone");
                pr.Property(r => r.Ore).HasColumnName("Ore");
            });
            players.Navigation(a => a.Resources).IsRequired();
            players.OwnsOne(p => p.BankedResources, pr => {
                pr.Property(r => r.Gold).HasColumnName("BankedGold");
                pr.Property(r => r.Food).HasColumnName("BankedFood");
                pr.Property(r => r.Wood).HasColumnName("BankedWood");
                pr.Property(r => r.Stone).HasColumnName("BankedStone");
                pr.Property(r => r.Ore).HasColumnName("BankedOre");
            });
            players.Navigation(a => a.BankedResources).IsRequired();

            var titleTypes = modelBuilder.Entity<TitleTypeEntity>().ToTable("TitleTypes", "Players");
            titleTypes.HasKey(t => t.Id);
            titleTypes.HasMany(t => t.Players).WithOne().IsRequired().HasForeignKey(p => p.Title);
            titleTypes.Property(t => t.Name).IsRequired();
            titleTypes.HasData(ReferenceEntityExtensions.GetValues<Players.TitleType, TitleTypeEntity>());

            var workerTypes = modelBuilder.Entity<WorkerTypeEntity>().ToTable("WorkerTypes", "Empires");
            workerTypes.HasKey(w => w.Id);
            workerTypes.HasMany(w => w.Workers).WithOne().IsRequired().HasForeignKey(w => w.Type);
            workerTypes.Property(w => w.Name).IsRequired();
            workerTypes.HasData(ReferenceEntityExtensions.GetValues<Empires.WorkerType, WorkerTypeEntity>());

            modelBuilder.Entity<Empires.Workers>().ToTable("Workers", "Empires").HasKey(t => t.Id);

            modelBuilder.Entity<Attacks.Troops>().ToTable("Troops", "Attacks").HasKey(t => t.Id);

            modelBuilder.Entity<Empires.Building>().ToTable("Buildings", "Empires").HasKey(b => b.Id);

            modelBuilder.Entity<Siege.SiegeWeapon>().ToTable("SiegeWeapons", "Siege").HasKey(w => w.Id);

            var messages = modelBuilder.Entity<Players.Message>().ToTable("Messages", "Players");
            messages.HasKey(m => m.Id);
            messages.Property(m => m.Subject).IsRequired().HasMaxLength(100);

            var caravans = modelBuilder.Entity<Markets.Caravan>().ToTable("Caravans", "Markets");
            caravans.HasMany(c => c.Merchandise).WithOne().IsRequired();

            modelBuilder.Entity<Markets.Merchandise>().ToTable("Merchandise", "Markets");

            var merchandiseTypes = modelBuilder.Entity<MerchandiseTypeEntity>().ToTable("MerchandiseTypes", "Markets");
            merchandiseTypes.HasKey(t => t.Id);
            merchandiseTypes.HasMany(m => m.Merchandise).WithOne().IsRequired().HasForeignKey(m => m.Type);
            merchandiseTypes.HasMany(m => m.Transactions).WithOne().IsRequired().HasForeignKey(m => m.Type);
            merchandiseTypes.Property(m => m.Name).IsRequired();
            merchandiseTypes.HasData(ReferenceEntityExtensions.GetValues<Markets.MerchandiseType, MerchandiseTypeEntity>());

            modelBuilder.Entity<Markets.Transaction>().ToTable("Transactions", "Markets");
        }

        private void OnAllianceModelCreating(ModelBuilder modelBuilder) {
            var alliances = modelBuilder.Entity<Alliances.Alliance>().ToTable("Alliances", "Alliances");
            alliances.HasKey(a => a.Id);
            alliances.HasMany(a => a.Members).WithOne(p => p.Alliance);
            alliances.HasMany(a => a.Invites).WithOne(i => i.Alliance).IsRequired();
            alliances.HasMany(a => a.Roles).WithOne(r => r.Alliance).IsRequired();
            alliances.HasMany(a => a.NonAggressionPacts).WithMany(p => p.Alliances).UsingEntity<Dictionary<string, object>>(
                "AllianceNonAggressionPacts",
                n => n.HasOne<Alliances.NonAggressionPact>().WithMany().HasForeignKey("NonAggressionPactId"),
                n => n.HasOne<Alliances.Alliance>().WithMany().HasForeignKey("AllianceId")
            ).ToTable("AllianceNonAggressionPacts", "Alliances");
            alliances.HasMany(a => a.SentNonAggressionPactRequests).WithOne(r => r.Sender).IsRequired().OnDelete(DeleteBehavior.NoAction);
            alliances.HasMany(a => a.ReceivedNonAggressionPactRequests).WithOne(r => r.Recipient).IsRequired();
            alliances.HasMany(a => a.ChatMessages).WithOne().IsRequired();
            alliances.HasOne(a => a.Leader).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            alliances.Property(a => a.Code).IsRequired().HasMaxLength(4);
            alliances.Property(a => a.Name).IsRequired();

            var invites = modelBuilder.Entity<Alliances.Invite>().ToTable("Invites", "Alliances");
            invites.HasKey(i => i.Id);
            invites.Property(i => i.Subject).IsRequired().HasMaxLength(100);

            var chatMessages = modelBuilder.Entity<Alliances.ChatMessage>().ToTable("ChatMessages", "Alliances");
            chatMessages.HasKey(m => m.Id);
            chatMessages.HasOne(m => m.Player).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
            chatMessages.Property(m => m.Message).IsRequired();

            var roles = modelBuilder.Entity<Alliances.Role>().ToTable("Roles", "Alliances");
            roles.HasKey(r => r.Id);
            roles.Property(r => r.Name).IsRequired();

            modelBuilder.Entity<Alliances.NonAggressionPact>().ToTable("NonAggressionPacts", "Alliances").HasKey(m => m.Id);

            modelBuilder.Entity<Alliances.NonAggressionPactRequest>().ToTable("NonAggressionPactRequests", "Alliances").HasKey(m => m.Id);
        }

        private void OnAttacksModelCreating(ModelBuilder modelBuilder) {
            var attackResults = modelBuilder.Entity<AttackResultEntity>().ToTable("AttackResults", "Attacks");
            attackResults.HasKey(r => r.Id);
            attackResults.HasMany(r => r.Attacks).WithOne().IsRequired().HasForeignKey(a => a.Result);
            attackResults.Property(r => r.Name).IsRequired();
            attackResults.HasData(ReferenceEntityExtensions.GetValues<Attacks.AttackResult, AttackResultEntity>());

            var attackTypes = modelBuilder.Entity<AttackTypeEntity>().ToTable("AttackTypes", "Attacks");
            attackTypes.HasKey(r => r.Id);
            attackTypes.HasMany(r => r.Attacks).WithOne().IsRequired().HasForeignKey(a => a.Type);
            attackTypes.Property(r => r.Name).IsRequired();
            attackTypes.HasData(ReferenceEntityExtensions.GetValues<Attacks.AttackType, AttackTypeEntity>());

            var attacks = modelBuilder.Entity<Attacks.Attack>().ToTable("Attacks", "Attacks");
            attacks.HasKey(a => a.Id);
            attacks.HasMany(a => a.Rounds).WithOne().IsRequired();
            attacks.OwnsOne(a => a.Resources, ar => {
                ar.Property(r => r.Gold).HasColumnName("Gold");
                ar.Property(r => r.Food).HasColumnName("Food");
                ar.Property(r => r.Wood).HasColumnName("Wood");
                ar.Property(r => r.Stone).HasColumnName("Stone");
                ar.Property(r => r.Ore).HasColumnName("Ore");
            });
            attacks.Navigation(a => a.Resources).IsRequired();
            attacks.HasDiscriminator<string>("AttackType")
                .HasValue<Attacks.Assault>(nameof(Attacks.Assault))
                .HasValue<Attacks.Raid>(nameof(Attacks.Raid));

            var troopTypes = modelBuilder.Entity<TroopTypeEntity>().ToTable("TroopTypes", "Attacks");
            troopTypes.HasKey(t => t.Id);
            troopTypes.HasMany(t => t.AttackRounds).WithOne().IsRequired().HasForeignKey(r => r.TroopType);
            troopTypes.HasMany(t => t.Casualties).WithOne().IsRequired().HasForeignKey(c => c.TroopType).OnDelete(DeleteBehavior.NoAction);
            troopTypes.HasMany(t => t.Troops).WithOne().IsRequired().HasForeignKey(t => t.Type);
            troopTypes.Property(t => t.Name).IsRequired();
            troopTypes.HasData(ReferenceEntityExtensions.GetValues<Attacks.TroopType, TroopTypeEntity>());

            var attackRounds = modelBuilder.Entity<Attacks.AttackRound>().ToTable("AttackRounds", "Attacks");
            attackRounds.HasKey(r => r.Id);
            attackRounds.HasMany(r => r.Casualties).WithOne().IsRequired();

            modelBuilder.Entity<Attacks.Casualties>().ToTable("Casualties", "Attacks").HasKey(c => c.Id);
        }

        private void OnSecurityModelCreating(ModelBuilder modelBuilder) {
            var userEventTypes = modelBuilder.Entity<UserEventTypeEntity>().ToTable("UserEventTypes", "Security");
            userEventTypes.HasKey(t => t.Id);
            userEventTypes.HasMany(t => t.UserEvents).WithOne().IsRequired().HasForeignKey(e => e.Type);
            userEventTypes.Property(t => t.Name).IsRequired();
            userEventTypes.HasData(ReferenceEntityExtensions.GetValues<Security.UserEventType, UserEventTypeEntity>());

            var userStatus = modelBuilder.Entity<UserStatusEntity>().ToTable("UserStatus", "Security");
            userStatus.HasKey(s => s.Id);
            userStatus.HasMany(t => t.Users).WithOne().IsRequired().HasForeignKey(u => u.Status);
            userStatus.Property(t => t.Name).IsRequired();
            userStatus.HasData(ReferenceEntityExtensions.GetValues<Security.UserStatus, UserStatusEntity>());

            var users = modelBuilder.Entity<Security.User>().ToTable("Users", "Security");
            users.HasKey(u => u.Id);
            users.HasMany(u => u.UserEvents).WithOne(e => e.User).IsRequired();
            users.HasIndex(u => u.Email).IsUnique();
            users.Property(u => u.Status);
            users.Property(u => u.Email).IsRequired().HasMaxLength(255);
            users.OwnsOne(u => u.Password, up => {
                up.Property(p => p.Salt).HasColumnName("PasswordSalt").IsRequired().HasMaxLength(20);
                up.Property(p => p.Hash).HasColumnName("PasswordHash").IsRequired().HasMaxLength(20);
                up.Property(p => p.HashIterations).HasColumnName("PasswordHashIterations").IsRequired();
            });
            users.Navigation(u => u.Password).IsRequired();
            users.OwnsOne(u => u.PasswordResetToken, up => {
                up.Property(p => p.Salt).HasColumnName("PasswordResetTokenSalt").IsRequired().HasMaxLength(20);
                up.Property(p => p.Hash).HasColumnName("PasswordResetTokenHash").IsRequired().HasMaxLength(20);
                up.Property(p => p.HashIterations).HasColumnName("PasswordResetTokenHashIterations").IsRequired();
                up.Property(p => p.ExpiryDate).HasColumnName("PasswordResetTokenExpiryDate").IsRequired();
            });
            users.Property(u => u.NewEmail).HasMaxLength(255);

            var userEvents = modelBuilder.Entity<Security.UserEvent>().ToTable("UserEvents", "Security");
            userEvents.HasKey(e => e.Id);
        }
    }
}