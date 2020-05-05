namespace WarOfEmpires.Queries.Security {
    [System.Obsolete]
    public sealed class GetUserIsAdminQuery : IQuery<bool> {
        public string Email { get; private set; }

        public GetUserIsAdminQuery(string email) {
            Email = email;
        }
    }
}