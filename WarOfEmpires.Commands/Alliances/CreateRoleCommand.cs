﻿namespace WarOfEmpires.Commands.Alliances {
    public sealed class CreateRoleCommand : ICommand {
        public string Email { get; }
        public string Name { get; }
        public bool CanInvite { get; }
        public bool CanManageRoles { get; }
        public bool CanDeleteChatMessages { get; }
        public bool CanKickMembers { get; }
        public bool CanManageNonAggressionPacts { get; }
        public bool CanManageWars { get; }
        public bool CanBank { get; }

        public CreateRoleCommand(string email, string name, bool canInvite, bool canManageRoles, bool canDeleteChatMessages, bool canKickMembers, bool canManageNonAggressionPacts, bool canManageWars, bool canBank) {
            Email = email;
            Name = name;
            CanInvite = canInvite;
            CanManageRoles = canManageRoles;
            CanDeleteChatMessages = canDeleteChatMessages;
            CanKickMembers = canKickMembers;
            CanManageNonAggressionPacts = canManageNonAggressionPacts;
            CanManageWars = canManageWars;
            CanBank = canBank;
        }
    }
}