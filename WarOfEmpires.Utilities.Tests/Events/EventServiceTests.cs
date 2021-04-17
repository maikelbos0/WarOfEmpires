using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Utilities.Events;

namespace WarOfEmpires.Utilities.Tests.Events {
    [TestClass]
    public sealed class EventServiceTests {
        public class TestEvent : IEvent { }

        public class TestEventHandler : IEventHandler<TestEvent> {
            public void Handle(TestEvent domainEvent) {
                TestHandlerCalled = true;
            }
        }

        public class DoubleTestEvent : IEvent { }

        public class DoubleTestEvent1Handler : IEventHandler<DoubleTestEvent> {
            public void Handle(DoubleTestEvent domainEvent) {
                DoubleTestHandler1Called = true;
            }
        }
        public class DoubleTestEvent2Handler : IEventHandler<DoubleTestEvent> {
            public void Handle(DoubleTestEvent domainEvent) {
                DoubleTestHandler2Called = true;
            }
        }

        public static bool TestHandlerCalled { get; set; }
        public static bool DoubleTestHandler1Called { get; set; }
        public static bool DoubleTestHandler2Called { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public EventServiceTests() {
            _serviceProvider = new ServiceCollection()
                .AddScoped<IEventHandler<TestEvent>, TestEventHandler>()
                .AddScoped<IEventHandler<DoubleTestEvent>, DoubleTestEvent1Handler>()
                .AddScoped<IEventHandler<DoubleTestEvent>, DoubleTestEvent2Handler>()
                .BuildServiceProvider();
        }

        [TestMethod]
        public void EventService_Dispatches_Event() {
            var service = new EventService(_serviceProvider);

            TestHandlerCalled = false;

            service.Dispatch(new TestEvent());

            TestHandlerCalled.Should().BeTrue();
        }

        [TestMethod]
        public void EventService_Dispatches_Event_For_Multiple_Handlers() {
            var service = new EventService(_serviceProvider);

            DoubleTestHandler1Called = false;
            DoubleTestHandler2Called = false;

            service.Dispatch(new DoubleTestEvent());

            DoubleTestHandler1Called.Should().BeTrue();
            DoubleTestHandler2Called.Should().BeTrue();
        }

        [TestMethod]
        public void EventService_Dispatches_Correct_Event() {
            var service = new EventService(_serviceProvider);

            DoubleTestHandler1Called = false;
            DoubleTestHandler2Called = false;

            service.Dispatch(new TestEvent());

            DoubleTestHandler1Called.Should().BeFalse();
            DoubleTestHandler2Called.Should().BeFalse();
        }
    }
}