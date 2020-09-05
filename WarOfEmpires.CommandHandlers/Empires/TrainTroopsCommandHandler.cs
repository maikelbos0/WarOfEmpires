using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.CommandHandlers.Empires {
    [InterfaceInjectable]
    [Audit]
    public sealed class TrainTroopsCommandHandler : ICommandHandler<TrainTroopsCommand> {
        private readonly IPlayerRepository _repository;

        public TrainTroopsCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        public CommandResult<TrainTroopsCommand> Execute(TrainTroopsCommand command) {
            var result = new CommandResult<TrainTroopsCommand>();
            var player = _repository.Get(command.Email);
            var troops = new List<TroopInfo>();

            for (var index = 0; index < command.Troops.Count; index++) {
                var i = index; // Don't use iterator in lambdas
                var type = (TroopType)Enum.Parse(typeof(TroopType), command.Troops[i].Type);
                int soldiers = 0;
                int mercenaries = 0;

                if (!string.IsNullOrEmpty(command.Troops[i].Soldiers) && !int.TryParse(command.Troops[i].Soldiers, out soldiers) || soldiers < 0) {
                    result.AddError(c => c.Troops[i].Soldiers, "Invalid number");
                }

                if (!string.IsNullOrEmpty(command.Troops[i].Mercenaries) && !int.TryParse(command.Troops[i].Mercenaries, out mercenaries) || mercenaries < 0) {
                    result.AddError(c => c.Troops[i].Mercenaries, "Invalid number");
                }

                if (result.Success && (soldiers > 0 || mercenaries > 0)) {
                    troops.Add(new TroopInfo(type, soldiers, mercenaries));
                }
            }

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

                _repository.SaveChanges();
            }

            return result;
        }
    }
}
