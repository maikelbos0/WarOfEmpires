using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Events;

namespace WarOfEmpires.Domain.Tests.Events {
    [TestClass]
    public sealed class ScheduledTaskTests {
        public class TestEvent : IEvent { }

        public void MoveTaskDate(ScheduledTask task, TimeSpan time) {
            var property = typeof(ScheduledTask).GetProperty(nameof(ScheduledTask.LastExecutionDate));

            property.SetValue(task, (DateTime)property.GetValue(task) + time);
        }

        [TestMethod]
        public void ScheduledTask_Is_Created_Paused() {
            var task = ScheduledTask.Create<TestEvent>(1, new TimeSpan(0, 5, 0), TaskExecutionMode.ExecuteAllIntervals);

            task.IsPaused.Should().BeTrue();
            task.LastExecutionDate.Should().BeNull();
        }

        [TestMethod]
        public void ScheduledTask_Unpause_Succeeds() {
            var task = ScheduledTask.Create<TestEvent>(1, new TimeSpan(0, 5, 0), TaskExecutionMode.ExecuteAllIntervals);

            task.Unpause();
            task.IsPaused.Should().BeFalse();
            task.LastExecutionDate.Should().BeAfter(DateTime.UtcNow.AddMinutes(-5));
            task.LastExecutionDate.Should().BeBefore(DateTime.UtcNow);
        }

        [TestMethod]
        public void ScheduledTask_Pause_Succeeds() {
            var task = ScheduledTask.Create<TestEvent>(1, new TimeSpan(0, 5, 0), TaskExecutionMode.ExecuteAllIntervals);

            task.Unpause();
            task.Pause();

            task.IsPaused.Should().BeTrue();
            task.LastExecutionDate.Should().BeNull();
        }

        [TestMethod]
        public void ScheduledTask_Execute_Does_Nothing_When_Paused() {
            var task = ScheduledTask.Create<TestEvent>(1, new TimeSpan(0, 5, 0), TaskExecutionMode.ExecuteAllIntervals);

            task.Execute().Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow(TaskExecutionMode.ExecuteAllIntervals)]
        [DataRow(TaskExecutionMode.ExecuteOnce)]
        public void ScheduledTask_Execute_Does_Nothing_When_Not_Scheduled_Yet(TaskExecutionMode executionMode) {
            var task = ScheduledTask.Create<TestEvent>(1, new TimeSpan(0, 5, 0), executionMode);

            task.Unpause();

            task.Execute().Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow(TaskExecutionMode.ExecuteAllIntervals)]
        [DataRow(TaskExecutionMode.ExecuteOnce)]
        public void ScheduledTask_Execute_Succeeds(TaskExecutionMode executionMode) {
            var task = ScheduledTask.Create<TestEvent>(1, new TimeSpan(0, 5, 0), executionMode);

            task.Unpause();
            MoveTaskDate(task, new TimeSpan(0, -5, 0));

            task.Execute().Should().BeTrue();
            task.NextExecutionDate.Should().BeAfter(DateTime.UtcNow);
        }

        [DataTestMethod]
        [DataRow(TaskExecutionMode.ExecuteAllIntervals, 3, DisplayName = "All intervals")]
        [DataRow(TaskExecutionMode.ExecuteOnce, 1, DisplayName = "Once")]
        public void ScheduledTask_Execute_Succeeds_When_Behind(TaskExecutionMode executionMode, int expectedExecutions) {
            var task = ScheduledTask.Create<TestEvent>(1, new TimeSpan(0, 5, 0), executionMode);

            task.Unpause();
            MoveTaskDate(task, new TimeSpan(0, -15, 0));

            for (var i = 0; i < expectedExecutions; i++) {
                task.Execute().Should().BeTrue();
            }

            task.Execute().Should().BeFalse();
        }
    }
}