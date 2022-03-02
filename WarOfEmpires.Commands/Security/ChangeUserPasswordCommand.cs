using System.Text.Json.Serialization;

namespace WarOfEmpires.Commands.Security {
    public sealed class ChangeUserPasswordCommand : ICommand {
        public string Email { get; }
        [JsonIgnore]
        public string CurrentPassword { get; }
        [JsonIgnore]
        public string NewPassword { get; }

        public ChangeUserPasswordCommand(string email, string currentPassword, string newPassword) {
            Email = email;
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}