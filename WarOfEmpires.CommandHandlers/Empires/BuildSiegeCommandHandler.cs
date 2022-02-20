using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Siege;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Linq;

namespace WarOfEmpires.CommandHandlers.Empires {
    public sealed class BuildSiegeCommandHandler : ICommandHandler<BuildSiegeCommand> {
        private readonly IPlayerRepository _repository;

        public BuildSiegeCommandHandler(IPlayerRepository repository) {
            _repository = repository;
        }

        [Audit]
        public CommandResult<BuildSiegeCommand> Execute(BuildSiegeCommand command) {
            var result = new CommandResult<BuildSiegeCommand>();
            var player = _repository.Get(command.Email);
            var availableMaintenance = player.GetWorkerCount(WorkerType.SiegeEngineers) * player.GetBuildingBonus(BuildingType.SiegeFactory)
                - player.SiegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance);
            var siegeWeapons = new List<SiegeWeaponInfo>();

            for (var i = 0; i < command.SiegeWeapons.Count; i++) {
                var type = (SiegeWeaponType)Enum.Parse(typeof(SiegeWeaponType), command.SiegeWeapons[i].Type);

                if (result.Success && command.SiegeWeapons[i].Count.HasValue) {
                    siegeWeapons.Add(new SiegeWeaponInfo(type, command.SiegeWeapons[i].Count.Value));
                }
            }

            if (availableMaintenance < siegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Maintenance)) {
                result.AddError("You don't have enough siege maintenance available to build that much siege");
            }

            if (!player.CanAfford(siegeWeapons.Sum(s => s.Count * SiegeWeaponDefinitionFactory.Get(s.Type).Cost))) {
                result.AddError("You don't have enough resources to build that much siege");
            }

            if (result.Success) {
                foreach (var info in siegeWeapons) {
                    player.BuildSiege(info.Type, info.Count);
                }

                _repository.SaveChanges();
            }

            return result;
        }
    }
}