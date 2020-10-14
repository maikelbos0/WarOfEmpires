using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Web.Mvc;
using WarOfEmpires.ActionResults;
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
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnFailure_For_Command_Error() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Succesful_Execution() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void CommandResultBuilder_Returns_OnSuccess_For_Warnings() {
            throw new System.NotImplementedException();
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