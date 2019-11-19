using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Utilities.Events {
    public interface IEventService {
        void Dispatch(IEvent domainEvent);
    }
}