namespace WarOfEmpires.Domain.Events {
    public interface IEventHandler<TEvent> where TEvent : IEvent {
        void Handle(TEvent domainEvent);
    }
}