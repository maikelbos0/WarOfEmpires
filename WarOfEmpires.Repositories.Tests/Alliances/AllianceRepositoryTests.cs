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
    }
}