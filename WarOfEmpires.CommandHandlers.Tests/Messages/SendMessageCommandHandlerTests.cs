using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Test.Utilities;

namespace WarOfEmpires.CommandHandlers.Tests.Messages {
    [TestClass]
    public sealed class SendMessageCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly PlayerRepository _repository;

        public SendMessageCommandHandlerTests() {
            _repository = new PlayerRepository(_context);
        }

        [TestMethod]
        public void SendMessageCommandHandler_Succeeds() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SendMessageCommandHandler_Fails_For_Alphanumeric_Recipient() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SendMessageCommandHandler_Fails_For_Nonexistent_Recipient() {
            throw new System.NotImplementedException();
        }
    }
}