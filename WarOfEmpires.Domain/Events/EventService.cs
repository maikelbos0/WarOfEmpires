using System;
using System.Collections.Generic;
using System.Linq;
using WarOfEmpires.Domain.Reflection;

namespace WarOfEmpires.Domain.Events {
    public sealed class EventService {
        public static EventService Service => new EventService(new ClassFinder());

        private readonly IClassFinder _classFinder;
        private readonly Dictionary<Type, List<Type>> _handlerMap;

        public EventService(IClassFinder classFinder) {
            _classFinder = classFinder;     
            _handlerMap = _classFinder
                .FindAllClasses()
                .SelectMany(t => t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)).Select(i => new { Type = t, Interface = i }))
                .GroupBy(x => x.Interface.GetGenericArguments().First())
                .ToDictionary(g => g.Key, g => g.Select(x => x.Type).ToList());
        }

        public void Dispatch(IEvent domainEvent) {
            if (_handlerMap.TryGetValue(domainEvent.GetType(), out List<Type> handlerTypes)) {
                foreach (var handlerType in handlerTypes) {
                    // TODO: use Unity Container to resolve event handlers to make sure constructor arguments are injected when needed
                    dynamic handler = Activator.CreateInstance(handlerType);
                    handler.Handle((dynamic)domainEvent);
                }
            }
        }
    }
}