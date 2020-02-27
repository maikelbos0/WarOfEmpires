using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class DiscardSiegeCommandHandler : ICommandHandler<DiscardSiegeCommand> {
        public PlayerRepository _repository;

        public DiscardSiegeCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<DiscardSiegeCommand> Execute(DiscardSiegeCommand command) {
            var result = new CommandResult<DiscardSiegeCommand>();
            var player = _repository.Get(command.Email);
            var siege = new List<SiegeWeaponInfo>();
            int weapons = 0;

            // Fire arrows
            if (!string.IsNullOrEmpty(command.FireArrows) && !int.TryParse(command.FireArrows, out weapons) || weapons < 0) {
                result.AddError(c => c.FireArrows, "Fire arrows must be a valid number");
            }
            else if (player.GetSiegeWeaponCount(SiegeWeaponType.FireArrows) < weapons) {
                result.AddError(c => c.FireArrows, "You don't have that many fire arrows to discard");
            }
            else if (weapons > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.FireArrows, weapons));
            }

            // Battering rams
            if (!string.IsNullOrEmpty(command.BatteringRams) && !int.TryParse(command.BatteringRams, out weapons) || weapons < 0) {
                result.AddError(c => c.BatteringRams, "Battering rams must be a valid number");
            }
            else if (player.GetSiegeWeaponCount(SiegeWeaponType.BatteringRams) < weapons) {
                result.AddError(c => c.BatteringRams, "You don't have that many battering rams to discard");
            }
            else if (weapons > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.BatteringRams, weapons));
            }

            // Scaling ladders
            if (!string.IsNullOrEmpty(command.ScalingLadders) && !int.TryParse(command.ScalingLadders, out weapons) || weapons < 0) {
                result.AddError(c => c.ScalingLadders, "Scaling ladders must be a valid number");
            }
            else if (player.GetSiegeWeaponCount(SiegeWeaponType.ScalingLadders) < weapons) {
                result.AddError(c => c.ScalingLadders, "You don't have that many scaling ladders to discard");
            }
            else if (weapons > 0) {
                siege.Add(new SiegeWeaponInfo(SiegeWeaponType.ScalingLadders, weapons));
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