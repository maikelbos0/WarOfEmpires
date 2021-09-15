namespace WarOfEmpires.Commands.Empires {
    public sealed class QueueResearchCommand : ICommand {
        public string Email { get; }
        public string ResearchType { get; }

        public QueueResearchCommand(string email, string researchType) {
            Email = email;
            ResearchType = researchType;
        }
    }
}
