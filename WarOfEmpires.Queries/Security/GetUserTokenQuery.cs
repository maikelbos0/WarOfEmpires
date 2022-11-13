namespace WarOfEmpires.Queries.Security {
    public sealed class GetUserTokenQuery : IQuery<string> {
        public string Email { get; }

        public GetUserTokenQuery(string email) {
            Email = email;
        }
    }
}