using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Domain.Reflection;

namespace WarOfEmpires.Domain.Tests.Events {
    [TestClass]
    public sealed class EventServiceTests {
        public static bool TestHandlerCalled { get; set; }
        public static bool DoubleTestHandler1Called { get; set; }
        public static bool DoubleTestHandler2Called { get; set; }

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

        public IClassFinder GetClassFinder() {
            var classFinder = Substitute.For<IClassFinder>();

            classFinder.FindAllClasses().Returns(new Type[] {
                typeof(TestEventHandler),
                typeof(DoubleTestEvent1Handler),
                typeof(DoubleTestEvent2Handler)
            });

            return classFinder;
        }

        [TestMethod]
        public void EventService_Dispatches_Event() {
            var service = new EventService(GetClassFinder());

            TestHandlerCalled = false;

            service.Dispatch(new TestEvent());

            TestHandlerCalled.Should().BeTrue();
        }

        [TestMethod]
        public void EventService_Dispatches_Event_For_Multiple_Handlers() {
            var service = new EventService(GetClassFinder());

            DoubleTestHandler1Called = false;
            DoubleTestHandler2Called = false;

            service.Dispatch(new DoubleTestEvent());

            DoubleTestHandler1Called.Should().BeTrue();
            DoubleTestHandler2Called.Should().BeTrue();
        }

        [TestMethod]
        public void EventService_Dispatches_Correct_Event() {
            var service = new EventService(GetClassFinder());

            DoubleTestHandler1Called = false;
            DoubleTestHandler2Called = false;

            service.Dispatch(new TestEvent());

            DoubleTestHandler1Called.Should().BeFalse();
            DoubleTestHandler2Called.Should().BeFalse();
        }
    }
}