namespace WarOfEmpires.Commands.Empires {
    public sealed class PrioritizeResearchCommand : ICommand {
        public string Email { get; }
        public string ResearchType { get; }

        public PrioritizeResearchCommand(string email, string researchType) {
            Email = email;
            ResearchType = researchType;
        }
    }
}
