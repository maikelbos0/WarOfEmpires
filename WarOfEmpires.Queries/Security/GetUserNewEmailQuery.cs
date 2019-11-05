namespace WarOfEmpires.Queries.Security {
    public sealed class GetUserNewEmailQuery : IQuery<string> {
        public string Email { get; private set; }

        public GetUserNewEmailQuery(string email) {
            Email = email;
        }
    }
}