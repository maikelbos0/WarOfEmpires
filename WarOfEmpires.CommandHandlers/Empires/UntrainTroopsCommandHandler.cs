using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class UntrainTroopsCommandHandler : ICommandHandler<UntrainTroopsCommand> {
        public PlayerRepository _repository;

        public UntrainTroopsCommandHandler(PlayerRepository repository) {
            _repository = repository;
        }

        private IEnumerable<TroopInfo> ParseTroops(UntrainTroopsCommand command,
                                                   CommandResult<UntrainTroopsCommand> result,
                                                   TroopType type,
                                                   Expression<Func<UntrainTroopsCommand, object>> soldierFunc,
                                                   Expression<Func<UntrainTroopsCommand, object>> mercenaryFunc,
                                                   Troops maximumTroops) {
            
            var commandSoldiers = (string)soldierFunc.Compile().Invoke(command);
            var commandMercenaries = (string)mercenaryFunc.Compile().Invoke(command);
            int soldiers = 0;
            int mercenaries = 0;

            if (!string.IsNullOrEmpty(commandSoldiers) && !int.TryParse(commandSoldiers, out soldiers) || soldiers < 0) {
                result.AddError(soldierFunc, $"{type.ToString()} must be a valid number");
            }
            else if (soldiers > maximumTroops.Soldiers) {
                result.AddError(soldierFunc, $"You don't have that many {type.ToString().ToLower()} to untrain");
            }

            if (!string.IsNullOrEmpty(commandMercenaries) && !int.TryParse(commandMercenaries, out mercenaries) || mercenaries < 0) {
                result.AddError(mercenaryFunc, $"Mercenary {type.ToString().ToLower()} must be a valid number");
            }
            else if (mercenaries > maximumTroops.Mercenaries) {
                result.AddError(mercenaryFunc, $"You don't have that many mercenary {type.ToString().ToLower()} to untrain");
            }

            if (result.Success && (soldiers > 0 || mercenaries > 0)) {
                yield return new TroopInfo(type, soldiers, mercenaries);
            }
        }

        public CommandResult<UntrainTroopsCommand> Execute(UntrainTroopsCommand command) {
            var result = new CommandResult<UntrainTroopsCommand>();
            var player = _repository.Get(command.Email);
            var troops = new List<TroopInfo>();

            troops.AddRange(ParseTroops(command, result, TroopType.Archers, c => c.Archers, c => c.MercenaryArchers, player.GetTroops(TroopType.Archers)));
            troops.AddRange(ParseTroops(command, result, TroopType.Cavalry, c => c.Cavalry, c => c.MercenaryCavalry, player.GetTroops(TroopType.Cavalry)));
            troops.AddRange(ParseTroops(command, result, TroopType.Footmen, c => c.Footmen, c => c.MercenaryFootmen, player.GetTroops(TroopType.Footmen)));

            if (result.Success) {
                foreach (var t in troops) {
                    player.UntrainTroops(t.Type, t.Soldiers, t.Mercenaries);
                }

                _repository.Update();
            }

            return result;
        }
    }
}
