using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using WarOfEmpires.CommandHandlers.Events;
using WarOfEmpires.Commands.Events;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Events {
    [TestClass]
    public sealed class RunScheduledTasksCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly ScheduledTaskRepository _repository;

        public RunScheduledTasksCommandHandlerTests() {
            _repository = new ScheduledTaskRepository(_context);
        }

        public ScheduledTask CreateTask(bool firstSuccess, params bool[] success) {
            var task = Substitute.For<ScheduledTask>();

            task.Execute().Returns(firstSuccess, success);
            _context.ScheduledTasks.Add(task);
            return task;
        }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Runs_All_Tasks() {
            var handler = new RunScheduledTasksCommandHandler(_repository);
            var command = new RunScheduledTasksCommand();
            var tasks = new[] {
                CreateTask(false),
                CreateTask(false),
                CreateTask(false)
            };

            handler.Execute(command);

            foreach (var task in tasks) {
                task.Received().Execute();
            }
        }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Runs_Tasks_Multiple_Times_If_Needed() {
            var handler = new RunScheduledTasksCommandHandler(_repository);
            var command = new RunScheduledTasksCommand();
            var task = CreateTask(true, true, false);

            handler.Execute(command);

            task.Received(3).Execute();
        }

        [TestMethod]
        public void RunScheduledTasksCommandHandler_Runs_Tasks_Once_If_Needed() {
            var handler = new RunScheduledTasksCommandHandler(_repository);
            var command = new RunScheduledTasksCommand();
            var task = CreateTask(false);

            handler.Execute(command);

            task.Received().Execute();
        }
    }
}