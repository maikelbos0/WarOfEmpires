namespace WarOfEmpires.Commands.Security {
    public sealed class UpdateUserDetailsCommand : ICommand {
        public string Email { get; }
        public string DisplayName { get; }
        public string AllianceCode { get; }
        public string AllianceName { get; }
        public string Status { get; }
        public bool IsAdmin { get; }

        public UpdateUserDetailsCommand(string email, string displayName, string allianceCode, string allianceName, string status, bool isAdmin) {
            Email = email;
            DisplayName = displayName;
            AllianceCode = allianceCode;
            AllianceName = allianceName;
            Status = status;
            IsAdmin = isAdmin;
        }
    }
}