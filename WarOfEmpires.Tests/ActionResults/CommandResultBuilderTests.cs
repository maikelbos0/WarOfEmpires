using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using WarOfEmpires.ActionResults;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
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
            var modelState = new ModelStateDictionary();
            var urlHelper = Substitute.For<IUrlHelper>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, View, modelState, urlHelper, false, new TestCommand("test"))
                .OnFailure("Failure");

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>().WithMessage("Missing on success result handler");
        }

        [TestMethod]
        public void CommandResultBuilder_Requires_OnFailure() {
            var messageService = Substitute.For<IMessageService>();
            var modelState = new ModelStateDictionary();
            var urlHelper = Substitute.For<IUrlHelper>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, View, modelState, urlHelper, false, new TestCommand("test"))
                .OnSuccess("Success");

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>().WithMessage("Missing on failure result handler");
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnFailure_For_ModelState_Error() {
            var messageService = Substitute.For<IMessageService>();
            var modelState = new ModelStateDictionary();
            var urlHelper = Substitute.For<IUrlHelper>();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, View, modelState, urlHelper, false, new TestCommand("test"))
                .OnFailure("Failure", "test")
                .OnSuccess("Success");
            modelState.AddModelError("Test", "An error occurred");

            var result = builder.Execute();

            result.Should().BeOfType<ViewResult>();
            ((ViewResult)result).ViewName.Should().Be("Failure");
            ((ViewResult)result).Model.Should().Be("test");
            messageService.DidNotReceiveWithAnyArgs().Dispatch(Arg.Any<ICommand>());
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnFailure_For_Command_Error() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var urlHelper = Substitute.For<IUrlHelper>();
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, View, modelState, urlHelper, false, command)
                .OnFailure("Failure", "test")
                .OnSuccess("Success");
            commandResult.AddError(c => c.Test, "An error occurred");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            var result = builder.Execute();

            result.Should().BeOfType<ViewResult>();
            ((ViewResult)result).ViewName.Should().Be("Failure");
            ((ViewResult)result).Model.Should().Be("test");
            modelState.Should().Contain(m => m.Key == "Test" && m.Value.Errors.Any(e => e.ErrorMessage == "An error occurred"));
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Succesful_Execution_Of_Ajax_Request() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<UrlActionContext>()).Returns("Success");
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, View, modelState, urlHelper, true, command)
                .OnFailure("Failure")
                .OnSuccess("Success");
            commandResult.AddWarning("Be aware");
            commandResult.AddWarning("Second warning");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            var result = builder.Execute();

            result.Should().BeOfType<JsonResult>();
            ((JsonResult)result).Value.Should().BeEquivalentTo(new {
                Success = true,
                commandResult.Warnings,
                RedirectUrl = "Success"
            });
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Succesful_Execution() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var urlHelper = Substitute.For<IUrlHelper>();
            urlHelper.Action(Arg.Any<UrlActionContext>()).Returns("Success");
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, View, modelState, urlHelper, false, command)
                .OnFailure("Failure")
                .OnSuccess("Success");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            var result = builder.Execute();

            result.Should().BeOfType<RedirectResult>();
            ((RedirectResult)result).Url.Should().Be("Success");

            modelState.Should().BeEmpty();
            messageService.Received().Dispatch(command);
        }

        [TestMethod]
        public void CommandResultBuilder_ThrowOnFailure_Works() {
            var command = new TestCommand("test");
            var messageService = Substitute.For<IMessageService>();
            var commandResult = new CommandResult<TestCommand>();
            var urlHelper = Substitute.For<IUrlHelper>();
            var modelState = new ModelStateDictionary();
            var builder = new CommandResultBuilder<TestCommand, ViewResult>(messageService, View, modelState, urlHelper, false, command)
                .ThrowOnFailure()
                .OnSuccess("Success");
            commandResult.AddError(c => c.Test, "An error occurred");
            messageService.Dispatch(Arg.Any<TestCommand>()).Returns(commandResult);

            Action action = () => builder.Execute();

            action.Should().Throw<InvalidOperationException>().WithMessage("Unexpected error executing WarOfEmpires.Tests.ActionResults.CommandResultBuilderTests+TestCommand");
        }
    }
}