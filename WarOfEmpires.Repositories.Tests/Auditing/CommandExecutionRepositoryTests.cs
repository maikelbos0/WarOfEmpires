using WarOfEmpires.Domain.Auditing;
using WarOfEmpires.Repositories.Auditing;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarOfEmpires.Repositories.Tests.Auditing {
    [TestClass]
    public class CommandExecutionRepositoryTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        [TestMethod]
        public void CommandExecutionRepository_Add_Succeeds() {
            var repository = new CommandExecutionRepository(_context);
            var execution = new CommandExecution("test", "test", 1.0);

            repository.Add(execution);

            _context.CommandExecutions.Should().Contain(execution);
        }

        [TestMethod]
        public void CommandExecutionRepository_Add_Saves() {
            var repository = new CommandExecutionRepository(_context);

            repository.Add(new CommandExecution("test", "test", 1.0));

            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}
