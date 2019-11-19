using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Utilities.Events {
    public interface IEventHandler<TEvent> where TEvent : IEvent {
        void Handle(TEvent domainEvent);
    }
}