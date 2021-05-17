using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Database;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Queries.Empires;
using WarOfEmpires.QueryHandlers.Common;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.QueryHandlers.Empires {
    [TransientServiceImplementation(typeof(IQueryHandler<GetWorkersQuery, WorkersModel>))]
    public sealed class GetWorkersQueryHandler : IQueryHandler<GetWorkersQuery, WorkersModel> {
        private readonly IWarContext _context;
        private readonly IResourcesMap _resourcesMap;
        private readonly IEnumFormatter _formatter;

        public GetWorkersQueryHandler(IWarContext context, IResourcesMap resourcesMap, IEnumFormatter formatter) {
            _context = context;
            _resourcesMap = resourcesMap;
            _formatter = formatter;
        }

        [Audit]
        public WorkersModel Execute(GetWorkersQuery query) {
            var player = _context.Players
                .Include(p => p.Buildings)
                .Include(p => p.Troops)
                .Include(p => p.Workers)
                .Single(p => EmailComparisonService.Equals(p.User.Email, query.Email));

            return new WorkersModel() {
                CurrentPeasants = player.Peasants,
                CurrentGoldPerWorkerPerTurn = player.GetGoldPerWorkerPerTurn(),
                CurrentGoldPerTurn = player.GetGoldPerTurn(),
                Workers = new List<WorkerModel>() {
                    MapWorkers(player, WorkerType.Farmers),
                    MapWorkers(player, WorkerType.WoodWorkers),
                    MapWorkers(player, WorkerType.StoneMasons),
                    MapWorkers(player, WorkerType.OreMiners),
                    MapWorkers(player, WorkerType.SiegeEngineers),
                    MapWorkers(player, WorkerType.Merchants),
                },
                WorkerCost = _resourcesMap.ToViewModel(WorkerDefinitionFactory.Get(WorkerType.WoodWorkers).Cost),
                UpkeepPerTurn = _resourcesMap.ToViewModel(player.GetUpkeepPerTurn()),
                RecruitsPerDay = player.GetRecruitsPerDay(),
                WillUpkeepRunOut = player.WillUpkeepRunOut(),
                HasUpkeepRunOut = player.HasUpkeepRunOut
            };
        }

        private WorkerModel MapWorkers(Player player, WorkerType type) {
            var definition = WorkerDefinitionFactory.Get(type);
            var model = new WorkerModel() {
                Type = type.ToString(),
                IsProducer = definition.IsProducer,
                Name = _formatter.ToString(type),
                Cost = _resourcesMap.ToViewModel(definition.Cost),
                CurrentWorkers = player.GetWorkerCount(type)
            };

            if (definition.IsProducer) {
                var productionInfo = player.GetProduction(type);

                model.CurrentProductionPerWorkerPerTurn = productionInfo.GetProductionPerWorker();
                model.CurrentProductionPerTurn = productionInfo.GetTotalProduction();
            }
            else {
                model.CurrentProductionPerWorkerPerTurn = player.GetBuildingBonus(definition.BuildingType);
                model.CurrentProductionPerTurn = player.GetBuildingBonus(definition.BuildingType) * model.CurrentWorkers;
            }

            return model;
        }
    }
}