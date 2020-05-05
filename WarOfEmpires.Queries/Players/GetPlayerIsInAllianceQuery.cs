namespace WarOfEmpires.Queries.Players {
    [System.Obsolete]
    public sealed class GetPlayerIsInAllianceQuery : IQuery<bool> {
        public string Email { get; private set; }

        public GetPlayerIsInAllianceQuery(string email) {
            Email = email;
        }
    }
}