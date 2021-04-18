using System;
using System.Collections.Generic;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Empires {
    [ScopedServiceImplementation(typeof(ICommandHandler<DiscardSiegeCommand>))]
    [Audit]
    public sealed class DiscardSiegeCommandHandler : ICommandHandler<DiscardSiegeCommand> {
        private readonly IPlayerRepository _repository;
        private readonly EnumFormatter _formatter;

        public DiscardSiegeCommandHandler(IPlayerRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        public CommandResult<DiscardSiegeCommand> Execute(DiscardSiegeCommand command) {
            var result = new CommandResult<DiscardSiegeCommand>();
            var player = _repository.Get(command.Email);
            var siegeWeapons = new List<SiegeWeaponInfo>();

            for (var index = 0; index < command.SiegeWeapons.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (SiegeWeaponType)Enum.Parse(typeof(SiegeWeaponType), command.SiegeWeapons[i].Type);
                
                if (command.SiegeWeapons[i].Count.HasValue && command.SiegeWeapons[i].Count.Value > player.GetSiegeWeaponCount(type)) {
                    result.AddError(c => c.SiegeWeapons[i].Count, $"You don't have that many {_formatter.ToString(type, false)} to discard");
                }

                if (result.Success && command.SiegeWeapons[i].Count.HasValue) {
                    siegeWeapons.Add(new SiegeWeaponInfo(type, command.SiegeWeapons[i].Count.Value));
                }
            }

            if (result.Success) {
                foreach (var info in siegeWeapons) {
                    player.DiscardSiege(info.Type, info.Count);
                }

                _repository.SaveChanges();
            }

            return result;
        }
    }
}