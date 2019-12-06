using WarOfEmpires.Models.Players;

namespace WarOfEmpires.Queries.Players {
    public sealed class GetNotificationsQuery : IQuery<NotificationsViewModel> {
        public string Email { get; }

        public GetNotificationsQuery(string email) {
            Email = email;
        }
    }
}