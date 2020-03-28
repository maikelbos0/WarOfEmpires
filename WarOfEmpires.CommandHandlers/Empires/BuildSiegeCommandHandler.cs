using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class BuildSiegeCommandHandler : ICommandHandler<BuildSiegeCommand> {
        private readonly IPlayerRepository _repository;
        private readonly EnumFormatter _formatter;

        public BuildSiegeCommandHandler(IPlayerRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        private IEnumerable<SiegeWeaponInfo> ParseSiegeWeapon(BuildSiegeCommand command,
                                                              CommandResult<BuildSiegeCommand> result,
                                                              SiegeWeaponType type,
                                                              Expression<Func<BuildSiegeCommand, object>> siegeWeaponFunc) {

            var commandSiegeWeapons = (string)siegeWeaponFunc.Compile().Invoke(command);
            int siegeWeapons = 0;

            if (!string.IsNullOrEmpty(commandSiegeWeapons) && !int.TryParse(commandSiegeWeapons, out siegeWeapons) || siegeWeapons < 0) {
                result.AddError(siegeWeaponFunc, $"{_formatter.ToString(type)} must be a valid number");
            }

            if (result.Success && siegeWeapons > 0) {
                yield return new SiegeWeaponInfo(type, siegeWeapons);
            }
        }

        public CommandResult<BuildSiegeCommand> Execute(BuildSiegeCommand command) {
            var result = new CommandResult<BuildSiegeCommand>();
            var player = _repository.Get(command.Email);
            var availableMaintenance = player.GetWorkerCount(WorkerType.SiegeEngineers) * player.GetBuildingBonus(BuildingType.SiegeFactory)
                - player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance);
            var siegeWeapons = new List<SiegeWeaponInfo>();

            siegeWeapons.AddRange(ParseSiegeWeapon(command, result, SiegeWeaponType.FireArrows, c => c.FireArrows));
            siegeWeapons.AddRange(ParseSiegeWeapon(command, result, SiegeWeaponType.BatteringRams, c => c.BatteringRams));
            siegeWeapons.AddRange(ParseSiegeWeapon(command, result, SiegeWeaponType.ScalingLadders, c => c.ScalingLadders));

            if (availableMaintenance < siegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance)) {
                result.AddError("You don't have enough siege maintenance available to build that much siege");
            }

            if (!player.CanAfford(siegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Cost))) {
                result.AddError("You don't have enough resources to build that much siege");
            }

            if (result.Success) {
                foreach (var info in siegeWeapons) {
                    player.BuildSiege(info.Type, info.Count);
                }

                _repository.Update();
            }

            return result;
        }
    }
}