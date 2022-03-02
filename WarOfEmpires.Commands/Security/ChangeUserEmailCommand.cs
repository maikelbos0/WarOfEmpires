using System.Text.Json.Serialization;

namespace WarOfEmpires.Commands.Security {
    public sealed class ChangeUserEmailCommand : ICommand {
        public string CurrentEmail { get; }
        [JsonIgnore]
        public string Password { get; }
        public string NewEmail { get; }

        public ChangeUserEmailCommand(string currentEmail, string password, string newEmail) {
            CurrentEmail = currentEmail;
            Password = password;
            NewEmail = newEmail;
        }
    }
}