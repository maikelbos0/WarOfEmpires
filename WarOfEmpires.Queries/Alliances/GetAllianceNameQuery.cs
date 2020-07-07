namespace WarOfEmpires.Queries.Alliances {
    public sealed class GetAllianceNameQuery : IQuery<string> {
        public string Email { get; set; }

        public GetAllianceNameQuery(string email) {
            Email = email;
        }
    }
}