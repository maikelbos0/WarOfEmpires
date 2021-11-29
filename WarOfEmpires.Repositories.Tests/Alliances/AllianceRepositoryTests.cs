using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Repositories.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests.Alliances {
    [TestClass]
    public sealed class AllianceRepositoryTests {
        [TestMethod]
        public void AllianceRepository_Get_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var repository = new AllianceRepository(builder.Context);

            var alliance = repository.Get("test1@test.com");

            alliance.Should().NotBeNull();
            alliance.Id.Should().Be(1);
        }

        [TestMethod]
        public void AllianceRepository_Get_Does_Not_Save() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var repository = new AllianceRepository(builder.Context);

            repository.Get("test1@test.com");

            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AllianceRepository_Get_By_Id_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1);

            var repository = new AllianceRepository(builder.Context);

            var alliance = repository.Get(1);

            alliance.Should().NotBeNull();
            alliance.Id.Should().Be(1);
        }

        [TestMethod]
        public void AllianceRepository_Get_By_Id_Does_Not_Save() {
            var builder = new FakeBuilder()
                .BuildAlliance(1);

            var repository = new AllianceRepository(builder.Context);

            repository.Get(1);

            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AllianceRepository_GetAll_Succeeds() {
            var builder = new FakeBuilder()
                .WithAlliance(1)
                .WithAlliance(2);

            var repository = new AllianceRepository(builder.Context);

            var alliances = repository.GetAll();

            alliances.Should().NotBeNull();
            alliances.Should().HaveCount(2);
        }

        [TestMethod]
        public void AllianceRepository_GetAll_Does_Not_Save() {
            var builder = new FakeBuilder()
                .WithAlliance(1);

            var repository = new AllianceRepository(builder.Context);

            repository.GetAll();

            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [TestMethod]
        public void AllianceRepository_Add_Succeeds() {
            var context = new FakeWarContext();

            var repository = new AllianceRepository(context);
            var alliance = new Alliance(null, "ALLY", "The Alliance");

            repository.Add(alliance);

            context.Alliances.Should().Contain(alliance);
        }

        [TestMethod]
        public void AllianceRepository_Add_Saves() {
            var context = new FakeWarContext();

            var repository = new AllianceRepository(context);

            repository.Add(new Alliance(null, "ALLY", "The Alliance"));

            context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AllianceRepository_Remove_Succeeds() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var repository = new AllianceRepository(builder.Context);

            repository.Remove(builder.Alliance);

            builder.Context.Alliances.Should().NotContain(builder.Alliance);
        }

        [TestMethod]
        public void AllianceRepository_Remove_Saves() {
            var builder = new FakeBuilder()
                .BuildAlliance(1)
                .WithMember(1);

            var repository = new AllianceRepository(builder.Context);

            repository.Remove(builder.Alliance);

            builder.Context.CallsToSaveChanges.Should().Be(1);
        }
    }
}