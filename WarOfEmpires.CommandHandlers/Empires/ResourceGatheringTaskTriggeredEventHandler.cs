using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Empires;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.CommandHandlers.Empires {
    public sealed class ResourceGatheringTaskTriggeredEventHandler : IEventHandler<ResourceGatheringTaskTriggeredEvent> {
        private readonly GatherResourcesCommandHandler _gatherResourcesCommandHandler;

        public ResourceGatheringTaskTriggeredEventHandler(GatherResourcesCommandHandler gatherResourcesCommandHandler) {
            _gatherResourcesCommandHandler = gatherResourcesCommandHandler;
        }

        public void Handle(ResourceGatheringTaskTriggeredEvent domainEvent) {
            _gatherResourcesCommandHandler.Execute(new GatherResourcesCommand());
        }
    }
}