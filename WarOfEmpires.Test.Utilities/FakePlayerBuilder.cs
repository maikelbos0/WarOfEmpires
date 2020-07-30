using NSubstitute;
using System;
using System.Collections.Generic;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Attacks;
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
            _context.Users.Add(User);

            Player = Substitute.For<Player>();
            Player.User.Returns(User);
            Player.Id.Returns(id);
            Player.DisplayName.Returns(displayName ?? $"Test display name {id}");
            Player.Rank.Returns(rank);
            Player.Title.Returns(title);
            Player.Workers.Returns(new List<Workers>());
            Player.Troops.Returns(new List<Troops>());

            _context.Players.Add(Player);
        }

        public FakePlayerBuilder AddPeasants(int peasants) {
            Player.Peasants.Returns(peasants);

            return this;
        }

        public FakePlayerBuilder AddWorkers(WorkerType type, int workers) {
            Player.Workers.Add(new Workers(type, workers));

            return this;
        }

        public FakePlayerBuilder AddTroops(TroopType type, int soldiers, int mercenaries) {
            Player.Troops.Add(new Troops(type, soldiers, mercenaries));

            return this;
        }

        public FakePlayerBuilder AddPopulation() {
            return AddPeasants(5)
                .AddWorkers(WorkerType.Farmers, 1)
                .AddWorkers(WorkerType.WoodWorkers, 2)
                .AddWorkers(WorkerType.StoneMasons, 3)
                .AddWorkers(WorkerType.OreMiners, 4)
                .AddWorkers(WorkerType.SiegeEngineers, 6)
                .AddTroops(TroopType.Archers, 15, 5)
                .AddTroops(TroopType.Cavalry, 3, 1)
                .AddTroops(TroopType.Footmen, 3, 1);
        }

        public FakePlayerBuilder AddChatMessage(DateTime date, string message) {
            var chatMessage = Substitute.For<ChatMessage>();

            chatMessage.Player.Returns(Player);
            chatMessage.Date.Returns(date);
            chatMessage.Message.Returns(message);
            Player.Alliance.ChatMessages.Add(chatMessage);

            return this;
        }
    }
}