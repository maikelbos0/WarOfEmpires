using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Players {
    public class RankService : IRankService {
        public const double TroopModifier = 0.1;
        public const double WorkerModifier = 10.0;
        public const double DefenceModifier = 1000.0;

        private readonly List<TitleDefinition> _titles = TitleDefinitionFactory.GetAll().OrderByDescending(t => t.Type).ToList();

        public double GetRatio(Player dividendPlayer, Player divisorPlayer) {
            var dividend = GetPoints(dividendPlayer);
            var divisor = GetPoints(divisorPlayer);

            if (dividend == 0 && divisor == 0) {
                return 1;
            }
            
            if (divisor == 0) {
                return double.PositiveInfinity;
            }

            return dividend / divisor;
        }

        public virtual void Update(IEnumerable<Player> players) {
            var rank = 1;

            foreach (var player in players
                .Select(p => new { Player = p, RankPoints = GetPoints(p) })
                .OrderByDescending(p => p.RankPoints)
                .Select(p => p.Player)) {

                player.UpdateRank(rank, GetTitle(player, rank++));
            }
        }

        public virtual double GetPoints(Player player) {
            var troops = player.Troops.Select(t => player.GetTroopInfo(t.Type)).Sum(t => t.GetTotalAttack() + t.GetTotalDefense());
            var workers = player.Workers.Sum(w => w.Count);
            var defences = player.GetBuildingBonus(BuildingType.Defences);

            return troops * TroopModifier
                + workers * WorkerModifier
                + defences * DefenceModifier;
        }

        public virtual TitleType GetTitle(Player player, int newRank) {
            return _titles.First(title => title.RequiredDefenceLevel <= player.GetBuildingBonus(BuildingType.Defences)
                && title.RequiredSoldiers <= player.Troops.Sum(t => t.Soldiers)
                && title.RequiredRank >= newRank).Type;
        }
    }
}