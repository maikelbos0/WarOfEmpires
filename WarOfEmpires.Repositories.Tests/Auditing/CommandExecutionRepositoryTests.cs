using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Repositories.Auditing;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarOfEmpires.Repositories.Tests.Auditing {
    [TestClass]
    public class CommandExecutionRepositoryTests {
        [TestMethod]
        public void CommandExecutionRepository_Add_Succeeds() {
            var context = new FakeWarContext();

            var repository = new CommandExecutionRepository(context);
            var execution = new CommandExecution("test", "test", 1.0);

            repository.Add(execution);

            context.CommandExecutions.Should().Contain(execution);
        }

        [TestMethod]
        public void CommandExecutionRepository_Add_Saves() {
            var context = new FakeWarContext();

            var repository = new CommandExecutionRepository(context);

            repository.Add(new CommandExecution("test", "test", 1.0));

            context.CallsToSaveChanges.Should().Be(1);
        }
    }
}
