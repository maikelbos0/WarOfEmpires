using System;
using System.Collections.Generic;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
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
                int count = 0;

                if (!string.IsNullOrEmpty(command.SiegeWeapons[i].Count) && !int.TryParse(command.SiegeWeapons[i].Count, out count) || count < 0) {
                    result.AddError(c => c.SiegeWeapons[i].Count, "Invalid number");
                }
                else if (count > player.GetSiegeWeaponCount(type)) {
                    result.AddError(c => c.SiegeWeapons[i].Count, $"You don't have that many {_formatter.ToString(type, false)} to discard");
                }

                if (result.Success && count > 0) {
                    siegeWeapons.Add(new SiegeWeaponInfo(type, count));
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