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

        private IEnumerable<MerchandiseTotals> ParseMerchandise(SellResourcesCommand command,
                                                                CommandResult<SellResourcesCommand> result,
                                                                MerchandiseType type,
                                                                Player player,
                                                                Expression<Func<SellResourcesCommand, object>> quantityFunc,
                                                                Expression<Func<SellResourcesCommand, object>> priceFunc) {

            var commandResources = (string)quantityFunc.Compile().Invoke(command);
            var commandPrice = (string)priceFunc.Compile().Invoke(command);
            int quantity = 0;
            int price = 0;

            if (!string.IsNullOrEmpty(commandResources) && !int.TryParse(commandResources, out quantity) || quantity < 0) {
                result.AddError(quantityFunc, $"{_formatter.ToString(type)} must be a valid number");
            }

            if (!string.IsNullOrEmpty(commandPrice) && !int.TryParse(commandPrice, out price) || price < 0) {
                result.AddError(priceFunc, $"{_formatter.ToString(type)} price must be a valid number");
            }
            else if (quantity > 0 && price <= 0) {
                result.AddError(priceFunc, $"{_formatter.ToString(type)} price is required when selling {_formatter.ToString(type, false)}");
            }

            if (quantity > 0 && !player.CanAfford(MerchandiseTotals.ToResources(type, quantity))) {
                result.AddError(quantityFunc, $"You don't have enough {_formatter.ToString(type, false)} available to sell that much");
            }

            if (result.Success && price > 0 && quantity > 0) {
                yield return new MerchandiseTotals(type, quantity, price);
            }
        }

        public CommandResult<SellResourcesCommand> Execute(SellResourcesCommand command) {
            var result = new CommandResult<SellResourcesCommand>();
            var merchandiseTotals = new List<MerchandiseTotals>();
            var player = _repository.Get(command.Email);
            var availableMerchants = player.GetWorkerCount(WorkerType.Merchants) - player.Caravans.Count();
            var caravanCapacity = player.GetBuildingBonus(BuildingType.Market);

            merchandiseTotals.AddRange(ParseMerchandise(command, result, MerchandiseType.Food, player, c => c.Food, c => c.FoodPrice));
            merchandiseTotals.AddRange(ParseMerchandise(command, result, MerchandiseType.Wood, player, c => c.Wood, c => c.WoodPrice));
            merchandiseTotals.AddRange(ParseMerchandise(command, result, MerchandiseType.Stone, player, c => c.Stone, c => c.StonePrice));
            merchandiseTotals.AddRange(ParseMerchandise(command, result, MerchandiseType.Ore, player, c => c.Ore, c => c.OrePrice));

            if (merchandiseTotals.Sum(m => m.Quantity) > availableMerchants * caravanCapacity) {
                result.AddError("You don't have enough merchants available to send this many to the market");
            }

            if (result.Success && merchandiseTotals.Any()) {
                player.SellResources(merchandiseTotals);
                _repository.Update();
            }

            return result;
        }
    }
}