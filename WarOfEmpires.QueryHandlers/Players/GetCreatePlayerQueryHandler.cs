using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Players {
    [TransientServiceImplementation(typeof(IQueryHandler<GetCreatePlayerQuery, CreatePlayerModel>))]
    public class GetCreatePlayerQueryHandler : IQueryHandler<GetCreatePlayerQuery, CreatePlayerModel> {
        private readonly IEnumFormatter _formatter;

        public GetCreatePlayerQueryHandler(IEnumFormatter formatter) {
            _formatter = formatter;
        }

        [Audit]
        public CreatePlayerModel Execute(GetCreatePlayerQuery query) {
            return new CreatePlayerModel() {
                Races = RaceDefinitionFactory
                    .GetAll()
                    .Select(r => new RaceViewModel() {
                        Race = r.Race.ToString(),
                        Name = _formatter.ToString(r.Race),
                        Description = r.Description,
                        FarmerModifier = r.GetWorkerModifier(WorkerType.Farmers),
                        WoodWorkerModifier = r.GetWorkerModifier(WorkerType.WoodWorkers),
                        StoneMasonModifier = r.GetWorkerModifier(WorkerType.StoneMasons),
                        OreMinerModifier = r.GetWorkerModifier(WorkerType.OreMiners),
                        ArcherModifier = r.GetTroopModifier(TroopType.Archers),
                        CavalryModifier = r.GetTroopModifier(TroopType.Cavalry),
                        FootmenModifier = r.GetTroopModifier(TroopType.Footmen)
                    })
                    .ToList()
            };
        }
    }
}
