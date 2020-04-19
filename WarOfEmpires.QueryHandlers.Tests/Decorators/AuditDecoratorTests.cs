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

        private readonly Serializer _serializer = new Serializer();
        private readonly FakeWarContext _context = new FakeWarContext();

        [TestMethod]
        public void AuditDecorator_Succeeds() {
            var queryHandler = new TestQueryHandler();
            var query = new TestQuery("Value");
            var decorator = new AuditDecorator<TestQuery, string>(_context, _serializer) {
                Handler = queryHandler
            };

            decorator.Execute(query);

            _context.QueryExecutions.Should().HaveCount(1);
            _context.QueryExecutions.First().Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            _context.QueryExecutions.First().QueryType.Should().Be("WarOfEmpires.QueryHandlers.Tests.Decorators.AuditDecoratorTests+TestQuery");
            _context.QueryExecutions.First().QueryData.Should().Be(_serializer.SerializeToJson(query));
            _context.QueryExecutions.First().ElapsedMilliseconds.Should().BeInRange(0, 1000);
            _context.CallsToSaveChanges.Should().Be(1);
        }

        [TestMethod]
        public void AuditDecorator_Calls_Query() {
            var queryHandler = new TestQueryHandler();
            var query = new TestQuery("Value");
            var decorator = new AuditDecorator<TestQuery, string>(_context, _serializer) {
                Handler = queryHandler
            };

            var result = decorator.Execute(query);

            result.Should().Be("Result");
        }
    }
}