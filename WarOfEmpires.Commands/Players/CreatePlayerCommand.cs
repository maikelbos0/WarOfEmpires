using System;
using System.IO;

namespace WarOfEmpires.Commands.Players {
    public sealed class CreatePlayerCommand : ICommand {
        public string Email { get; }
        public string DisplayName { get; }
        public string FullName { get; }
        public string Description { get; }
        public Func<Stream> Avatar { get; }

        // TODO add race

        public CreatePlayerCommand(string email, string displayName, string fullName, string description, Func<Stream> avatar) {
            Email = email;
            DisplayName = displayName;
            FullName = fullName;
            Description = description;
            Avatar = avatar;
        }
    }
}