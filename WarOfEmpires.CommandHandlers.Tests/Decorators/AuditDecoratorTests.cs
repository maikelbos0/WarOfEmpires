using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands;
using WarOfEmpires.Repositories.Auditing;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Serialization;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using VDT.Core.DependencyInjection.Decorators;

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
            [Audit]
            public CommandResult<TestCommand> Execute(TestCommand command) {
                return new CommandResult<TestCommand>();
            }
        }

        [TestMethod]
        public void AuditDecorator_Succeeds() {
            var context = new FakeWarContext();
            var command = new TestCommand("Value");
            var commandHandler = new ServiceCollection()
                .AddScoped<ICommandExecutionRepository>(serviceProvider => new CommandExecutionRepository(context))
                .AddScoped<ISerializer, Serializer>()
                .AddScoped<AuditDecorator>()
                .AddScoped<ICommandHandler<TestCommand>, TestCommandHandler>(options => options.AddAttributeDecorators())
                .BuildServiceProvider()
                .GetRequiredService<ICommandHandler<TestCommand>>();

            commandHandler.Execute(command);

            context.CommandExecutions.Should().HaveCount(1);
            context.CommandExecutions.First().Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            context.CommandExecutions.First().CommandType.Should().Be("WarOfEmpires.CommandHandlers.Tests.Decorators.AuditDecoratorTests+TestCommand");
            context.CommandExecutions.First().CommandData.Should().Be("{\"Test\":\"Value\"}");
            context.CommandExecutions.First().ElapsedMilliseconds.Should().BeInRange(0, 1000);
            context.CallsToSaveChanges.Should().Be(1);
        }
    }
}