using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class DiscardSiegeCommandHandler : ICommandHandler<DiscardSiegeCommand> {
        private class SiegeWeaponInfo {
            public SiegeWeaponType Type { get; }
            public int Count { get; }

            public SiegeWeaponInfo(SiegeWeaponType type, int count) {
                Type = type;
                Count = count;
            }
        }

        public PlayerRepository _repository;

        public DiscardSiegeCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<DiscardSiegeCommand> Execute(DiscardSiegeCommand command) {
            var result = new CommandResult<DiscardSiegeCommand>();
            var player = _repository.Get(command.Email);
            var siege = new List<SiegeWeaponInfo>();
            int fireArrows = 0;
            int batteringRams = 0;
            int scalingLadders = 0;

            if (!string.IsNullOrEmpty(command.FireArrows) && !int.TryParse(command.FireArrows, out fireArrows) || fireArrows < 0) {
                result.AddError(c => c.FireArrows, "Fire arrows must be a valid number");
            }
            else if (player.SiegeWeapons.SingleOrDefault(w => w.Type == SiegeWeaponType.FireArrows).Count < fireArrows) {
                result.AddError(c => c.FireArrows, "You don't have that many fire arrows to discard");
            }
            else if (fireArrows > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.FireArrows, fireArrows));
            }

            if (!string.IsNullOrEmpty(command.BatteringRams) && !int.TryParse(command.BatteringRams, out batteringRams) || batteringRams < 0) {
                result.AddError(c => c.BatteringRams, "Battering rams must be a valid number");
            }
            else if (player.GetSiegeWeaponCount(SiegeWeaponType.BatteringRams) < batteringRams) {
                result.AddError(c => c.BatteringRams, "You don't have that many battering rams to discard");
            }
            else if (batteringRams > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.BatteringRams, batteringRams));
            }

            if (!string.IsNullOrEmpty(command.ScalingLadders) && !int.TryParse(command.ScalingLadders, out scalingLadders) || scalingLadders < 0) {
                result.AddError(c => c.ScalingLadders, "Scaling ladders must be a valid number");
            }
            else if (player.GetSiegeWeaponCount(SiegeWeaponType.ScalingLadders) <scalingLadders) {
                result.AddError(c => c.ScalingLadders, "You don't have that many scaling ladders to discard");
            }
            else if (scalingLadders > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.ScalingLadders, scalingLadders));
            }
            
            if (result.Success) {
                foreach (var info in siege) {
                    player.DiscardSiege(info.Type, info.Count);
                }

                _repository.Update();
            }

            return result;
        }
    }
}