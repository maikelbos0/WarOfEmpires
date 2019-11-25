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

        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ScheduledTaskRepository _repository;
        private readonly FakeEventService _eventService = new FakeEventService();

        public RunScheduledTasksCommandHandlerTests() {
            _repository = new ScheduledTaskRepository(_context);
        }

        public ScheduledTask CreateTask(bool firstSuccess, params bool[] success) {
            var task = Substitute.For<ScheduledTask>();

            task.EventType.Returns(typeof(TestEvent).AssemblyQualifiedName);
            task.Execute().Returns(firstSuccess, success);
            _context.ScheduledTasks.Add(task);
            return task;
        }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Runs_All_Tasks() {
            var handler = new RunScheduledTasksCommandHandler(_repository, _eventService);
            var command = new RunScheduledTasksCommand();
            var tasks = new[] {
                CreateTask(false),
                CreateTask(false),
                CreateTask(false)
            };

            handler.Execute(command);

            foreach (var task in tasks) {
                task.Received().Execute();
            }
        }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Runs_Tasks_Multiple_Times_If_Needed() {
            var handler = new RunScheduledTasksCommandHandler(_repository, _eventService);
            var command = new RunScheduledTasksCommand();
            var task = CreateTask(true, true, false);

            handler.Execute(command);

            task.Received(3).Execute();
        }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Runs_Tasks_Once_If_Needed() {
            var handler = new RunScheduledTasksCommandHandler(_repository, _eventService);
            var command = new RunScheduledTasksCommand();
            var task = CreateTask(false);

            handler.Execute(command);

            task.Received().Execute();
        }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Dispatches_Event_While_Task_Execute_Is_True() {
            var handler = new RunScheduledTasksCommandHandler(_repository, _eventService);
            var command = new RunScheduledTasksCommand();

            CreateTask(true, false);

            handler.Execute(command);

            _eventService.Events.Should().HaveCount(1);
            _eventService.Events.First().Should().BeOfType<TestEvent>();
            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}