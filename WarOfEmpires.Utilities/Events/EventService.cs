using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Utilities.Reflection;

namespace WarOfEmpires.Utilities.Events {
    [ScopedServiceImplementation(typeof(IEventService))]
    public sealed class EventService : IEventService {
        private readonly IServiceProvider _serviceProvider;
        private readonly IClassFinder _classFinder;
        private readonly Dictionary<Type, List<Type>> _handlerMap;

        public EventService(IClassFinder classFinder, IServiceProvider serviceProvider) {
            _classFinder = classFinder;
            _serviceProvider = serviceProvider;
            _handlerMap = _classFinder
                .FindAllClasses()
                .SelectMany(t => t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)).Select(i => new { Type = t, Interface = i }))
                .GroupBy(x => x.Interface.GetGenericArguments().First())
                .ToDictionary(g => g.Key, g => g.Select(x => x.Type).ToList());
        }

        public void Dispatch(IEvent domainEvent) {
            if (_handlerMap.TryGetValue(domainEvent.GetType(), out List<Type> handlerTypes)) {
                foreach (var handlerType in handlerTypes) {
                    dynamic handler = _serviceProvider.GetRequiredService(handlerType);
                    handler.Handle((dynamic)domainEvent);
                }
            }
        }
    }
}