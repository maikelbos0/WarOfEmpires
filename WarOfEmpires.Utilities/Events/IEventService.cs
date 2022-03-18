using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Utilities.Events {
    public interface IEventService {
        void Dispatch<TEvent>(TEvent domainEvent) where TEvent : IEvent;
    }
}