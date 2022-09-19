using System;
using System.IO;
using System.Text.Json.Serialization;

namespace WarOfEmpires.Commands.Players {
    public sealed class UpdateProfileCommand : ICommand {
        public string Email { get; }
        public string FullName { get; }
        public string Description { get; }
        [JsonIgnore]
        public Func<Stream> Avatar { get; }

        public UpdateProfileCommand(string email, string fullName, string description, Func<Stream> avatar) {
            Email = email;
            FullName = fullName;
            Description = description;
            Avatar = avatar;
        }
    }
}
