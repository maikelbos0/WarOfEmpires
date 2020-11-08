using WarOfEmpires.Models.Attacks;

namespace WarOfEmpires.Queries.Attacks {
    public class GetDefenderQuery : IQuery<ExecuteAttackModel> {
        public int Id { get; }

        public GetDefenderQuery(int id) {
            Id = id;
        }
    }
}