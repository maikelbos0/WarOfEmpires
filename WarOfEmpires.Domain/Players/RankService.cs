using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Players {
    [ScopedServiceImplementation(typeof(IRankService))]
    public class RankService : IRankService {
        public const double TroopModifier = 0.1;
        public const double WorkerModifier = 10.0;
        public const double DefenceModifier = 1000.0;

        private readonly List<TitleDefinition> _titles = TitleDefinitionFactory.GetAll().OrderByDescending(t => t.Type).ToList();

        public double GetPoints(Player player) {
            var troops = player.Troops.Select(t => player.GetTroopInfo(t.Type)).Sum(t => t.GetTotalAttack() + t.GetTotalDefense());
            var workers = player.Workers.Sum(w => w.Count);
            var defences = player.GetBuildingBonus(BuildingType.Defences);

            return troops * TroopModifier
                + workers * WorkerModifier
                + defences * DefenceModifier;
        }

        public virtual TitleType GetTitle(Player player) {
            return _titles.First(title => title.RequiredDefenceLevel <= player.GetBuildingBonus(BuildingType.Defences)
                && title.RequiredSoldiers <= player.Troops.Sum(t => t.Soldiers)
                && title.MeetsAdditionalRequirements(player)).Type;
        }

        public virtual void Update(IEnumerable<Player> players) {
            var rank = 1;

            foreach (var player in players
                .Select(p => new { Player = p, RankPoints = GetPoints(p) })
                .OrderByDescending(p => p.RankPoints)
                .Select(p => p.Player)) {

                player.UpdateRank(rank++, GetTitle(player));
            }
        }
    }
}