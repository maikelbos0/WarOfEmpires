using Microsoft.Extensions.DependencyInjection;
using System;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Utilities.Events {
    public sealed class EventService : IEventService {
        private readonly IServiceProvider _serviceProvider;

        public EventService(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public void Dispatch<TEvent>(TEvent domainEvent) where TEvent : IEvent {
            foreach (var handler in _serviceProvider.GetServices<IEventHandler<TEvent>>()) {
                handler.Handle(domainEvent);
            }
        }
    }
}