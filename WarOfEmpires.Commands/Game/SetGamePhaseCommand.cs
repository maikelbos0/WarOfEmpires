namespace WarOfEmpires.Commands.Game {
    public sealed class SetGamePhaseCommand : ICommand {
        public string Phase { get; }

        public SetGamePhaseCommand(string phase) {
            Phase = phase;
        }
    }
}
