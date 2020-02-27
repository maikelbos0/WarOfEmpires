using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarOfEmpires.Domain.Empires;

namespace WarOfEmpires.Domain.Tests.Empires {
    [TestClass]
    public sealed class WorkerDefinitionFactoryTests {
        [TestMethod]
        public void WorkerDefinitionFactory_Has_Definitions_For_All_WorkerTypes() {
            foreach (WorkerType type in Enum.GetValues(typeof(WorkerType))) {
                var worker = WorkerDefinitionFactory.Get(type);

                worker.Type.Should().Be(type);
            }
        }
    }
}