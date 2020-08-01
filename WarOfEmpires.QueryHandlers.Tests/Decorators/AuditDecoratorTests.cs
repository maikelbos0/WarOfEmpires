using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Queries;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Serialization;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace WarOfEmpires.QueryHandlers.Tests.Decorators {
    [TestClass]
    public sealed class AuditDecoratorTests {
        public sealed class TestQuery : IQuery<string> {
            public string Test { get; private set; }

            public TestQuery(string test) {
                Test = test;
            }
        }

        public sealed class TestQueryHandler : IQueryHandler<TestQuery, string> {
            public string Execute(TestQuery query) {
                return "Result";
            }
        }

        [TestMethod]
        public void AuditDecorator_Succeeds() {
            var context = new FakeWarContext();

            var queryHandler = new TestQueryHandler();
            var query = new TestQuery("Value");
            var decorator = new AuditDecorator<TestQuery, string>(context, new Serializer()) {
                Handler = queryHandler
            };

            decorator.Execute(query);

            context.QueryExecutions.Should().HaveCount(1);
            context.QueryExecutions.First().Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            context.QueryExecutions.First().QueryType.Should().Be("WarOfEmpires.QueryHandlers.Tests.Decorators.AuditDecoratorTests+TestQuery");
            context.QueryExecutions.First().QueryData.Should().Be("{\"Test\":\"Value\"}");
            context.QueryExecutions.First().ElapsedMilliseconds.Should().BeInRange(0, 1000);
            context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AuditDecorator_Calls_Query() {
            var queryHandler = new TestQueryHandler();
            var query = new TestQuery("Value");
            var decorator = new AuditDecorator<TestQuery, string>(new FakeWarContext(), new Serializer()) {
                Handler = queryHandler
            };

            var result = decorator.Execute(query);

            result.Should().Be("Result");
        }
    }
}