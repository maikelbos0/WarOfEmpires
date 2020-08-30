using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Events {
    [TestClass]
    public sealed class ScheduledTaskRepositoryTests {
        [TestMethod]
        public void ScheduledTaskRepository_GetAll_Succeeds() {
            var builder = new FakeBuilder()
                .WithScheduledTask(1, false)
                .WithScheduledTask(2, false);

            var repository = new ScheduledTaskRepository(builder.Context);

            var tasks = repository.GetAll();

            tasks.Should().NotBeNull();
            tasks.Should().HaveCount(2);
        }

        [TestMethod]
        public void ScheduledTaskRepository_GetAll_Does_Not_Save() {
            var context = new FakeWarContext();

            var repository = new ScheduledTaskRepository(context);

            repository.GetAll();

            context.CallsToSaveChanges.Should().Be(0);
        }
    }
}