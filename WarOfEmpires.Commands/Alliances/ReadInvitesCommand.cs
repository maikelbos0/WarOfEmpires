namespace WarOfEmpires.Commands.Alliances {
    public sealed class ReadInvitesCommand : ICommand {
        public string Email { get; }

        public ReadInvitesCommand(string email) {
            Email = email;
        }
    }
}