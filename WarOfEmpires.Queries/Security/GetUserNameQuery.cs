namespace WarOfEmpires.Queries.Security {
    [System.Obsolete]
    public sealed class GetUserNameQuery : IQuery<string> {
        public string Email { get; private set; }

        public GetUserNameQuery(string email) {
            Email = email;
        }
    }
}