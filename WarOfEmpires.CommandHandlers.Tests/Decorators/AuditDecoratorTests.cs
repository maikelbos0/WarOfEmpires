using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands;
using WarOfEmpires.Repositories.Auditing;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Serialization;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace WarOfEmpires.CommandHandlers.Tests.Decorators {
    [TestClass]
    public sealed class AuditDecoratorTests {
        public sealed class TestCommand : ICommand {
            public string Test { get; private set; }

            public TestCommand(string test) {
                Test = test;
            }
        }

        public sealed class TestCommandHandler : ICommandHandler<TestCommand> {
            public CommandResult<TestCommand> Execute(TestCommand command) {
                var result = new CommandResult<TestCommand>();

                result.AddError("Error success");

                return result;
            }
        }

        [TestMethod]
        public void AuditDecorator_Succeeds() {
            var context = new FakeWarContext();
            var serializer = new Serializer();

            var commandHandler = new TestCommandHandler();
            var command = new TestCommand("Value");
            var decorator = new AuditDecorator<TestCommand>(new CommandExecutionRepository(context), new Serializer()) {
                Handler = commandHandler
            };

            decorator.Execute(command);

            context.CommandExecutions.Should().HaveCount(1);
            context.CommandExecutions.First().Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            context.CommandExecutions.First().CommandType.Should().Be("WarOfEmpires.CommandHandlers.Tests.Decorators.AuditDecoratorTests+TestCommand");
            context.CommandExecutions.First().CommandData.Should().Be(serializer.SerializeToJson(command));
            context.CommandExecutions.First().ElapsedMilliseconds.Should().BeInRange(0, 1000);
            context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AuditDecorator_Calls_Command() {
            var commandHandler = new TestCommandHandler();
            var command = new TestCommand("Value");
            var decorator = new AuditDecorator<TestCommand>(new CommandExecutionRepository(new FakeWarContext()), new Serializer()) {
                Handler = commandHandler
            };

            var result = decorator.Execute(command);

            result.Should().HaveError("Error success");
        }
    }
}