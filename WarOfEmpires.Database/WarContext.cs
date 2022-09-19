using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Database.ReferenceEntities;
using WarOfEmpires.Utilities.Configuration;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Domain.Siege;

namespace WarOfEmpires.Database {
    public class WarContext : DbContext, IWarContext {
        public DbSet<GameStatus> GameStatus { get; private set; }
        public DbSet<User> Users { get; private set; }
        public DbSet<ActionExecution> ActionExecutions { get; private set; }
        public DbSet<Alliance> Alliances { get; private set; }
        public DbSet<Player> Players { get; private set; }
        public DbSet<ScheduledTask> ScheduledTasks { get; private set; }

        public WarContext(AppSettings appSettings)
            : base(new DbContextOptionsBuilder<WarContext>()
                  .UseSqlServer(appSettings.DatabaseConnectionString)
                  .UseLazyLoadingProxies()
                  .Options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            OnGameModelCreating(modelBuilder);
            OnAuditingModelCreating(modelBuilder);
            OnEventsModelCreating(modelBuilder);
            OnPlayersModelCreating(modelBuilder);
            OnAllianceModelCreating(modelBuilder);
            OnAttacksModelCreating(modelBuilder);
            OnSecurityModelCreating(modelBuilder);
        }

        private static void OnGameModelCreating(ModelBuilder modelBuilder) {
            var gamePhase = modelBuilder.Entity<GamePhaseEntity>().ToTable("GamePhases", "Game");
            gamePhase.HasKey(p => p.Id);
            gamePhase.HasMany(p => p.GameStatus).WithOne().IsRequired().HasForeignKey(s => s.Phase);
            gamePhase.Property(p => p.Name).IsRequired();
            gamePhase.HasData(ReferenceEntityExtensions.GetValues<GamePhase, GamePhaseEntity>());

            var gameStatus = modelBuilder.Entity<GameStatus>().ToTable("GameStatus", "Game");
            gameStatus.HasKey(s => s.Id);
            gameStatus.Property(s => s.Id).ValueGeneratedNever();
            gameStatus.HasOne(s => s.CurrentGrandOverlord).WithMany();
            gameStatus.HasData(new GameStatus() { Id = 1, Phase = GamePhase.Truce });
        }

        private static void OnAuditingModelCreating(ModelBuilder modelBuilder) {
            var actionExecutions = modelBuilder.Entity<ActionExecution>().ToTable("ActionExecutions", "Auditing");
            actionExecutions.HasKey(e => e.Id);
            actionExecutions.Property(e => e.Date).IsRequired();
            actionExecutions.Property(e => e.Type).IsRequired().HasMaxLength(255);
            actionExecutions.Property(e => e.Method).IsRequired().HasMaxLength(255);
            actionExecutions.Property(e => e.Data).IsRequired();
        }

        private static void OnEventsModelCreating(ModelBuilder modelBuilder) {
            var executionModes = modelBuilder.Entity<TaskExecutionModeEntity>().ToTable("TaskExecutionModes", "Events");
            executionModes.HasKey(t => t.Id);
            executionModes.HasMany(m => m.ScheduledTasks).WithOne().IsRequired().HasForeignKey(e => e.ExecutionMode);
            executionModes.Property(m => m.Name).IsRequired();
            executionModes.HasData(ReferenceEntityExtensions.GetValues<TaskExecutionMode, TaskExecutionModeEntity>());

            var scheduledTasks = modelBuilder.Entity<ScheduledTask>().ToTable("ScheduledTasks", "Events");
            scheduledTasks.HasKey(t => t.Id);
            scheduledTasks.Property(e => e.EventType).IsRequired();
            scheduledTasks.HasData(
                ScheduledTask.Create<RecruitTaskTriggeredEvent>(1, new TimeSpan(1, 0, 0), TaskExecutionMode.ExecuteAllIntervals),
                ScheduledTask.Create<TurnTaskTriggeredEvent>(2, new TimeSpan(0, 10, 0), TaskExecutionMode.ExecuteAllIntervals),
                ScheduledTask.Create<Domain.Empires.BankTurnTaskTriggeredEvent>(3, new TimeSpan(4, 0, 0), TaskExecutionMode.ExecuteAllIntervals),
                ScheduledTask.Create<UpdateRankTaskTriggeredEvent>(4, new TimeSpan(0, 1, 0), TaskExecutionMode.ExecuteOnce),
                ScheduledTask.Create<Domain.Alliances.BankTurnTaskTriggeredEvent>(5, new TimeSpan(1, 0, 0), TaskExecutionMode.ExecuteAllIntervals)
            );
        }

