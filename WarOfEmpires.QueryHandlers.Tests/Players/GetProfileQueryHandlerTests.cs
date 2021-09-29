using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.QueryHandlers.Players;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.QueryHandlers.Tests.Players {
    [TestClass]
    public sealed class GetProfileQueryHandlerTests {
        [TestMethod]
        public void GetPlayerDetailsQueryHandler_Returns_Correct_Information() {
            var builder = new FakeBuilder()
                .BuildPlayer(1)
                .WithProfile(1, "A name", "Description", "1.jpeg");
            var appSettings = new AppSettings() {
                UserImageBaseUrl = "https://localhost/user"
            };

            var handler = new GetProfileQueryHandler(builder.Context, appSettings);
            var query = new GetProfileQuery("test1@test.com");

            var result = handler.Execute(query);

            result.FullName.Should().Be("A name");
            result.Description.Should().Be("Description");
            result.AvatarLocation.Should().Be("https://localhost/user/1.jpeg");
        }
    }
}
