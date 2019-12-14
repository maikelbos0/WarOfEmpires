using WarOfEmpires.Models.Attacks;

namespace WarOfEmpires.Queries.Attacks {
    public sealed class GetAttackDetailsQuery : IQuery<AttackDetailsViewModel> {
        public string Email { get; }
        public string AttackId { get; }

        public GetAttackDetailsQuery(string email, string attackId) {
            Email = email;
            AttackId = attackId;
        }
    }
}