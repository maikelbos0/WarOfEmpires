using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class BuildSiegeCommandHandler : ICommandHandler<BuildSiegeCommand> {
        public PlayerRepository _repository;

        public BuildSiegeCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<BuildSiegeCommand> Execute(BuildSiegeCommand command) {
            var result = new CommandResult<BuildSiegeCommand>();
            var player = _repository.Get(command.Email);
            var availableMaintenance = player.GetWorkerCount(WorkerType.SiegeEngineer) * player.GetBuildingBonus(BuildingType.SiegeFactory)
                - player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance);
            var siege = new List<SiegeWeaponInfo>();
            int value = 0;

            // Fire arrows
            if (!string.IsNullOrEmpty(command.FireArrows) && !int.TryParse(command.FireArrows, out value) || value < 0) {
                result.AddError(c => c.FireArrows, "Fire arrows must be a valid number");
            }
            else if (value > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.FireArrows, value));
            }

            // Battering rams
            value = 0;
            if (!string.IsNullOrEmpty(command.BatteringRams) && !int.TryParse(command.BatteringRams, out value) || value < 0) {
                result.AddError(c => c.BatteringRams, "Battering rams must be a valid number");
            }
            else if (value > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.BatteringRams, value));
            }

            // Scaling ladders
            value = 0;
            if (!string.IsNullOrEmpty(command.ScalingLadders) && !int.TryParse(command.ScalingLadders, out value) || value < 0) {
                result.AddError(c => c.ScalingLadders, "Scaling ladders must be a valid number");
            }
            else if (value > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.ScalingLadders, value));
            }

            if (availableMaintenance < siege.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance)) {
                result.AddError("You don't have enough siege maintenance available to build that much siege");
            }

            if (!player.CanAfford(siege.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Cost))) {
                result.AddError("You don't have enough resources to build that much siege");
            }

            if (result.Success) {
                foreach (var info in siege) {
                    player.BuildSiege(info.Type, info.Count);
                }

                _repository.Update();
            }

            return result;
        }
    }
}