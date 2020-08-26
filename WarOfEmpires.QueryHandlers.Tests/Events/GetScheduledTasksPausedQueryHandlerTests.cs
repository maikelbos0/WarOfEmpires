using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.QueryHandlers.Events;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Events {
    [TestClass]
    public sealed class GetScheduledTasksPausedQueryHandlerTests {
        [TestMethod]
        public void GetScheduledTasksPausedQueryHandler_Returns_True_When_All_Paused() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, true)
                .WithScheduledTask(2, true);

            var query = new GetScheduledTasksPausedQuery();
            var handler = new GetScheduledTasksPausedQueryHandler(builder.Context);

            handler.Execute(query).Should().BeTrue();
        }

        [TestMethod]
        public void GetScheduledTasksPausedQueryHandler_Returns_False_When_None_Paused() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, false)
                .WithScheduledTask(2, false);

            var query = new GetScheduledTasksPausedQuery();
            var handler = new GetScheduledTasksPausedQueryHandler(builder.Context);

            handler.Execute(query).Should().BeFalse();
        }

        [TestMethod]
        public void GetScheduledTasksPausedQueryHandler_Returns_Null_When_No_Tasks_Exist() {
            var query = new GetScheduledTasksPausedQuery();
            var handler = new GetScheduledTasksPausedQueryHandler(new FakeWarContext());

            handler.Execute(query).Should().BeNull();
        }

        [TestMethod]
        public void GetScheduledTasksPausedQueryHandler_Returns_Null_When_Some_Paused() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, true)
                .WithScheduledTask(2, false);

            var query = new GetScheduledTasksPausedQuery();
            var handler = new GetScheduledTasksPausedQueryHandler(builder.Context);

            handler.Execute(query).Should().BeNull();
        }
    }
}