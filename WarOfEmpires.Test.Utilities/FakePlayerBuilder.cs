using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.Test.Utilities {
    public class FakePlayerBuilder : FakeBuilder {
        public User User { get; }
        public Player Player { get; }

        internal FakePlayerBuilder(IWarContext context, int id, string email, string displayName, int rank, TitleType title, DateTime? lastOnline, UserStatus status) : base(context) {
            User = Substitute.For<User>();
            User.Id.Returns(id);
            User.Email.Returns(email ?? $"test{id}@test.com");
            User.LastOnline.Returns(lastOnline);
            User.Status.Returns(status);
            Context.Users.Add(User);

            Player = Substitute.For<Player>();
            Player.User.Returns(User);
            Player.Id.Returns(id);
            Player.DisplayName.Returns(displayName ?? $"Test display name {id}");
            Player.Rank.Returns(rank);
            Player.Title.Returns(title);
            Player.Workers.Returns(new List<Workers>());
            Player.Troops.Returns(new List<Troops>());
            Player.Invites.Returns(new List<Invite>());
            Player.ReceivedAttacks.Returns(new List<Attack>());
            Player.ExecutedAttacks.Returns(new List<Attack>());
            Player.Buildings.Returns(new List<Building>());

            Context.Players.Add(Player);
        }

        public FakeAttackBuilder BuildAttack(int id, Player defender, AttackType type, AttackResult result, int turns = 10, bool isRead = false, DateTime? date = null, Resources resources = null) {
            return new FakeAttackBuilder(Context, id, Player, defender, type, result, turns, isRead, date, resources);
        }

        public FakePlayerBuilder WithPeasants(int peasants) {
            Player.Peasants.Returns(peasants);

            return this;
        }

        public FakePlayerBuilder WithWorkers(WorkerType type, int workers) {
            Player.Workers.Add(new Workers(type, workers));

            return this;
        }

        public FakePlayerBuilder WithTroops(TroopType type, int soldiers, int mercenaries) {
            Player.Troops.Add(new Troops(type, soldiers, mercenaries));

            return this;
        }

        public FakePlayerBuilder WithPopulation() {
            return WithPeasants(5)
                .WithWorkers(WorkerType.Farmers, 1)
                .WithWorkers(WorkerType.WoodWorkers, 2)
                .WithWorkers(WorkerType.StoneMasons, 3)
                .WithWorkers(WorkerType.OreMiners, 4)
                .WithWorkers(WorkerType.SiegeEngineers, 6)
                .WithTroops(TroopType.Archers, 15, 5)
                .WithTroops(TroopType.Cavalry, 3, 1)
                .WithTroops(TroopType.Footmen, 3, 1);
        }

        public FakePlayerBuilder WithBuilding(BuildingType type, int level) {
            Player.Buildings.Add(new Building(type, level));

            return this;
        }
    }
}