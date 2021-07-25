using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Domain.Security;

namespace WarOfEmpires.CommandHandlers.Tests.Security {
    [TestClass]
    public sealed class UpdateUserDetailsCommandHandlerTests {
        [TestMethod]
        public void UpdateUserDetailsCommandHandler_Succeeds() {
            throw new System.NotImplementedException();
        }

        [DataTestMethod]
        [DataRow("New", UserStatus.New, DisplayName = "New")]
        [DataRow("Active", UserStatus.Active, DisplayName = "Active")]
        [DataRow("Inactive", UserStatus.Inactive, DisplayName = "Inactive")]
        public void UpdateUserDetailsCommandHandler_Resolves_Status_To_Correct_Status(string status, UserStatus expectedStatus) {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void UpdateUserDetailsCommandHandler_Fails_For_Existing_Email() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void UpdateUserDetailsCommandHandler_Throws_Exception_For_Invalid_User() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void UpdateUserDetailsCommandHandler_Throws_Exception_For_Nonexistent_Type() {
            throw new System.NotImplementedException();
        }
    }
}
