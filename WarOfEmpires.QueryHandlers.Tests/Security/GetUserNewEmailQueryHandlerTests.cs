using WarOfEmpires.Queries.Security;
using WarOfEmpires.QueryHandlers.Security;
using WarOfEmpires.Test.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace WarOfEmpires.QueryHandlers.Tests.Security {
    [TestClass]
    public sealed class GetUserNewEmailQueryHandlerTests {
        [TestMethod]
        public void GetUserNewEmailQueryHandler_Gives_NewEmail_If_Available() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.User.NewEmail.Returns("new@test.com");

            var handler = new GetUserNewEmailQueryHandler(builder.Context);
            var query = new GetUserNewEmailQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().Be("new@test.com");
        }

        [TestMethod]
        public void GetUserNewEmailQueryHandler_Gives_Null_NewEmail_If_NewEmail_Is_Not_Available() {
            var builder = new FakeBuilder().BuildPlayer(1);

            builder.User.NewEmail.Returns((string)null);

            var handler = new GetUserNewEmailQueryHandler(builder.Context);
            var query = new GetUserNewEmailQuery("test1@test.com");

            var result = handler.Execute(query);

            result.Should().BeNull();
        }

        [TestMethod]
        public void GetUserNewEmailQueryHandler_Throws_Exception_For_Nonexistent_User() {
            var handler = new GetUserNewEmailQueryHandler(new FakeWarContext());
            var query = new GetUserNewEmailQuery("wrong@test.com");

            Action queryAction = () => handler.Execute(query);

            queryAction.Should().Throw<InvalidOperationException>();
        }
    }
}