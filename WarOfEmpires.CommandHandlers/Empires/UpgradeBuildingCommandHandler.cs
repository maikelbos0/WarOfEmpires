using System;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class UpgradeBuildingCommandHandler : ICommandHandler<UpgradeBuildingCommand> {
        private readonly PlayerRepository _repository;

        public UpgradeBuildingCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<UpgradeBuildingCommand> Execute(UpgradeBuildingCommand command) {
            var result = new CommandResult<UpgradeBuildingCommand>();
            var player = _repository.Get(command.Email);
            var buildingType = (BuildingType)Enum.Parse(typeof(BuildingType), command.BuildingType);
            var buildingDefinition = BuildingDefinitionFactory.Get(buildingType);
            var buildingLevel = player.Buildings.SingleOrDefault(b => b.Type == buildingType)?.Level ?? 0;

            if (!player.CanAfford(buildingDefinition.GetNextLevelCost(buildingLevel))) {
                result.AddError($"You don't have enough resources to upgrade your {buildingDefinition.GetName(buildingLevel)}");
            }

            if (result.Success) {
                player.UpgradeBuilding(buildingType);
                _repository.Update();
            }

            return result;
        }
    }
}