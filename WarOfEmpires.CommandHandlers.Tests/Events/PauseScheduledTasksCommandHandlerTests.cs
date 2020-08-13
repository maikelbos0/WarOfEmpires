using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Events;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Events {
    [TestClass]
    public sealed class PauseScheduledTasksCommandHandlerTests {
        [TestMethod]
        public void PauseScheduledTasksCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, out var task, false);

            var handler = new PauseScheduledTasksCommandHandler(new ScheduledTaskRepository(builder.Context));
            var command = new PauseScheduledTasksCommand();

            handler.Execute(command);

            task.Received().Pause();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void PauseScheduledTasksCommandHandler_Only_Pauses_Not_Paused() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, out var task, true);

            var handler = new PauseScheduledTasksCommandHandler(new ScheduledTaskRepository(builder.Context));
            var command = new PauseScheduledTasksCommand();

            handler.Execute(command);

            task.DidNotReceive().Pause();
        }
    }
}