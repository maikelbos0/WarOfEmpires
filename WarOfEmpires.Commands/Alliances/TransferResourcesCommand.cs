namespace WarOfEmpires.Commands.Alliances {
    public sealed class TransferResourcesCommand : ICommand {
        public string Email { get; }
        public int RecipientId { get; }
        public int? Gold { get; }
        public int? Food { get; }
        public int? Wood { get; }
        public int? Stone { get; }
        public int? Ore { get; }

        public TransferResourcesCommand(string email, int recipientId, int? gold, int? food, int? wood, int? stone, int? ore) {
            Email = email;
            RecipientId = recipientId;
            Gold = gold;
            Food = food;
            Wood = Wood;
            Stone = stone;
            Ore = ore;
        }
    }
}