        private static void OnPlayersModelCreating(ModelBuilder modelBuilder) {
            var buildingTypes = modelBuilder.Entity<BuildingTypeEntity>().ToTable("BuildingTypes", "Empires");
            buildingTypes.HasKey(t => t.Id);
            buildingTypes.HasMany(t => t.Buildings).WithOne().IsRequired().HasForeignKey(b => b.Type);
            buildingTypes.Property(t => t.Name).IsRequired();
            buildingTypes.HasData(ReferenceEntityExtensions.GetValues<BuildingType, BuildingTypeEntity>());

            var siegeWeaponTypes = modelBuilder.Entity<SiegeWeaponTypeEntity>().ToTable("SiegeWeaponTypes", "Siege");
            siegeWeaponTypes.HasKey(t => t.Id);
            siegeWeaponTypes.HasMany(t => t.SiegeWeapons).WithOne().IsRequired().HasForeignKey(b => b.Type);
            siegeWeaponTypes.Property(t => t.Name).IsRequired();
            siegeWeaponTypes.HasData(ReferenceEntityExtensions.GetValues<SiegeWeaponType, SiegeWeaponTypeEntity>());

            var players = modelBuilder.Entity<Player>().ToTable("Players", "Players");
            players.HasKey(p => p.Id);
            players.HasOne(p => p.User).WithOne().HasForeignKey<Player>(p => p.Id);
            players.HasOne(p => p.Profile).WithOne().HasForeignKey<Profile>(p => p.Id);
            players.HasMany(p => p.Workers).WithOne().IsRequired();
            players.HasMany(p => p.Troops).WithOne().IsRequired();
            players.HasMany(p => p.SiegeWeapons).WithOne().IsRequired();
            players.HasMany(p => p.Buildings).WithOne().IsRequired();
            players.HasMany(p => p.SentMessages).WithOne(m => m.Sender).IsRequired().OnDelete(DeleteBehavior.NoAction);
            players.HasMany(p => p.ReceivedMessages).WithOne(m => m.Recipient).IsRequired();
            players.HasMany(p => p.PlayerBlocks).WithOne().IsRequired();
            players.HasMany(p => p.ExecutedAttacks).WithOne(a => a.Attacker).IsRequired().OnDelete(DeleteBehavior.NoAction);
            players.HasMany(p => p.ReceivedAttacks).WithOne(a => a.Defender).IsRequired();
            players.HasMany(p => p.SellTransactions).WithOne().IsRequired().OnDelete(DeleteBehavior.NoAction).HasForeignKey("SellerId");
            players.HasMany(p => p.BuyTransactions).WithOne().IsRequired().HasForeignKey("BuyerId");
            players.HasMany(p => p.Caravans).WithOne(c => c.Player).IsRequired();
            players.HasMany(p => p.Invites).WithOne(i => i.Player).IsRequired().OnDelete(DeleteBehavior.NoAction);
            players.HasMany(p => p.QueuedResearch).WithOne(r => r.Player).IsRequired();
            players.HasMany(p => p.Research).WithOne().IsRequired();
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
            players.Property(p => p.GrandOverlordTime).HasConversion(
                t => t.Ticks,
                t => TimeSpan.FromTicks(t)
            );

            var titleTypes = modelBuilder.Entity<TitleTypeEntity>().ToTable("TitleTypes", "Players");
            titleTypes.HasKey(t => t.Id);
            titleTypes.HasMany(t => t.Players).WithOne().IsRequired().HasForeignKey(p => p.Title);
            titleTypes.Property(t => t.Name).IsRequired();
            titleTypes.HasData(ReferenceEntityExtensions.GetValues<TitleType, TitleTypeEntity>());

            var workerTypes = modelBuilder.Entity<WorkerTypeEntity>().ToTable("WorkerTypes", "Empires");
            workerTypes.HasKey(w => w.Id);
            workerTypes.HasMany(w => w.Workers).WithOne().IsRequired().HasForeignKey(w => w.Type);
            workerTypes.Property(w => w.Name).IsRequired();
            workerTypes.HasData(ReferenceEntityExtensions.GetValues<WorkerType, WorkerTypeEntity>());

            var researchTypes = modelBuilder.Entity<ResearchTypeEntity>().ToTable("ResearchTypes", "Empires");
            researchTypes.HasKey(r => r.Id);
            researchTypes.HasMany(r => r.Research).WithOne().IsRequired().HasForeignKey(r => r.Type);
            researchTypes.HasMany(r => r.QueuedResearch).WithOne().IsRequired().HasForeignKey(r => r.Type);
            researchTypes.Property(r => r.Name).IsRequired();
            researchTypes.HasData(ReferenceEntityExtensions.GetValues<ResearchType, ResearchTypeEntity>());

            var races = modelBuilder.Entity<RaceEntity>().ToTable("Races", "Players");
            races.HasKey(r => r.Id);
            races.HasMany(r => r.Players).WithOne().IsRequired().HasForeignKey(p => p.Race);
            races.Property(r => r.Name).IsRequired();
            races.HasData(ReferenceEntityExtensions.GetValues<Race, RaceEntity>());

            modelBuilder.Entity<Profile>().ToTable("Profiles", "Players").HasKey(p => p.Id);

            modelBuilder.Entity<Workers>().ToTable("Workers", "Empires").HasKey(t => t.Id);

            modelBuilder.Entity<Troops>().ToTable("Troops", "Attacks").HasKey(t => t.Id);

            modelBuilder.Entity<Building>().ToTable("Buildings", "Empires").HasKey(b => b.Id);

            modelBuilder.Entity<SiegeWeapon>().ToTable("SiegeWeapons", "Siege").HasKey(w => w.Id);

            modelBuilder.Entity<QueuedResearch>().ToTable("QueuedResearch", "Empires").HasKey(t => t.Id);

            modelBuilder.Entity<Research>().ToTable("Research", "Empires").HasKey(t => t.Id);

            var messages = modelBuilder.Entity<Message>().ToTable("Messages", "Players");
            messages.HasKey(m => m.Id);
            messages.Property(m => m.Subject).IsRequired().HasMaxLength(100);

            var playerBlocks = modelBuilder.Entity<PlayerBlock>().ToTable("PlayerBlocks", "Players");
            playerBlocks.HasKey(b => b.Id);
            playerBlocks.HasOne(b => b.BlockedPlayer).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);

