using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Events;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Events {
    [TestClass]
    public sealed class UnpauseScheduledTasksCommandHandlerTests {
        [TestMethod]
        public void UnpauseScheduledTasksCommandHandler_Succeeds() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, out var task, true);

            var handler = new UnpauseScheduledTasksCommandHandler(new ScheduledTaskRepository(builder.Context));
            var command = new UnpauseScheduledTasksCommand();
            
            handler.Execute(command);

            task.Received().Unpause();
            builder.Context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void UnpauseScheduledTasksCommandHandler_Only_Unpauses_Paused() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, out var task, false);

            var handler = new UnpauseScheduledTasksCommandHandler(new ScheduledTaskRepository(builder.Context));
            var command = new UnpauseScheduledTasksCommand();

            handler.Execute(command);

            task.DidNotReceive().Unpause();
        }
    }
}