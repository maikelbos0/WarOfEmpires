﻿namespace WarOfEmpires.Commands.Empires {
    public sealed class BuildSiegeCommand : ICommand {
        public string Email { get; }
        public string FireArrows { get; }
        public string BatteringRams { get; }
        public string ScalingLadders { get; }

        public BuildSiegeCommand(string email, string fireArrows, string batteringRams, string scalingLadders) {
            Email = email;
            FireArrows = fireArrows;
            BatteringRams = batteringRams;
            ScalingLadders = scalingLadders;
        }
    }
}