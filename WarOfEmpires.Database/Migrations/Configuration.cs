namespace WarOfEmpires.Database.Migrations {
    using WarOfEmpires.Database.ReferenceEntities;
    using WarOfEmpires.Domain.Security;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<WarContext> {
        protected override void Seed(WarContext context) {
            SeedEntityType<UserEventType, UserEventTypeEntity>(context);
            SeedEntityType<UserStatus, UserStatusEntity>(context);

            AddOrUpdateUser(context, "example@test.com", "I am example", null, true);
            AddOrUpdateUser(context, "anon@test.com", null, null, false);
            AddOrUpdateUser(context, "you@test.com", "You", "Who are you?", false);
            AddOrUpdateUser(context, "another@test.com", "Another", "One bites the dust", true);

            for (var i = 0; i < 100; i++) {
                AddOrUpdateUser(context, $"user{i}@test.com", $"User {i}", (i % 3 == 0 ? null : $"Hello I am user number {i}"), i % 2 == 0);
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

        private void AddOrUpdateUser(WarContext context, string email, string displayName, string description, bool showEmail) {
            var user = context.Users.SingleOrDefault(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            if (user == null) {
                user = new User(email, new Random().Next().ToString());
                context.Users.Add(user);
            }

            user.Activate();
            user.DisplayName = displayName;
            user.Description = description;
            user.ShowEmail = showEmail;

            context.SaveChanges();
        }
    }
}