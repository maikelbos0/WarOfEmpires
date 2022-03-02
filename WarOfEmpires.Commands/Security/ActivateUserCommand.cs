using System.Text.Json.Serialization;

namespace WarOfEmpires.Commands.Security {
    public sealed class ActivateUserCommand : ICommand {
        public string Email { get; }
        [JsonIgnore]
        public string ActivationCode { get; }

        public ActivateUserCommand(string email, string activationCode) {
            Email = email;
            ActivationCode = activationCode;
        }
    }
}