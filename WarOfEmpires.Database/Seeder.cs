using System;
using System.Linq;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Utilities.Services;
using Security = WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Database {
    public sealed class Seeder {
        public void Seed(WarContext context) {
            AddOrUpdateUser(context, "example@test.com", "I am example");
            AddOrUpdateUser(context, "anon@test.com", "Anon");
            AddOrUpdateUser(context, "you@test.com", "You");
            AddOrUpdateUser(context, "another@test.com", "Another");
            AddOrUpdateUser(context, "singsongkat@live.com", "Shira");

            for (var i = 0; i < 100; i++) {
                AddOrUpdateUser(context, $"user{i}@test.com", $"User {i}");
            }
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
    }
}
