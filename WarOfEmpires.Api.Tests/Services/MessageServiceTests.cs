using WarOfEmpires.Api.Services;
using WarOfEmpires.Commands;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Queries;
using WarOfEmpires.QueryHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using NSubstitute;
using System;

namespace WarOfEmpires.Api.Tests.Services {
    [TestClass]
    public sealed class MessageServiceTests {
        public sealed class TestCommand : ICommand {
        }

        public sealed class TestCommandHandler : ICommandHandler<TestCommand> {
            public CommandResult<TestCommand> Execute(TestCommand command) {
                return new CommandResult<TestCommand>();
            }
        }

        public sealed class TestQuery : IQuery<bool> {
        }

        public sealed class TestQueryHandler : IQueryHandler<TestQuery, bool> {
            public bool Execute(TestQuery query) {
                return true;
            }
        }

        [TestMethod]
        public void MessageService_Dispatches_Command_To_CommandHandler() {
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ICommandHandler<TestCommand>)).Returns(new TestCommandHandler());
            var messageService = new MessageService(serviceProvider);
            var command = new TestCommand();

            var result = messageService.Dispatch(command);

            result.Success.Should().BeTrue();
        }

        [TestMethod]
        public void MessageService_Dispatches_Query_To_QueryHandler() {
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IQueryHandler<TestQuery, bool>)).Returns(new TestQueryHandler());
            var messageService = new MessageService(serviceProvider);
            var query = new TestQuery();

            var result = messageService.Dispatch(query);

            result.Should().BeTrue();
        }
    }
}