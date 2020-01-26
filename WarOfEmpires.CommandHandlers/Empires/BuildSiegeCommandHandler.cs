using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Common;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class BuildSiegeCommandHandler : ICommandHandler<BuildSiegeCommand> {
        private class SiegeWeaponInfo {
            public SiegeWeaponType Type { get; }
            public int Count { get; }

            public SiegeWeaponInfo(SiegeWeaponType type, int count) {
                Type = type;
                Count = count;
            }
        }

        public PlayerRepository _repository;

        public BuildSiegeCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<BuildSiegeCommand> Execute(BuildSiegeCommand command) {
            var result = new CommandResult<BuildSiegeCommand>();
            var player = _repository.Get(command.Email);
            var availableMaintenance = player.SiegeEngineers * player.GetBuildingBonus(BuildingType.SiegeFactory)
                - player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance);
            var siege = new List<SiegeWeaponInfo>();
            int fireArrows = 0;
            int batteringRams = 0;
            int scalingLadders = 0;

            if (!string.IsNullOrEmpty(command.FireArrows) && !int.TryParse(command.FireArrows, out fireArrows) || fireArrows < 0) {
                result.AddError(c => c.FireArrows, "Fire arrows must be a valid number");
            }
            else if (fireArrows>0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.FireArrows, fireArrows));
            }

            if (!string.IsNullOrEmpty(command.BatteringRams) && !int.TryParse(command.BatteringRams, out batteringRams) || batteringRams < 0) {
                result.AddError(c => c.BatteringRams, "Battering rams must be a valid number");
            }
            else if (batteringRams > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.BatteringRams, batteringRams));
            }

            if (!string.IsNullOrEmpty(command.ScalingLadders) && !int.TryParse(command.ScalingLadders, out scalingLadders) || scalingLadders < 0) {
                result.AddError(c => c.ScalingLadders, "Scaling ladders must be a valid number");
            }
            else if (scalingLadders > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.ScalingLadders, scalingLadders));
            }

            if (availableMaintenance < siege.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance)) {
                result.AddError("You don't have enough siege maintenance available to build that much siege");
            }

            if (!player.CanAfford(siege.Aggregate(new Resources(), (seed, s) => seed + s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Cost))) {
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