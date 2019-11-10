namespace WarOfEmpires.Domain.Schedule {
    public sealed class ScheduledTask : Entity {
        public TaskDefinition Definition { get; set; }
    }
}