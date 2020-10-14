using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using System.Web.Mvc;
using WarOfEmpires.ActionResults;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Extensions;
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
            var controller = Substitute.For<Controller>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, new TestCommand("test"))
                .OnFailure(() => null);

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>("Missing on success result handler");
        }

        [TestMethod]
        public void CommandResultBuilder_Requires_OnFailure() {
            var messageService = Substitute.For<IMessageService>();
            var controller = Substitute.For<Controller>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, new TestCommand("test"))
                .OnSuccess(() => null);

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>("Missing on failure result handler");
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnFailure_For_ModelState_Error() {
            var messageService = Substitute.For<IMessageService>();
            var controller = Substitute.For<Controller>();
            controller.ModelState.AddModelError("Test", "An error occurred");
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, new TestCommand("test"))
                .OnFailure("Failure")
                .OnSuccess("Success");

            var result = builder.Execute();

            result.ViewName.Should().Be("Failure");
            messageService.DidNotReceiveWithAnyArgs().Dispatch(Arg.Any<ICommand>());
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnFailure_For_Command_Error() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            commandResult.AddError(c => c.Test, "An error occurred");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);
            var controller = Substitute.For<Controller>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, command)
                .OnFailure("Failure")
                .OnSuccess("Success");

            var result = builder.Execute();

            result.ViewName.Should().Be("Failure");
            controller.ModelState.Should().ContainKey("");
            controller.ModelState[""].Errors.Should().Contain(e => e.ErrorMessage == "An error occurred");
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Succesful_Execution() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);
            var controller = Substitute.For<Controller>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, command)
                .OnFailure("Failure")
                .OnSuccess("Success");

            var result = builder.Execute();

            result.ViewName.Should().Be("Success");
            //controller.Response.Headers.Cast<string>().Should().Contain("X-Valid");
            //controller.Response.Headers["X-Valid"].Should().Be("true");
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Warnings() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            commandResult.AddWarning("Be aware");
            commandResult.AddWarning("Second warning");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);
            var controller = Substitute.For<Controller>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, command)
                .OnFailure("Failure")
                .OnSuccess("Success");

            var result = builder.Execute();

            result.ViewName.Should().Be("Success");
            //controller.Response.Headers.Cast<string>().Should().Contain("X-Warnings");
            //controller.Response.Headers["X-Valid"].Should().Be("Be aware|Second warning");
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_Clears_ModelState_For_Success() {
            throw new System.NotImplementedException();
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