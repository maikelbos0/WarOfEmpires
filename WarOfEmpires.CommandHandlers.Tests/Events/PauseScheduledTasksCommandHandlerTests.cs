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
    public sealed class PauseScheduledTasksCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ScheduledTaskRepository _repository;

        public PauseScheduledTasksCommandHandlerTests() {
            _repository = new ScheduledTaskRepository(_context);
        }

        [TestMethod]
        public void PauseScheduledTasksCommandHandler_Succeeds() {
            var command = new PauseScheduledTasksCommand();
            var handler = new PauseScheduledTasksCommandHandler(_repository);
            var task = Substitute.For<ScheduledTask>();

            task.IsPaused.Returns(false);

            _context.ScheduledTasks.Add(task);

            handler.Execute(command);

            task.Received().Pause();
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void PauseScheduledTasksCommandHandler_Only_Pauses_Not_Paused() {
            var command = new PauseScheduledTasksCommand();
            var handler = new PauseScheduledTasksCommandHandler(_repository);
            var task = Substitute.For<ScheduledTask>();

            task.IsPaused.Returns(true);

            _context.ScheduledTasks.Add(task);

            handler.Execute(command);

            task.DidNotReceive().Pause();
        }
    }
}