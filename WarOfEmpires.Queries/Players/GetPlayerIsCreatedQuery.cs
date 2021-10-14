namespace WarOfEmpires.Queries.Players {
    public sealed class GetPlayerIsCreatedQuery : IQuery<bool> {
        public string Email { get; }

        public GetPlayerIsCreatedQuery(string email) {
            Email = email;
        }
    }
}
