using WarOfEmpires.Models.Attacks;

namespace WarOfEmpires.Queries.Attacks {
    public sealed class GetAttackDetailsQuery : IQuery<AttackDetailsViewModel> {
        public string Email { get; }
        public int AttackId { get; }

        public GetAttackDetailsQuery(string email, int attackId) {
            Email = email;
            AttackId = attackId;
        }
    }
}