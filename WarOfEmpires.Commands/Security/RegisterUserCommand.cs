using System.Text.Json.Serialization;

namespace WarOfEmpires.Commands.Security {
    public sealed class RegisterUserCommand : ICommand {
        public string Email { get; }
        [JsonIgnore]
        public string Password { get; }

        public RegisterUserCommand(string email, string password) {
            Email = email;
            Password = password;
        }
    }
}