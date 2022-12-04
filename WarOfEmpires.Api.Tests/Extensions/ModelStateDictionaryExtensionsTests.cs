using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Api.Extensions;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;

namespace WarOfEmpires.Api.Tests.Extensions {
    [TestClass]
    public sealed class ModelStateDictionaryExtensionsTests {
        private sealed class TestCommand : ICommand {
            public Property Test { get; set; }
        }

        [TestMethod]
        public void ModelStateDictionaryExtensions_Merge_Merges_Property_Error() {
            var modelState = new ModelStateDictionary();
            var commandResult = new CommandResult<TestCommand>();

            commandResult.AddError(t => t.Test, "Test is wrong");
            modelState.Merge(commandResult);

            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey("Test").WhoseValue.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be("Test is wrong");
        }

        [TestMethod]
        public void ModelStateDictionaryExtensions_Merge_Merges_Model_Error() {
            var modelState = new ModelStateDictionary();
            var commandResult = new CommandResult<TestCommand>();

            commandResult.AddError("Everything is wrong");
            modelState.Merge(commandResult);

            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey("").WhoseValue.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be("Everything is wrong");
        }
    }
}
