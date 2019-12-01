namespace WarOfEmpires.Queries.Players {
    public sealed class GetPlayerDetailsQuery : IQuery<object> {
        public string PlayerId { get; }
    }
}