namespace WarOfEmpires.Commands.Empires {
    public sealed class RemoveQueuedResearchCommand : ICommand {
        public string Email { get; }
        public int QueudResearchId { get; }

        public RemoveQueuedResearchCommand(string email, int queuedResearchId) {
            Email = email;
            QueudResearchId = queuedResearchId;
        }
    }
}
