using System;
using System.IO;
using System.Text.Json.Serialization;

namespace WarOfEmpires.Commands.Players {
    public sealed class CreatePlayerCommand : ICommand {
        public string Email { get; }
        public string DisplayName { get; }
        public string Race { get; }
        public string FullName { get; }
        public string Description { get; }
        [JsonIgnore]
        public Func<Stream> Avatar { get; }

        public CreatePlayerCommand(string email, string displayName, string race, string fullName, string description, Func<Stream> avatar) {
            Email = email;
            DisplayName = displayName;
            Race = race;
            FullName = fullName;
            Description = description;
            Avatar = avatar;
        }
    }
}
