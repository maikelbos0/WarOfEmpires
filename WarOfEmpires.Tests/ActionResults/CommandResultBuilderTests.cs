using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Web.Mvc;
using WarOfEmpires.ActionResults;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Controllers;
using WarOfEmpires.Services;

namespace WarOfEmpires.Tests.ActionResults {
    [TestClass]
    public sealed class CommandResultBuilderTests {
        public class TestCommand : ICommand {
            public string Test { get; }

            public TestCommand(string test) {
                Test = test;
            }
        }

        [TestMethod]
        public void CommandResultBuilder_Requires_OnSuccess() {
            var messageService = Substitute.For<IMessageService>();
            var controller = Substitute.For<IBaseController>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, new TestCommand("test"))
                .OnFailure(() => null);

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>("Missing on success result handler");
        }

        [TestMethod]
        public void CommandResultBuilder_Requires_OnFailure() {
            var messageService = Substitute.For<IMessageService>();
            var controller = Substitute.For<IBaseController>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, new TestCommand("test"))
                .OnSuccess(() => null);

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>("Missing on failure result handler");
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnFailure_For_ModelState_Error() {
            var messageService = Substitute.For<IMessageService>();
            var controller = Substitute.For<IBaseController>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, new TestCommand("test"))
                .OnFailure("Failure")
                .OnSuccess("Success");
            controller.IsModelStateValid().Returns(false);

            var result = builder.Execute();

            result.ViewName.Should().Be("Failure");
            messageService.DidNotReceiveWithAnyArgs().Dispatch(Arg.Any<ICommand>());
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnFailure_For_Command_Error() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var controller = Substitute.For<IBaseController>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, command)
                .OnFailure("Failure")
                .OnSuccess("Success");
            controller.IsModelStateValid().Returns(true, false);
            commandResult.AddError(c => c.Test, "An error occurred");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            var result = builder.Execute();

            result.ViewName.Should().Be("Failure");
            controller.Received().MergeModelState(commandResult);
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Succesful_Execution() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var controller = Substitute.For<IBaseController>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, command)
                .OnFailure("Failure")
                .OnSuccess("Success");
            controller.IsModelStateValid().Returns(true);
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            var result = builder.Execute();

            result.ViewName.Should().Be("Success");
            controller.Received().ClearModelState();
            controller.Received().AddResponseHeader("X-IsValid", "true");
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Warnings() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var controller = Substitute.For<IBaseController>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, command)
                .OnFailure("Failure")
                .OnSuccess("Success");
            controller.IsModelStateValid().Returns(true);
            commandResult.AddWarning("Be aware");
            commandResult.AddWarning("Second warning");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            var result = builder.Execute();

            result.ViewName.Should().Be("Success");
            controller.Received().ClearModelState();
            controller.Received().AddResponseHeader("X-Warnings", "Be aware|Second warning");
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_OnSuccess_View_Works() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void CommandResultBuilder_OnFailure_View_Works() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void CommandResultBuilder_OnFailure_View_Model_Works() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void CommandResultBuilder_OnFailure_Model_Works() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void CommandResultBuilder_ThrowOnFailure_Works() {
            throw new System.NotImplementedException();
        }
    }
}