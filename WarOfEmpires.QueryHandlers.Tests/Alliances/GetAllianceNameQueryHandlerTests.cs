using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using WarOfEmpires.Domain.Alliances;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Queries.Alliances;
using WarOfEmpires.QueryHandlers.Alliances;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.QueryHandlers.Tests.Alliances {
    [TestClass]
    public sealed class GetAllianceNameQueryHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly Alliance _alliance;

        public GetAllianceNameQueryHandlerTests() {
            _alliance = Substitute.For<Alliance>();

            _alliance.Id.Returns(1);
            _alliance.Code.Returns("FS");
            _alliance.Name.Returns("Føroyskir Samgonga");

            var user = Substitute.For<User>();
            var player = Substitute.For<Player>();

            user.Email.Returns("test@test.com");
            user.Status.Returns(UserStatus.Active);
            player.User.Returns(user);
            player.Alliance.Returns(_alliance);
            _alliance.Members.Returns(new List<Player>() { player });

            _context.Alliances.Add(_alliance);
            _context.Users.Add(user);
            _context.Players.Add(player);
        }

        [TestMethod]
        public void GetAllianceNameQueryHandler_Returns_Correct_Name() {
            var handler = new GetAllianceNameQueryHandler(_context);
            var query = new GetAllianceNameQuery("test@test.com");

            var result = handler.Execute(query);

            result.Should().Be("Føroyskir Samgonga");
        }
    }
}
