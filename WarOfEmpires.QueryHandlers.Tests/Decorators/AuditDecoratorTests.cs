using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using VDT.Core.DependencyInjection.Decorators;
using WarOfEmpires.Database;
using WarOfEmpires.Queries;
using WarOfEmpires.QueryHandlers.Decorators;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Serialization;

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
            [Audit]
            public string Execute(TestQuery query) {
                return "Result";
            }
        }

        [TestMethod]
        public void AuditDecorator_Succeeds() {
            var context = new FakeWarContext();
            var query = new TestQuery("Value");
            var queryHandler = new ServiceCollection()
                .AddSingleton<IWarContext>(context)
                .AddScoped<ISerializer, Serializer>()
                .AddScoped<AuditDecorator>()
                .AddScoped<IQueryHandler<TestQuery, string>, TestQueryHandler>(options => options.AddAttributeDecorators())
                .BuildServiceProvider()
                .GetRequiredService<IQueryHandler<TestQuery, string>>();

            queryHandler.Execute(query);

            context.QueryExecutions.Should().HaveCount(1);
            context.QueryExecutions.First().Date.Should().BeCloseTo(DateTime.UtcNow, 1000);
            context.QueryExecutions.First().QueryType.Should().Be("WarOfEmpires.QueryHandlers.Tests.Decorators.AuditDecoratorTests+TestQuery");
            context.QueryExecutions.First().QueryData.Should().Be("{\"Test\":\"Value\"}");
            context.QueryExecutions.First().ElapsedMilliseconds.Should().BeInRange(0, 1000);
            context.CallsToSaveChanges.Should().Be(1);
        }
    }
}