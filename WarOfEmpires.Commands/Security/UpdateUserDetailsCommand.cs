namespace WarOfEmpires.Commands.Security {
    public sealed class UpdateUserDetailsCommand : ICommand {
        public int Id { get; }
        public string Email { get; }
        public string DisplayName { get; }
        public string AllianceCode { get; }
        public string AllianceName { get; }
        public string Status { get; }
        public bool IsAdmin { get; }

        public UpdateUserDetailsCommand(int id, string email, string displayName, string allianceCode, string allianceName, string status, bool isAdmin) {
            Id = id;
            Email = email;
            DisplayName = displayName;
            AllianceCode = allianceCode;
            AllianceName = allianceName;
            Status = status;
            IsAdmin = isAdmin;
        }
    }
}