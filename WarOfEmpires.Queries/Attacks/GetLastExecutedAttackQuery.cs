namespace WarOfEmpires.Queries.Attacks {
    public sealed class GetLastExecutedAttackQuery : IQuery<int> {
        public string Email { get; }

        public GetLastExecutedAttackQuery(string email) {
            Email = email;
        }
    }
}