            var caravans = modelBuilder.Entity<Caravan>().ToTable("Caravans", "Markets");
            caravans.HasMany(c => c.Merchandise).WithOne().IsRequired();

            modelBuilder.Entity<Merchandise>().ToTable("Merchandise", "Markets");

            var merchandiseTypes = modelBuilder.Entity<MerchandiseTypeEntity>().ToTable("MerchandiseTypes", "Markets");
            merchandiseTypes.HasKey(m => m.Id);
            merchandiseTypes.HasMany(m => m.Merchandise).WithOne().IsRequired().HasForeignKey(m => m.Type);
            merchandiseTypes.HasMany(m => m.Transactions).WithOne().IsRequired().HasForeignKey(m => m.Type);
            merchandiseTypes.Property(m => m.Name).IsRequired();
            merchandiseTypes.HasData(ReferenceEntityExtensions.GetValues<MerchandiseType, MerchandiseTypeEntity>());

            modelBuilder.Entity<Transaction>().ToTable("Transactions", "Markets");
        }

        private static void OnAllianceModelCreating(ModelBuilder modelBuilder) {
            var alliances = modelBuilder.Entity<Alliance>().ToTable("Alliances", "Alliances");
            alliances.HasKey(a => a.Id);
            alliances.HasMany(a => a.Members).WithOne(p => p.Alliance);
            alliances.HasMany(a => a.Invites).WithOne(i => i.Alliance).IsRequired();
            alliances.HasMany(a => a.Roles).WithOne(r => r.Alliance).IsRequired();
            alliances.HasMany(a => a.NonAggressionPacts).WithMany(p => p.Alliances).UsingEntity<Dictionary<string, object>>(
                "AllianceNonAggressionPacts",
                n => n.HasOne<NonAggressionPact>().WithMany().HasForeignKey("NonAggressionPactId"),
                n => n.HasOne<Alliance>().WithMany().HasForeignKey("AllianceId")
            ).ToTable("AllianceNonAggressionPacts", "Alliances");
            alliances.HasMany(a => a.Wars).WithMany(w => w.Alliances).UsingEntity<Dictionary<string, object>>(
                "AllianceWars",
                n => n.HasOne<War>().WithMany().HasForeignKey("WarId"),
                n => n.HasOne<Alliance>().WithMany().HasForeignKey("AllianceId")
            ).ToTable("AllianceWars", "Alliances");
            alliances.HasMany(a => a.PeaceDeclarations).WithMany(w => w.PeaceDeclarations).UsingEntity<Dictionary<string, object>>(
                 "PeaceDeclarations",
                 n => n.HasOne<War>().WithMany().HasForeignKey("WarId"),
                 n => n.HasOne<Alliance>().WithMany().HasForeignKey("AllianceId")
            ).ToTable("PeaceDeclarations", "Alliances");
            alliances.HasMany(a => a.SentNonAggressionPactRequests).WithOne(r => r.Sender).IsRequired().OnDelete(DeleteBehavior.NoAction);
            alliances.HasMany(a => a.ReceivedNonAggressionPactRequests).WithOne(r => r.Recipient).IsRequired();
            alliances.HasMany(a => a.ChatMessages).WithOne().IsRequired();
            alliances.HasOne(a => a.Leader).WithMany().IsRequired().OnDelete(DeleteBehavior.Cascade);
            alliances.Property(a => a.Code).IsRequired().HasMaxLength(4);
            alliances.Property(a => a.Name).IsRequired();
            alliances.OwnsOne(p => p.BankedResources, ar => {
                ar.Property(r => r.Gold).HasColumnName("BankedGold");
                ar.Property(r => r.Food).HasColumnName("BankedFood");
                ar.Property(r => r.Wood).HasColumnName("BankedWood");
                ar.Property(r => r.Stone).HasColumnName("BankedStone");
                ar.Property(r => r.Ore).HasColumnName("BankedOre");
            });

            var invites = modelBuilder.Entity<Invite>().ToTable("Invites", "Alliances");
            invites.HasKey(i => i.Id);
            invites.Property(i => i.Subject).IsRequired().HasMaxLength(100);

            var chatMessages = modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages", "Alliances");
            chatMessages.HasKey(m => m.Id);
            chatMessages.HasOne(m => m.Player).WithMany().OnDelete(DeleteBehavior.NoAction);
            chatMessages.Property(m => m.Message).IsRequired();

            var rights = modelBuilder.Entity<RightEntity>().ToTable("Rights", "Alliances");
            rights.HasKey(r => r.Id);
            rights.Property(r => r.Name).IsRequired();
            rights.HasData(ReferenceEntityExtensions.GetValues<Right, RightEntity>());

            var roles = modelBuilder.Entity<Role>().ToTable("Roles", "Alliances");
            roles.HasKey(r => r.Id);
            roles.Property(r => r.Name).IsRequired();

            modelBuilder.Entity<NonAggressionPact>().ToTable("NonAggressionPacts", "Alliances").HasKey(m => m.Id);

            modelBuilder.Entity<NonAggressionPactRequest>().ToTable("NonAggressionPactRequests", "Alliances").HasKey(m => m.Id);

            modelBuilder.Entity<War>().ToTable("Wars", "Alliances").HasKey(m => m.Id);
        }

