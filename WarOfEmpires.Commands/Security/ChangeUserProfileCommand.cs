namespace WarOfEmpires.Commands.Security {
    public sealed class ChangeUserProfileCommand : ICommand {
        public string Email { get; private set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool ShowEmail { get; set; }

        public ChangeUserProfileCommand(string email) {
            Email = email;
        }
    }
}