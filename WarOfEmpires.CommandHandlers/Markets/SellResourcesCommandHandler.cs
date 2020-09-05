using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class SellResourcesCommandHandler : ICommandHandler<SellResourcesCommand> {
        private readonly IPlayerRepository _repository;
        private readonly EnumFormatter _formatter;

        public SellResourcesCommandHandler(IPlayerRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        public CommandResult<SellResourcesCommand> Execute(SellResourcesCommand command) {
            var result = new CommandResult<SellResourcesCommand>();
            var merchandiseTotals = new List<MerchandiseTotals>();
            var player = _repository.Get(command.Email);
            var availableMerchants = player.GetWorkerCount(WorkerType.Merchants) - player.Caravans.Count();
            var caravanCapacity = player.GetBuildingBonus(BuildingType.Market);

            for (var index = 0; index < command.Merchandise.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (MerchandiseType)Enum.Parse(typeof(MerchandiseType), command.Merchandise[i].Type);
                int quantity = 0;
                int price = 0;

                if (!string.IsNullOrEmpty(command.Merchandise[i].Quantity) && !int.TryParse(command.Merchandise[i].Quantity, out quantity) || quantity < 0) {
                    result.AddError(c => c.Merchandise[i].Quantity, "Invalid number");
                }

                if (!string.IsNullOrEmpty(command.Merchandise[i].Price) && !int.TryParse(command.Merchandise[i].Price, out price) || price < 0) {
                    result.AddError(c => c.Merchandise[i].Price, "Invalid number");
                }
                else if (quantity > 0 && price <= 0) {
                    result.AddError(c => c.Merchandise[i].Price, $"{_formatter.ToString(type)} price is required when selling {_formatter.ToString(type, false)}");
                }

                if (quantity > 0 && !player.CanAfford(MerchandiseTotals.ToResources(type, quantity))) {
                    result.AddError(c => c.Merchandise[i].Quantity, $"You don't have enough {_formatter.ToString(type, false)} available to sell that much");
                }

                if (result.Success && price > 0 && quantity > 0) {
                    merchandiseTotals.Add(new MerchandiseTotals(type, quantity, price));
                }
            }

            if (merchandiseTotals.Sum(m => m.Quantity) > availableMerchants * caravanCapacity) {
                result.AddError("You don't have enough merchants available to send this many to the market");
            }

            if (result.Success && merchandiseTotals.Any()) {
                player.SellResources(merchandiseTotals);
                _repository.SaveChanges();
            }

            return result;
        }
    }
}