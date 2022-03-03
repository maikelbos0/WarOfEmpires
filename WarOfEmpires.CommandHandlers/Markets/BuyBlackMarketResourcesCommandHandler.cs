using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Markets {
    public sealed class BuyBlackMarketResourcesCommandHandler : ICommandHandler<BuyBlackMarketResourcesCommand> {
        private readonly IPlayerRepository _repository;

        public BuyBlackMarketResourcesCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<BuyBlackMarketResourcesCommand> Execute(BuyBlackMarketResourcesCommand command) {
            var result = new CommandResult<BuyBlackMarketResourcesCommand>();
            var merchandiseTotals = new List<BlackMarketMerchandiseTotals>();
            var player = _repository.Get(command.Email);

            for (var index = 0; index < command.Merchandise.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (MerchandiseType)Enum.Parse(typeof(MerchandiseType), command.Merchandise[i].Type);

                if (result.Success && command.Merchandise[i].Quantity.HasValue) {
                    merchandiseTotals.Add(new BlackMarketMerchandiseTotals(type, command.Merchandise[i].Quantity.Value));
                }
            }

            if (!player.CanAfford(new Resources(gold: merchandiseTotals.Sum(m => Player.BlackMarketBuyPrice * m.Quantity)))) {
                result.AddError("You don't have enough gold available to buy this many resources");
            }

            if (result.Success) {
                foreach (var totals in merchandiseTotals) {
                    player.BuyResourcesFromBlackMarket(totals);
                }

                _repository.SaveChanges();
            }

            return result;
        }
    }
}
