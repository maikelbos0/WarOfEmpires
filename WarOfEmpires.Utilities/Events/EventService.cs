using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Utilities.Container;
using WarOfEmpires.Utilities.Reflection;

namespace WarOfEmpires.Utilities.Events {
    [InterfaceInjectable]
    public sealed class EventService : IEventService {
        private readonly IUnityContainer _container;
        private readonly IClassFinder _classFinder;
        private readonly Dictionary<Type, List<Type>> _handlerMap;

        public EventService(IClassFinder classFinder, IUnityContainer container) {
            _classFinder = classFinder;
            _container = container;
            _handlerMap = _classFinder
                .FindAllClasses()
                .SelectMany(t => t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)).Select(i => new { Type = t, Interface = i }))
                .GroupBy(x => x.Interface.GetGenericArguments().First())
                .ToDictionary(g => g.Key, g => g.Select(x => x.Type).ToList());
        }

        public void Dispatch(IEvent domainEvent) {
            if (_handlerMap.TryGetValue(domainEvent.GetType(), out List<Type> handlerTypes)) {
                foreach (var handlerType in handlerTypes) {
                    dynamic handler = _container.Resolve(handlerType);
                    handler.Handle((dynamic)domainEvent);
                }
            }
        }
    }
}