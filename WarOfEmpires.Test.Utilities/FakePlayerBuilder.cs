using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Domain.Siege;

namespace WarOfEmpires.Test.Utilities {
    public class FakePlayerBuilder : FakeUserBuilder {
        public Player Player { get; }

        internal FakePlayerBuilder(FakeWarContext context, int id, string email, string password, string displayName, int rank, TitleType title, DateTime? lastOnline, UserStatus status, int attackTurns, int bankTurns, bool canAffordAnything, int stamina, TimeSpan? grandOverlordTime) 
            : base(context, id, email, password, lastOnline, status) {

            Player = Substitute.For<Player>();
            Player.Alliance.Returns((Alliance)null);
            Player.User.Returns(User);
            Player.Id.Returns(id);
            Player.DisplayName.Returns(displayName ?? $"Test display name {id}");
            Player.Rank.Returns(rank);
            Player.Title.Returns(title);
            Player.AttackTurns.Returns(attackTurns);
            Player.BankTurns.Returns(bankTurns);
            Player.Workers.Returns(new List<Workers>());
            Player.Troops.Returns(new List<Troops>());
            Player.Invites.Returns(new List<Invite>());
            Player.ReceivedAttacks.Returns(new List<Attack>());
            Player.ExecutedAttacks.Returns(new List<Attack>());
            Player.Buildings.Returns(new List<Building>());
            Player.SiegeWeapons.Returns(new List<SiegeWeapon>());
            Player.BuyTransactions.Returns(new List<Transaction>());
            Player.SellTransactions.Returns(new List<Transaction>());
            Player.Caravans.Returns(new List<Caravan>());
            Player.SentMessages.Returns(new List<Message>());
            Player.ReceivedMessages.Returns(new List<Message>());
            Player.PlayerBlocks.Returns(new List<PlayerBlock>());
            Player.CanAfford(Arg.Any<Resources>()).Returns(canAffordAnything);
            Player.Stamina.Returns(stamina);
            Player.GrandOverlordTime.Returns(grandOverlordTime ?? TimeSpan.Zero);

            Context.Players.Add(Player);
        }

        public FakeAttackBuilder BuildAttackOn(int id, Player defender, AttackType type, AttackResult result, int turns = 10, bool isRead = false, DateTime? date = null, Resources resources = null) {
            return new FakeAttackBuilder(Context, id, Player, defender, type, result, turns, isRead, date, resources);
        }

        public FakePlayerBuilder WithAttackOn(int id, out Attack attack, Player defender, AttackType type, AttackResult result, int turns = 10, bool isRead = false, DateTime? date = null, Resources resources = null) {
            attack = BuildAttackOn(id, defender, type, result, turns, isRead, date, resources).Attack;

            return this;
        }

        public FakePlayerBuilder WithAttackOn(int id, Player defender, AttackType type, AttackResult result, int turns = 10, bool isRead = false, DateTime? date = null, Resources resources = null) {
            BuildAttackOn(id, defender, type, result, turns, isRead, date, resources);

            return this;
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
            var troops = new Troops(type, soldiers, mercenaries);

            Player.Troops.Add(troops);
            Player.GetTroops(type).Returns(troops);

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
            Player.GetBuildingBonus(type).Returns(BuildingDefinitionFactory.Get(type).GetBonus(level));

            return this;
        }

        public FakePlayerBuilder WithSiege(SiegeWeaponType type, int count) {
            Player.SiegeWeapons.Add(new SiegeWeapon(type, count));

            return this;
        }

        public FakePlayerBuilder WithBuyTransaction(MerchandiseType type, int quantity, int price) {
            Player.BuyTransactions.Add(new Transaction(type, quantity, price));

            return this;
        }

        public FakePlayerBuilder WithSellTransaction(MerchandiseType type, int quantity, int price) {
            Player.SellTransactions.Add(new Transaction(type, quantity, price));

            return this;
        }

        public FakePlayerBuilder WithCaravan(int id, out Caravan caravan, params Merchandise[] merchandise) {
            caravan = Substitute.For<Caravan>();

            caravan.Id.Returns(id);
            caravan.Player.Returns(Player);
            caravan.Date.Returns(DateTime.UtcNow);
            caravan.Merchandise.Returns(merchandise);
            caravan.Buy(Arg.Any<Player>(), Arg.Any<MerchandiseType>(), Arg.Any<int>()).Returns(c => merchandise.Single(m => m.Type == c.ArgAt<MerchandiseType>(1)).Buy(Player, c.ArgAt<Player>(0), c.ArgAt<int>(2)));
            Player.Caravans.Add(caravan);

            return this;
        }

        public FakePlayerBuilder WithCaravan(int id, params Merchandise[] merchandise) {
            return WithCaravan(id, out _, merchandise);
        }

        public FakePlayerBuilder WithMessageTo(int id, out Message message, Player recipient, DateTime date, bool isRead = false, string subject = "Message subject", string body = "Message body") {
            message = Substitute.For<Message>();

            message.Id.Returns(id);
            message.Sender.Returns(Player);
            message.Recipient.Returns(recipient);
            message.Date.Returns(date);
            message.IsRead.Returns(isRead);
            message.Subject.Returns(subject);
            message.Body.Returns(body);
            Player.SentMessages.Add(message);
            recipient.ReceivedMessages.Add(message);

            return this;
        }

        public FakePlayerBuilder WithMessageTo(int id, Player recipient, DateTime date, bool isRead = false, string subject = "Message subject", string body = "Message body") {
            return WithMessageTo(id, out _, recipient, date, isRead, subject, body);
        }

        public FakePlayerBuilder WithPlayerBlock(int id, out PlayerBlock playerBlock, Player blockedPlayer) {
            playerBlock = Substitute.For<PlayerBlock>();

            playerBlock.Id.Returns(id);
            playerBlock.BlockedPlayer.Returns(blockedPlayer);
            Player.PlayerBlocks.Add(playerBlock);

            return this;
        }

        public FakePlayerBuilder WithPlayerBlock(int id, Player blockedPlayer) {
            return WithPlayerBlock(id, out _, blockedPlayer);
        }
    }
}