        private static void OnAttacksModelCreating(ModelBuilder modelBuilder) {
            var attackResults = modelBuilder.Entity<AttackResultEntity>().ToTable("AttackResults", "Attacks");
            attackResults.HasKey(r => r.Id);
            attackResults.HasMany(r => r.Attacks).WithOne().IsRequired().HasForeignKey(a => a.Result);
            attackResults.Property(r => r.Name).IsRequired();
            attackResults.HasData(ReferenceEntityExtensions.GetValues<AttackResult, AttackResultEntity>());

            var attackTypes = modelBuilder.Entity<AttackTypeEntity>().ToTable("AttackTypes", "Attacks");
            attackTypes.HasKey(r => r.Id);
            attackTypes.HasMany(r => r.Attacks).WithOne().IsRequired().HasForeignKey(a => a.Type);
            attackTypes.Property(r => r.Name).IsRequired();
            attackTypes.HasData(ReferenceEntityExtensions.GetValues<AttackType, AttackTypeEntity>());

            var attacks = modelBuilder.Entity<Attack>().ToTable("Attacks", "Attacks");
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
                .HasValue<Assault>(nameof(Assault))
                .HasValue<Raid>(nameof(Raid))
                .HasValue<GrandOverlordAttack>(nameof(GrandOverlordAttack))
                .HasValue<Revenge>(nameof(Revenge));

            var troopTypes = modelBuilder.Entity<TroopTypeEntity>().ToTable("TroopTypes", "Attacks");
            troopTypes.HasKey(t => t.Id);
            troopTypes.HasMany(t => t.AttackRounds).WithOne().IsRequired().HasForeignKey(r => r.TroopType);
            troopTypes.HasMany(t => t.Casualties).WithOne().IsRequired().HasForeignKey(c => c.TroopType).OnDelete(DeleteBehavior.NoAction);
            troopTypes.HasMany(t => t.Troops).WithOne().IsRequired().HasForeignKey(t => t.Type);
            troopTypes.Property(t => t.Name).IsRequired();
            troopTypes.HasData(ReferenceEntityExtensions.GetValues<TroopType, TroopTypeEntity>());

            var attackRounds = modelBuilder.Entity<AttackRound>().ToTable("AttackRounds", "Attacks");
            attackRounds.HasKey(r => r.Id);
            attackRounds.HasMany(r => r.Casualties).WithOne().IsRequired();

            modelBuilder.Entity<Casualties>().ToTable("Casualties", "Attacks").HasKey(c => c.Id);
        }

