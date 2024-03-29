﻿using System.Collections.Generic;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.Test.Utilities {
    public sealed class FakeEventService : IEventService {
        public List<IEvent> Events { get; set; } = new List<IEvent>();

        public void Dispatch<TEvent>(TEvent domainEvent) where TEvent : IEvent {
            Events.Add(domainEvent);
        }
    }
}
