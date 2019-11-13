using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Queries.Events;
using WarOfEmpires.QueryHandlers.Events;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Events {
    [TestClass]
    public sealed class GetScheduledTasksPausedQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        public void AddTasks(params bool[] arePaused) {
            foreach (var isPaused in arePaused) {
                var task = Substitute.For<ScheduledTask>();

                task.IsPaused.Returns(isPaused);
                _context.ScheduledTasks.Add(task);
            }
        }

        [TestMethod]
        public void GetScheduledTasksPausedQueryHandler_Returns_True_When_All_Paused() {
            var query = new GetScheduledTasksPausedQuery();
            var handler = new GetScheduledTasksPausedQueryHandler(_context);

            AddTasks(true, true);

            handler.Execute(query).Should().BeTrue();
        }

        [TestMethod]
        public void GetScheduledTasksPausedQueryHandler_Returns_False_When_None_Paused() {
            var query = new GetScheduledTasksPausedQuery();
            var handler = new GetScheduledTasksPausedQueryHandler(_context);

            AddTasks(false, false);

            handler.Execute(query).Should().BeFalse();
        }

        [TestMethod]
        public void GetScheduledTasksPausedQueryHandler_Returns_Null_When_No_Tasks_Exist() {
            var query = new GetScheduledTasksPausedQuery();
            var handler = new GetScheduledTasksPausedQueryHandler(_context);

            handler.Execute(query).Should().BeNull();
        }

        [TestMethod]
        public void GetScheduledTasksPausedQueryHandler_Returns_Null_When_Some_Paused() {
            var query = new GetScheduledTasksPausedQuery();
            var handler = new GetScheduledTasksPausedQueryHandler(_context);

            AddTasks(true, false);

            handler.Execute(query).Should().BeNull();
        }
    }
}