        private static void OnSecurityModelCreating(ModelBuilder modelBuilder) {
            var userEventTypes = modelBuilder.Entity<UserEventTypeEntity>().ToTable("UserEventTypes", "Security");
            userEventTypes.HasKey(t => t.Id);
            userEventTypes.HasMany(t => t.UserEvents).WithOne().IsRequired().HasForeignKey(e => e.Type);
            userEventTypes.Property(t => t.Name).IsRequired();
            userEventTypes.HasData(ReferenceEntityExtensions.GetValues<UserEventType, UserEventTypeEntity>());

            var userStatus = modelBuilder.Entity<UserStatusEntity>().ToTable("UserStatus", "Security");
            userStatus.HasKey(s => s.Id);
            userStatus.HasMany(t => t.Users).WithOne().IsRequired().HasForeignKey(u => u.Status);
            userStatus.Property(t => t.Name).IsRequired();
            userStatus.HasData(ReferenceEntityExtensions.GetValues<UserStatus, UserStatusEntity>());

            var users = modelBuilder.Entity<User>().ToTable("Users", "Security");
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

            var userEvents = modelBuilder.Entity<UserEvent>().ToTable("UserEvents", "Security");
            userEvents.HasKey(e => e.Id);
        }

        public override int SaveChanges() {
            DeleteOrphanedNonAggressionPacts();

            return base.SaveChanges();
        }

        private void DeleteOrphanedNonAggressionPacts() {
            var orphans = GetChangeTrackerEntities<NonAggressionPact>()
                .Except(GetChangeTrackerEntities<Alliance>().SelectMany(a => a.NonAggressionPacts));

            Set<NonAggressionPact>().RemoveRange(orphans);
        }

        private IEnumerable<TEntity> GetChangeTrackerEntities<TEntity>() {
            return ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Deleted)
                .Select(e => e.Entity)
                .OfType<TEntity>()
                .ToList();
        }
    }
}
