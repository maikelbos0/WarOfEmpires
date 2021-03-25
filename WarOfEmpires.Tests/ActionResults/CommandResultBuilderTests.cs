using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
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

        private ViewResult View(string viewName, object model) {
            return new ViewResult() { 
                ViewName = viewName,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) {  
                    Model = model
                }
            };
        }

        [TestMethod]
        public void CommandResultBuilder_Requires_OnSuccess() {
            var messageService = Substitute.For<IMessageService>();
            var controller = Substitute.For<IBaseController>();
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, View, modelState, new TestCommand("test"))
                .OnFailure("Failure");

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>().WithMessage("Missing on success result handler");
        }

        [TestMethod]
        public void CommandResultBuilder_Requires_OnFailure() {
            var messageService = Substitute.For<IMessageService>();
            var controller = Substitute.For<IBaseController>();
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, View, modelState, new TestCommand("test"))
                .OnSuccess("Success");

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>().WithMessage("Missing on failure result handler");
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnFailure_For_ModelState_Error() {
            var messageService = Substitute.For<IMessageService>();
            var controller = Substitute.For<IBaseController>();
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, View, modelState, new TestCommand("test"))
                .OnFailure("Failure", "test")
                .OnSuccess("Success");
            modelState.AddModelError("Test", "An error occurred");

            var result = builder.Execute();

            result.ViewName.Should().Be("Failure");
            result.Model.Should().Be("test");
            messageService.DidNotReceiveWithAnyArgs().Dispatch(Arg.Any<ICommand>());
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnFailure_For_Command_Error() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var controller = Substitute.For<IBaseController>();
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, View, modelState, command)
                .OnFailure("Failure", "test")
                .OnSuccess("Success");
            commandResult.AddError(c => c.Test, "An error occurred");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            var result = builder.Execute();

            result.ViewName.Should().Be("Failure");
            result.Model.Should().Be("test");
            modelState.Should().Contain(m => m.Key == "Test" && m.Value.Errors.Any(e => e.ErrorMessage == "An error occurred"));
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Succesful_Execution() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var controller = Substitute.For<IBaseController>();
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, View, modelState, command)
                .OnFailure("Failure")
                .OnSuccess("Success");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            var result = builder.Execute();

            result.ViewName.Should().Be("Success");
            controller.Received().AddResponseHeader("X-IsValid", "true");
            modelState.Should().BeEmpty();
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Warnings() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var controller = Substitute.For<IBaseController>();
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, View, modelState, command)
                .OnFailure("Failure")
                .OnSuccess("Success");
            commandResult.AddWarning("Be aware");
            commandResult.AddWarning("Second warning");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            var result = builder.Execute();

            result.ViewName.Should().Be("Success");
            controller.Received().AddResponseHeader("X-Warnings", "Be aware|Second warning");
            modelState.Should().BeEmpty();
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_ThrowOnFailure_Works() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var controller = Substitute.For<IBaseController>();
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, controller, View, modelState, command)
                .ThrowOnFailure()
                .OnSuccess("Success");
            commandResult.AddError(c => c.Test, "An error occurred");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>().WithMessage("Unexpected error executing WarOfEmpires.Tests.ActionResults.CommandResultBuilderTests+TestCommand: {\"Test\":\"test\"}");
        }
    }
}