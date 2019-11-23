using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Utilities.Services;

namespace WarOfEmpires.Utilities.Tests.Services {
    [TestClass]
    public sealed class EmailComparisonServiceTests {
        [TestMethod]
        public void EmailComparisonService_Equals_Returns_True_For_Equal_Emails() {
            EmailComparisonService.Equals("test@test.com", "test@test.com");
        }

        [TestMethod]
        public void EmailComparisonService_Equals_Returns_False_For_Unequal_Emails() {
            EmailComparisonService.Equals("test@test.com", "different@test.com");
        }

        [TestMethod]
        public void EmailComparisonService_Equals_Is_Case_Insensitive() {
            EmailComparisonService.Equals("test@test.com", "TEST@TEST.COM");
        }

        [TestMethod]
        public void EmailComparisonService_Equals_Is_Accent_Sensitive() {
            EmailComparisonService.Equals("test@test.com", "tést@tëst.côm");
        }
    }
}