using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using WarOfEmpires.Domain.Events;
using WarOfEmpires.Repositories.Events;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Events {
    [TestClass]
    public sealed class ScheduledTaskRepositoryTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        [TestInitialize]
        public void Initialize() {
            var task1 = Substitute.For<ScheduledTask>();

            task1.Id.Returns(1);
            task1.EventType.Returns("Event1");
            task1.Interval.Returns(new TimeSpan(0, 5, 0));

            _context.ScheduledTasks.Add(task1);

            var task2 = Substitute.For<ScheduledTask>();

            task2.Id.Returns(1);
            task2.EventType.Returns("Event2");
            task2.Interval.Returns(new TimeSpan(0, 5, 0));

            _context.ScheduledTasks.Add(task2);

        }

        [TestMethod]
        public void ScheduledTaskRepository_GetAll_Succeeds() {
            var repository = new ScheduledTaskRepository(_context);

            var tasks = repository.GetAll();

            tasks.Should().NotBeNull();
            tasks.Should().HaveCount(2);
        }

        [TestMethod]
        public void ScheduledTaskRepository_GetAll_Does_Not_Save() {
            var repository = new ScheduledTaskRepository(_context);

            repository.GetAll();

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void ScheduledTaskRepository_Update_Saves() {
            var repository = new ScheduledTaskRepository(_context);

            repository.Update();

            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}