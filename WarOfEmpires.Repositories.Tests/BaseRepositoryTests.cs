using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.Repositories.Tests {
    [TestClass]
    public sealed class BaseRepositoryTests {
        [TestMethod]
        public void BaseRepository_SaveChanges_Saves() {
            var context = new FakeWarContext();

            var repository = new BaseRepository(context);

            repository.SaveChanges();

            context.CallsToSaveChanges.Should().Be(1);
        }
    }
}