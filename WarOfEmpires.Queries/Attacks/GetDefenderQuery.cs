using WarOfEmpires.Models.Attacks;

namespace WarOfEmpires.Queries.Attacks {
    public class GetDefenderQuery : IQuery<ExecuteAttackModel> {
        public string Id { get; }

        public GetDefenderQuery(string id) {
            Id = id;
        }
    }
}