using System;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<TrainTroopsCommand>))]
    public sealed class TrainTroopsCommandHandler : ICommandHandler<TrainTroopsCommand> {
        private readonly IPlayerRepository _repository;

        public TrainTroopsCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<TrainTroopsCommand> Execute(TrainTroopsCommand command) {
            var result = new CommandResult<TrainTroopsCommand>();
            var player = _repository.Get(command.Email);
            var troops = new List<TroopInfo>();

            for (var i = 0; i < command.Troops.Count; i++) {
                var type = (TroopType)Enum.Parse(typeof(TroopType), command.Troops[i].Type);
                
                if (result.Success && (command.Troops[i].Soldiers.HasValue || command.Troops[i].Mercenaries.HasValue)) {
                    troops.Add(new TroopInfo(type, command.Troops[i].Soldiers ?? 0, command.Troops[i].Mercenaries ?? 0));
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
