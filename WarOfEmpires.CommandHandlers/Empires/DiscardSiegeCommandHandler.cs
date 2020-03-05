using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class DiscardSiegeCommandHandler : ICommandHandler<DiscardSiegeCommand> {
        private readonly PlayerRepository _repository;

        public DiscardSiegeCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        private IEnumerable<SiegeWeaponInfo> ParseSiegeWeapon(DiscardSiegeCommand command,
                                                              CommandResult<DiscardSiegeCommand> result,
                                                              SiegeWeaponType type,
                                                              Expression<Func<DiscardSiegeCommand, object>> siegeWeaponFunc,
                                                              int maximumSiegeWeapons) {

            var commandSiegeWeapons = (string)siegeWeaponFunc.Compile().Invoke(command);
            int siegeWeapons = 0;

            if (!string.IsNullOrEmpty(commandSiegeWeapons) && !int.TryParse(commandSiegeWeapons, out siegeWeapons) || siegeWeapons < 0) {
                result.AddError(siegeWeaponFunc, $"{type.ToString()} must be a valid number");
            }
            else if (siegeWeapons > maximumSiegeWeapons) {
                result.AddError(siegeWeaponFunc, $"You don't have that many {type.ToString().ToLower()} to discard");
            }

            if (result.Success && siegeWeapons > 0) {
                yield return new SiegeWeaponInfo(type, siegeWeapons);
            }
        }

        public CommandResult<DiscardSiegeCommand> Execute(DiscardSiegeCommand command) {
            var result = new CommandResult<DiscardSiegeCommand>();
            var player = _repository.Get(command.Email);
            var siegeWeapons = new List<SiegeWeaponInfo>();

            siegeWeapons.AddRange(ParseSiegeWeapon(command, result, SiegeWeaponType.FireArrows, c => c.FireArrows, player.GetSiegeWeaponCount(SiegeWeaponType.FireArrows)));
            siegeWeapons.AddRange(ParseSiegeWeapon(command, result, SiegeWeaponType.BatteringRams, c => c.BatteringRams, player.GetSiegeWeaponCount(SiegeWeaponType.BatteringRams)));
            siegeWeapons.AddRange(ParseSiegeWeapon(command, result, SiegeWeaponType.ScalingLadders, c => c.ScalingLadders, player.GetSiegeWeaponCount(SiegeWeaponType.ScalingLadders)));
                        
            if (result.Success) {
                foreach (var info in siegeWeapons) {
                    player.DiscardSiege(info.Type, info.Count);
                }

                _repository.Update();
            }

            return result;
        }
    }
}