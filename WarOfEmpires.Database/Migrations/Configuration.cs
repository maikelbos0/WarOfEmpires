namespace WarOfEmpires.Database.Migrations {
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WarOfEmpires.Database.ReferenceEntities;
    using WarOfEmpires.Domain.Players;
    using WarOfEmpires.Domain.Security;

    public sealed class Configuration : DbMigrationsConfiguration<WarContext> {
        protected override void Seed(WarContext context) {
            SeedEntityType<UserEventType, UserEventTypeEntity>(context);
            SeedEntityType<UserStatus, UserStatusEntity>(context);

            AddOrUpdateUser(context, "example@test.com", "I am example");
            AddOrUpdateUser(context, "anon@test.com", "Anon");
            AddOrUpdateUser(context, "you@test.com", "You");
            AddOrUpdateUser(context, "another@test.com", "Another");

            for (var i = 0; i < 100; i++) {
                AddOrUpdateUser(context, $"user{i}@test.com", $"User {i}");
            }
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
            var user = context.Users.SingleOrDefault(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            if (user == null) {
                user = new User(email, new Random().Next().ToString());
                context.Users.Add(user);
            }

            user.Activate();
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
    }
}