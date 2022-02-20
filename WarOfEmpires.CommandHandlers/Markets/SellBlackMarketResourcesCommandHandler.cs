using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Markets {
    public sealed class SellBlackMarketResourcesCommandHandler : ICommandHandler<SellBlackMarketResourcesCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IEnumFormatter _formatter;

        public SellBlackMarketResourcesCommandHandler(IPlayerRepository repository, IEnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        [Audit]
        public CommandResult<SellBlackMarketResourcesCommand> Execute(SellBlackMarketResourcesCommand command) {
            var result = new CommandResult<SellBlackMarketResourcesCommand>();
            var merchandiseTotals = new List<BlackMarketMerchandiseTotals>();
            var player = _repository.Get(command.Email);

            for (var index = 0; index < command.Merchandise.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (MerchandiseType)Enum.Parse(typeof(MerchandiseType), command.Merchandise[i].Type);

                if (command.Merchandise[i].Quantity.HasValue && !player.CanAfford(MerchandiseTotals.ToResources(type, command.Merchandise[i].Quantity.Value))) {
                    result.AddError(c => c.Merchandise[i].Quantity, $"You don't have enough {_formatter.ToString(type, false)} available to sell that much");
                }

                if (result.Success && command.Merchandise[i].Quantity.HasValue) {
                    merchandiseTotals.Add(new BlackMarketMerchandiseTotals(type, command.Merchandise[i].Quantity.Value));
                }
            }

            if (result.Success) {
                foreach (var totals in merchandiseTotals) {
                    player.SellResourcesToBlackMarket(totals);
                }

                _repository.SaveChanges();
            }

            return result;
        }
    }
}
