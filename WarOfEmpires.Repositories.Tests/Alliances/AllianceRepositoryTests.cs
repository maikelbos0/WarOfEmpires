using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Alliances {
    [TestClass]
    public sealed class AllianceRepositoryTests {
        private readonly FakeWarContext _context = new FakeWarContext();

        [TestMethod]
        public void AllianceRepository_Get_Succeeds() {
            var repository = new AllianceRepository(_context);

            var alliance = repository.Get(1);

            alliance.Should().NotBeNull();
            alliance.Id.Should().Be(1);
        }

        [TestMethod]
        public void AllianceRepository_Get_Throws_Exception_For_Nonexistent_Id() {
            var repository = new AllianceRepository(_context);

            Action action = () => repository.Get(-1);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void AllianceRepository_Get_Throws_Exception_For_Not_Active() {
            var repository = new AllianceRepository(_context);

            Action action = () => repository.Get(2);

            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void AllianceRepository_Get_Does_Not_Save() {
            var repository = new AllianceRepository(_context);

            repository.Get(1);

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AllianceRepository_GetAll_Succeeds() {
            var repository = new AllianceRepository(_context);

            var Alliances = repository.GetAll();

            Alliances.Should().NotBeNull();
            Alliances.Should().HaveCount(1);
        }

        [TestMethod]
        public void AllianceRepository_GetAll_Does_Not_Save() {
            var repository = new AllianceRepository(_context);

            repository.GetAll();

            _context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AllianceRepository_Add_Succeeds() {
            var repository = new AllianceRepository(_context);
            var alliance = new Alliance(null, "ALLY", "The Alliance");

            repository.Add(alliance);

            _context.Alliances.Should().Contain(alliance);
        }

        [TestMethod]
        public void AllianceRepository_Add_Saves() {
            var repository = new AllianceRepository(_context);

            repository.Add(new Alliance(null, "ALLY", "The Alliance"));

            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AllianceRepository_Update_Saves() {
            var repository = new AllianceRepository(_context);

            repository.Update();

            _context.CallsToSaveChanges.Should().Be(1);
        }
    }
}