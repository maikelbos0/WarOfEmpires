using WarOfEmpires.Models.Attacks;

namespace WarOfEmpires.Queries.Attacks {
    public class GetDefenderQuery : IQuery<ExecuteAttackModel> {
        public string Email { get; }
        public int Id { get; }

        public GetDefenderQuery(string email, int id) {
            Email = email;
            Id = id;
        }
    }
}