using Microsoft.Extensions.DependencyInjection;
using System;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Utilities.Events {
    [TransientServiceImplementation(typeof(IEventService))]
    public sealed class EventService : IEventService {
        private readonly IServiceProvider _serviceProvider;

        public EventService(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public void Dispatch(IEvent domainEvent) {
            foreach (dynamic handler in _serviceProvider.GetServices(typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType()))) {
                handler.Handle((dynamic)domainEvent);
            }
        }
    }
}