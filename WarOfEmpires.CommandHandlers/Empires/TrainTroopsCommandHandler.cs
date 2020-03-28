using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class TrainTroopsCommandHandler : ICommandHandler<TrainTroopsCommand> {
        private readonly IPlayerRepository _repository;
        private readonly EnumFormatter _formatter;

        public TrainTroopsCommandHandler(IPlayerRepository repository, EnumFormatter formatter) {
            _repository = repository;
            _formatter = formatter;
        }

        private IEnumerable<TroopInfo> ParseTroops(TrainTroopsCommand command,
                                                   CommandResult<TrainTroopsCommand> result,
                                                   TroopType type,
                                                   Expression<Func<TrainTroopsCommand, object>> soldierFunc,
                                                   Expression<Func<TrainTroopsCommand, object>> mercenaryFunc) {

            var commandSoldiers = (string)soldierFunc.Compile().Invoke(command);
            var commandMercenaries = (string)mercenaryFunc.Compile().Invoke(command);
            int soldiers = 0;
            int mercenaries = 0;

            if (!string.IsNullOrEmpty(commandSoldiers) && !int.TryParse(commandSoldiers, out soldiers) || soldiers < 0) {
                result.AddError(soldierFunc, $"{_formatter.ToString(type)} must be a valid number");
            }

            if (!string.IsNullOrEmpty(commandMercenaries) && !int.TryParse(commandMercenaries, out mercenaries) || mercenaries < 0) {
                result.AddError(mercenaryFunc, $"Mercenary {_formatter.ToString(type, false)} must be a valid number");
            }

            if (result.Success && (soldiers > 0 || mercenaries > 0)) {
                yield return new TroopInfo(type, soldiers, mercenaries);
            }
        }

        public CommandResult<TrainTroopsCommand> Execute(TrainTroopsCommand command) {
            var result = new CommandResult<TrainTroopsCommand>();
            var player = _repository.Get(command.Email);
            var troops = new List<TroopInfo>();

            troops.AddRange(ParseTroops(command, result, TroopType.Archers, c => c.Archers, c => c.MercenaryArchers));
            troops.AddRange(ParseTroops(command, result, TroopType.Cavalry, c => c.Cavalry, c => c.MercenaryCavalry));
            troops.AddRange(ParseTroops(command, result, TroopType.Footmen, c => c.Footmen, c => c.MercenaryFootmen));

            if (troops.Sum(t => t.Soldiers) > player.Peasants) {
                result.AddError("You don't have that many peasants available to train");
            }

            if (troops.Sum(t => t.Soldiers + t.Mercenaries) > player.GetAvailableBarracksCapacity()) {
                result.AddError("You don't have enough barracks available to train that many troops");
            }

            if (!player.CanAfford(troops.Sum(t => TroopDefinitionFactory.Get(t.Type).Cost * t.Soldiers)
                + troops.Sum(t => t.Mercenaries) * Player.MercenaryTrainingCost)) {
                result.AddError("You don't have enough resources to train these troops");
            }

            if (result.Success) {
                foreach (var t in troops) {
                    player.TrainTroops(t.Type, t.Soldiers, t.Mercenaries);
                }

                _repository.Update();
            }

            return result;
        }
    }
}
