using System;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Markets;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Markets {
    [ScopedServiceImplementation(typeof(ICommandHandler<SellResourcesCommand>))]
    public sealed class SellResourcesCommandHandler : ICommandHandler<SellResourcesCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IEnumFormatter _formatter;

        public SellResourcesCommandHandler(IPlayerRepository repository, IEnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        [Audit]
        public CommandResult<SellResourcesCommand> Execute(SellResourcesCommand command) {
            var result = new CommandResult<SellResourcesCommand>();
            var merchandiseTotals = new List<MerchandiseTotals>();
            var player = _repository.Get(command.Email);
            var availableMerchants = player.GetWorkerCount(WorkerType.Merchants) - player.Caravans.Count;
            var caravanCapacity = player.GetBuildingBonus(BuildingType.Market);

            for (var index = 0; index < command.Merchandise.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (MerchandiseType)Enum.Parse(typeof(MerchandiseType), command.Merchandise[i].Type);
                
                if (command.Merchandise[i].Quantity.HasValue && !command.Merchandise[i].Price.HasValue) {
                    result.AddError(c => c.Merchandise[i].Price, $"{_formatter.ToString(type)} price is required when selling {_formatter.ToString(type, false)}");
                }

                if (command.Merchandise[i].Quantity.HasValue && !player.CanAfford(MerchandiseTotals.ToResources(type, command.Merchandise[i].Quantity.Value))) {
                    result.AddError(c => c.Merchandise[i].Quantity, $"You don't have enough {_formatter.ToString(type, false)} available to sell that much");
                }

                if (result.Success && command.Merchandise[i].Quantity.HasValue && command.Merchandise[i].Price.HasValue) {
                    merchandiseTotals.Add(new MerchandiseTotals(type, command.Merchandise[i].Quantity.Value, command.Merchandise[i].Price.Value));
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