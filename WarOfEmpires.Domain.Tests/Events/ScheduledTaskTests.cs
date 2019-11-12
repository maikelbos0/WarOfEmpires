using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Domain.Tests.Events {
    [TestClass]
    public sealed class ScheduledTaskTests {
        public static bool TestHandlerCalled { get; set; }

        public class TestEvent : IEvent { }

        public class TestEventHandler : IEventHandler<TestEvent> {
            public void Handle(TestEvent domainEvent) {
                TestHandlerCalled = true;
            }
        }

        public void MoveTaskDate(ScheduledTask task, TimeSpan time) {
            var property = typeof(ScheduledTask).GetProperty(nameof(ScheduledTask.LastExecutionDate));

            property.SetValue(task, (DateTime)property.GetValue(task) + time);
        }

        [TestMethod]
        public void ScheduledTask_Is_Created_Paused() {
            var task = ScheduledTask.Create<TestEvent>(new TimeSpan(0, 5, 0));

            task.IsPaused.Should().BeTrue();
            task.LastExecutionDate.Should().BeNull();
        }

        [TestMethod]
        public void ScheduledTask_Unpause_Succeeds() {
            var task = ScheduledTask.Create<TestEvent>(new TimeSpan(0, 5, 0));

            task.Unpause();
            task.IsPaused.Should().BeFalse();
            task.LastExecutionDate.Should().BeAfter(DateTime.UtcNow.AddMinutes(-5));
            task.LastExecutionDate.Should().BeBefore(DateTime.UtcNow);
        }

        [TestMethod]
        public void ScheduledTask_Pause_Succeeds() {
            var task = ScheduledTask.Create<TestEvent>(new TimeSpan(0, 5, 0));

            task.Unpause();
            task.Pause();

            task.IsPaused.Should().BeTrue();
            task.LastExecutionDate.Should().BeNull();
        }

        [TestMethod]
        public void ScheduledTask_Execute_Does_Nothing_When_Paused() {
            var task = ScheduledTask.Create<TestEvent>(new TimeSpan(0, 5, 0));

            TestHandlerCalled = false;

            task.Execute().Should().BeFalse();
            TestHandlerCalled.Should().BeFalse();
        }

        [TestMethod]
        public void ScheduledTask_Execute_Does_Nothing_When_Not_Scheduled_Yet() {
            var task = ScheduledTask.Create<TestEvent>(new TimeSpan(0, 5, 0));

            task.Unpause();
            TestHandlerCalled = false;

            task.Execute().Should().BeFalse();
            TestHandlerCalled.Should().BeFalse();
        }

        [TestMethod]
        public void ScheduledTask_Execute_Succeeds() {
            var task = ScheduledTask.Create<TestEvent>(new TimeSpan(0, 5, 0));

            task.Unpause();
            MoveTaskDate(task, new TimeSpan(0, -5, 0));
            TestHandlerCalled = false;

            task.Execute().Should().BeTrue();
            task.NextExecutionDate.Should().BeAfter(DateTime.UtcNow);
            TestHandlerCalled.Should().BeTrue();
        }

        [TestMethod]
        public void ScheduledTask_Execute_Executes_Twice_When_Behind() {
            var task = ScheduledTask.Create<TestEvent>(new TimeSpan(0, 5, 0));

            task.Unpause();
            MoveTaskDate(task, new TimeSpan(0, -10, 0));

            task.Execute().Should().BeTrue();
            task.Execute().Should().BeTrue();
            task.Execute().Should().BeFalse();
        }
    }
}