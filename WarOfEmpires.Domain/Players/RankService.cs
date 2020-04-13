using System.Linq;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Players {
    public class RankService {
        public const double TroopModifier = 0.1;
        public const double WorkerModifier = 10.0;
        public const double DefenceModifier = 1000.0;

        public virtual double GetRankPoints(Player player) {
            var troops = player.Troops.Select(t => player.GetTroopInfo(t.Type)).Sum(t => t.GetTotalAttack() + t.GetTotalDefense());
            var workers = player.Workers.Sum(w => w.Count);
            var defences = player.GetBuildingBonus(BuildingType.Defences);

            return troops * TroopModifier
                + workers * WorkerModifier
                + defences * DefenceModifier;
        }
    }
}