using System.Text.Json.Serialization;

namespace WarOfEmpires.Commands.Security {
    public sealed class DeactivateUserCommand : ICommand {
        public string Email { get;  }
        [JsonIgnore]
        public string Password { get; }

        public DeactivateUserCommand(string email, string password) {
            Email = email;
            Password = password;
        }
    }
}