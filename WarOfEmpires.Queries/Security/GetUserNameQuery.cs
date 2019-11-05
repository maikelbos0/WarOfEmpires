namespace WarOfEmpires.Queries.Security {
    public sealed class GetUserNameQuery : IQuery<string> {
        public string Email { get; private set; }

        public GetUserNameQuery(string email) {
            Email = email;
        }
    }
}