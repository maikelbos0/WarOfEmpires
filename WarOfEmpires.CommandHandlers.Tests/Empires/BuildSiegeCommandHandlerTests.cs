using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WarOfEmpires.CommandHandlers.Tests.Empires {
    [TestClass]
    public sealed class BuildSiegeCommandHandlerTests {
        [TestMethod]
        public void BuildSiegeCommandHandler_Succceeds() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Allows_Empty_Values() {
            throw new System.NotImplementedException();
        }

        [DataTestMethod]
        [DataRow(1, 0, 0, DisplayName = "FireArrows")]
        [DataRow(0, 1, 0, DisplayName = "BatteringRams")]
        [DataRow(0, 0, 1, DisplayName = "ScalingLadders")]
        public void BuildSiegeCommandHandler_Fails_For_Too_Little_Maintenance() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Alphanumeric_FireArrows() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Alphanumeric_BatteringRams() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Alphanumeric_ScalingLadders() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Negative_FireArrows() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Negative_BatteringRams() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void BuildSiegeCommandHandler_Fails_For_Negative_ScalingLadders() {
            throw new System.NotImplementedException();
        }

        [DataTestMethod]
        [DataRow(1, 0, 0, DisplayName = "FireArrows")]
        [DataRow(0, 1, 0, DisplayName = "BatteringRams")]
        [DataRow(0, 0, 1, DisplayName = "ScalingLadders")]
        public void BuildSiegeCommandHandler_Fails_For_Too_Little_Resources() {
            throw new System.NotImplementedException();
        }
    }
}