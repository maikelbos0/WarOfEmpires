namespace WarOfEmpires.Database.Migrations {
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WarOfEmpires.Database.ReferenceEntities;
    using WarOfEmpires.Domain.Players;
    using WarOfEmpires.Utilities.Services;
    using Attacks = WarOfEmpires.Domain.Attacks;
    using Empires = WarOfEmpires.Domain.Empires;
    using Events = WarOfEmpires.Domain.Events;
    using Markets = WarOfEmpires.Domain.Markets;
    using Security = WarOfEmpires.Domain.Security;
    using Siege = WarOfEmpires.Domain.Siege;

    public sealed class Configuration : DbMigrationsConfiguration<WarContext> {
        protected override void Seed(WarContext context) {
            SeedEntityType<Security.UserEventType, UserEventTypeEntity>(context);
            SeedEntityType<Empires.BuildingType, BuildingTypeEntity>(context);
            SeedEntityType<Security.UserStatus, UserStatusEntity>(context);
            SeedEntityType<Empires.WorkerType, WorkerTypeEntity>(context);
            SeedEntityType<Attacks.TroopType, TroopTypeEntity>(context);
            SeedEntityType<Attacks.AttackResult, AttackResultEntity>(context);
            SeedEntityType<Attacks.AttackType, AttackTypeEntity>(context);
            SeedEntityType<Siege.SiegeWeaponType, SiegeWeaponTypeEntity>(context);
            SeedEntityType<Markets.MerchandiseType, MerchandiseTypeEntity>(context);

            AddOrUpdateUser(context, "example@test.com", "I am example");
            AddOrUpdateUser(context, "anon@test.com", "Anon");
            AddOrUpdateUser(context, "you@test.com", "You");
            AddOrUpdateUser(context, "another@test.com", "Another");
            AddOrUpdateUser(context, "singsongkat@live.com", "Shira");

            for (var i = 0; i < 100; i++) {
                AddOrUpdateUser(context, $"user{i}@test.com", $"User {i}");
            }

            AddOrUpdateScheduledTasks(context);
        }

        private void SeedEntityType<TEnum, TReferenceEntity>(WarContext context)
            where TEnum : Enum
            where TReferenceEntity : BaseReferenceEntity<TEnum>, new() {

            foreach (var entity in ReferenceEntityExtensions.GetValues<TEnum, TReferenceEntity>()) {
                context.Set<TReferenceEntity>().AddOrUpdate(entity);
            }

            context.SaveChanges();
        }

        private void AddOrUpdateUser(WarContext context, string email, string displayName) {
            var user = context.Users.SingleOrDefault(u => EmailComparisonService.Equals(u.Email, email));

            if (user == null) {
                user = new Security.User(email, new Random().Next().ToString());
                user.Activate();

                context.Users.Add(user);
            }

            context.SaveChanges();

            var player = context.Players.SingleOrDefault(p => p.Id == user.Id);

            if (player == null) {
                player = new Player(user.Id, displayName);
                context.Players.Add(player);
            }
            else {
                player.DisplayName = displayName;
            }

            context.SaveChanges();
        }

        private void AddOrUpdateScheduledTasks(WarContext context) {
            var recruitTask = context.ScheduledTasks.SingleOrDefault(t => t.EventType == typeof(Empires.RecruitTaskTriggeredEvent).AssemblyQualifiedName);

            if (recruitTask == null) {
                recruitTask = Events.ScheduledTask.Create<Empires.RecruitTaskTriggeredEvent>(new TimeSpan(1, 0, 0));
                context.ScheduledTasks.Add(recruitTask);
            }

            var turnTask = context.ScheduledTasks.SingleOrDefault(t => t.EventType == typeof(Empires.TurnTaskTriggeredEvent).AssemblyQualifiedName);

            if (turnTask == null) {
                turnTask = Events.ScheduledTask.Create<Empires.TurnTaskTriggeredEvent>(new TimeSpan(0, 10, 0));
                context.ScheduledTasks.Add(turnTask);
            }

            var bankTurnTask = context.ScheduledTasks.SingleOrDefault(t => t.EventType == typeof(Empires.BankTurnTaskTriggeredEvent).AssemblyQualifiedName);

            if (bankTurnTask == null) {
                bankTurnTask = Events.ScheduledTask.Create<Empires.BankTurnTaskTriggeredEvent>(new TimeSpan(4, 0, 0));
                context.ScheduledTasks.Add(bankTurnTask);
            }

            context.SaveChanges();
        }
    }
}