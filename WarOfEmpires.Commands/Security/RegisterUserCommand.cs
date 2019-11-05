namespace WarOfEmpires.Commands.Security {
    public sealed class RegisterUserCommand : ICommand {
        public string Email { get; }
        public string Password { get; }

        public RegisterUserCommand(string email, string password) {
            Email = email;
            Password = password;
        }
    }
}