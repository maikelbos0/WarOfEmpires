using System.Linq;
using WarOfEmpires.Domain.Attacks;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Utilities.Auditing;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.QueryHandlers.Players {
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
                        FarmerBonus = r.GetWorkerModifier(WorkerType.Farmers) - 1,
                        WoodWorkerBonus = r.GetWorkerModifier(WorkerType.WoodWorkers) - 1,
                        StoneMasonBonus = r.GetWorkerModifier(WorkerType.StoneMasons) - 1,
                        OreMinerBonus = r.GetWorkerModifier(WorkerType.OreMiners) - 1,
                        ArcherBonus = r.GetTroopModifier(TroopType.Archers) - 1,
                        CavalryBonus = r.GetTroopModifier(TroopType.Cavalry) - 1,
                        FootmenBonus = r.GetTroopModifier(TroopType.Footmen) - 1
                    })
                    .ToList()
            };
        }
    }
}
