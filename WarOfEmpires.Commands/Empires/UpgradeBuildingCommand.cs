namespace WarOfEmpires.Commands.Empires {
    public sealed class UpgradeBuildingCommand : ICommand {
        public string Email { get; }
        public string BuildingType { get; }

        public UpgradeBuildingCommand(string email, string buildingType) {
            Email = email;
            BuildingType = buildingType;
        }
    }
}