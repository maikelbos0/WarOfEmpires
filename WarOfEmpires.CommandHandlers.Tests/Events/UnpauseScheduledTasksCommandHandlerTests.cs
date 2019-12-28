using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Events;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Events {
    [TestClass]
    public sealed class UnpauseScheduledTasksCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ScheduledTaskRepository _repository;

        public UnpauseScheduledTasksCommandHandlerTests() {
            _repository = new ScheduledTaskRepository(_context);
        }

        [TestMethod]
        public void UnpauseScheduledTasksCommandHandler_Succeeds() {
            var command = new UnpauseScheduledTasksCommand();
            var handler = new UnpauseScheduledTasksCommandHandler(_repository);
            var task = Substitute.For<ScheduledTask>();

            task.IsPaused.Returns(true);

            _context.ScheduledTasks.Add(task);

            handler.Execute(command);

            task.Received().Unpause();
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UnpauseScheduledTasksCommandHandler_Only_Unpauses_Paused() {
            var command = new UnpauseScheduledTasksCommand();
            var handler = new UnpauseScheduledTasksCommandHandler(_repository);
            var task = Substitute.For<ScheduledTask>();

            task.IsPaused.Returns(false);

            _context.ScheduledTasks.Add(task);

            handler.Execute(command);

            task.DidNotReceive().Unpause();
        }
    }
}