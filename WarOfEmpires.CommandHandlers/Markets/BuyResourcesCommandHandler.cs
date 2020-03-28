using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Markets {
    [InterfaceInjectable]
    [Audit]
    public sealed class BuyResourcesCommandHandler : ICommandHandler<BuyResourcesCommand> {
        private readonly IPlayerRepository _repository;
        private readonly EnumFormatter _formatter;

        public BuyResourcesCommandHandler(IPlayerRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        private IEnumerable<MerchandiseTotals> ParseMerchandise(BuyResourcesCommand command,
                                                                CommandResult<BuyResourcesCommand> result,
                                                                MerchandiseType type,
                                                                Expression<Func<BuyResourcesCommand, object>> quantityFunc,
                                                                Expression<Func<BuyResourcesCommand, object>> priceFunc) {

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
                result.AddError(priceFunc, $"{_formatter.ToString(type)} price is required when buying {_formatter.ToString(type, false)}");
            }

            if (result.Success && price > 0 && quantity > 0) {
                yield return new MerchandiseTotals(type, quantity, price);
            }
        }

        public CommandResult<BuyResourcesCommand> Execute(BuyResourcesCommand command) {
            var result = new CommandResult<BuyResourcesCommand>();
            var merchandiseTotals = new List<MerchandiseTotals>();
            var player = _repository.Get(command.Email);

            merchandiseTotals.AddRange(ParseMerchandise(command, result, MerchandiseType.Food, c => c.Food, c => c.FoodPrice));
            merchandiseTotals.AddRange(ParseMerchandise(command, result, MerchandiseType.Wood, c => c.Wood, c => c.WoodPrice));
            merchandiseTotals.AddRange(ParseMerchandise(command, result, MerchandiseType.Stone, c => c.Stone, c => c.StonePrice));
            merchandiseTotals.AddRange(ParseMerchandise(command, result, MerchandiseType.Ore, c => c.Ore, c => c.OrePrice));

            if (!player.CanAfford(new Resources(gold: merchandiseTotals.Sum(m => m.Price * m.Quantity)))) {
                result.AddError("You don't have enough gold available to buy this many resources");
            }

            if (result.Success) {
                foreach (var totals in merchandiseTotals) {
                    var quantity = totals.Quantity;
                    var caravans = _repository.GetCaravans(totals.Type)
                        .Where(c => c.Merchandise.Any(m => m.Type == totals.Type && m.Price <= totals.Price))
                        .Where(c => c.Player != player)
                        .OrderBy(c => c.Merchandise.Single(m => m.Type == totals.Type).Price)
                        .ThenBy(c => c.Id);

                    foreach (var caravan in caravans) {
                        quantity = caravan.Buy(player, totals.Type, quantity);

                        if (!caravan.Merchandise.Any(m => m.Quantity > 0)) {
                            _repository.RemoveCaravan(caravan);
                        }

                        if (quantity == 0) {
                            break;
                        }
                    }

                    if (quantity > 0) {
                        result.AddWarning($"There was not enough {_formatter.ToString(totals.Type, false)} available at that price; all available {_formatter.ToString(totals.Type, false)} has been purchased");
                    }
                }

                _repository.Update();
            }

            return result;
        }
    }
}