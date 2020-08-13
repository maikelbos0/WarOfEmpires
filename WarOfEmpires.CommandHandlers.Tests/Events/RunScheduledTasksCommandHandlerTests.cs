using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using WarOfEmpires.CommandHandlers.Events;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Events {
    [TestClass]
    public sealed class RunScheduledTasksCommandHandlerTests {
        public sealed class TestEvent : IEvent { }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Runs_All_Tasks() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, out var task, false)
                .WithScheduledTask(2, out var anotherTask, false);

            var handler = new RunScheduledTasksCommandHandler(new ScheduledTaskRepository(builder.Context), new FakeEventService());
            var command = new RunScheduledTasksCommand();

            var result = handler.Execute(command);

            result.Success.Should().BeTrue();
            task.Received().Execute();
            anotherTask.Received().Execute();
        }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Runs_Tasks_Multiple_Times_If_Needed() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, out var task, false, eventType: typeof(TestEvent), successCalls: 2);

            var handler = new RunScheduledTasksCommandHandler(new ScheduledTaskRepository(builder.Context), new FakeEventService());
            var command = new RunScheduledTasksCommand();

            handler.Execute(command);

            task.Received(3).Execute();
            builder.Context.CallsToSaveChanges.Should().Be(2);
        }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Dispatches_Event_While_Task_Execute_Is_True() {
            var eventService = new FakeEventService();
            var builder = new FakeBuilder()
                .WithScheduledTask(1, false, eventType: typeof(TestEvent), successCalls: 1);

            var handler = new RunScheduledTasksCommandHandler(new ScheduledTaskRepository(builder.Context), eventService);
            var command = new RunScheduledTasksCommand();
                        
            handler.Execute(command);

            eventService.Events.Should().HaveCount(1);
            eventService.Events.First().Should().BeOfType<TestEvent>();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}