namespace WarOfEmpires.Queries.Security {
    public sealed class GetUserIsAdminQuery : IQuery<bool> {
        public string Email { get; }

        public GetUserIsAdminQuery(string email) {
            Email = email;
        }
    }
}