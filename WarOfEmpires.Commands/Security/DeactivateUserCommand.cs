namespace WarOfEmpires.Commands.Security {
    public sealed class DeactivateUserCommand : ICommand {
        public string Email { get;  }
        public string Password { get; }

        public DeactivateUserCommand(string email, string password) {
            Email = email;
            Password = password;
        }
    }
}