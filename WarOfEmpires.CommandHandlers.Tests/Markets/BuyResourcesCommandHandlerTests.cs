using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Formatting;

namespace WarOfEmpires.CommandHandlers.Tests.Markets {
    [TestClass]
    public sealed class BuyResourcesCommandHandlerTests {
        private readonly FakeWarContext _context = new FakeWarContext();
        private readonly EnumFormatter _formatter = new EnumFormatter();

        public BuyResourcesCommandHandlerTests() {
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Succeeds() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Allows_Empty_Values() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void BuyResourcesCommandHandler_Fails_For_Too_Little_Gold() {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Food() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "", "", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Food");
            result.Errors[0].Message.Should().Be("You don't have enough food available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Wood() {
            throw new System.NotImplementedException();

            /*
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "5", "5", "", "", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Wood");
            result.Errors[0].Message.Should().Be("You don't have enough wood available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Stone() {
            throw new System.NotImplementedException();
            
            /*
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "", "", "5", "5", "", "");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Stone");
            result.Errors[0].Message.Should().Be("You don't have enough stone available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Gives_Warning_For_Too_Little_Available_Ore() {
            throw new System.NotImplementedException();

            /*
            _player.CanAfford(Arg.Any<Resources>()).Returns(false);

            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "", "", "", "", "", "", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Ore");
            result.Errors[0].Message.Should().Be("You don't have enough ore available to sell that much");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Food() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "A", "5", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Food");
            result.Errors[0].Message.Should().Be("Food must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_Food() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "-1", "5", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Food");
            result.Errors[0].Message.Should().Be("Food must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_FoodPrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "A", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FoodPrice");
            result.Errors[0].Message.Should().Be("Food price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_FoodPrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "-1", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FoodPrice");
            result.Errors[0].Message.Should().Be("Food price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Food_Without_FoodPrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "0", "5", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.FoodPrice");
            result.Errors[0].Message.Should().Be("Food price is required when selling food");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Wood() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "A", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Wood");
            result.Errors[0].Message.Should().Be("Wood must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_Wood() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "-1", "5", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Wood");
            result.Errors[0].Message.Should().Be("Wood must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_WoodPrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "A", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodPrice");
            result.Errors[0].Message.Should().Be("Wood price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_WoodPrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "-1", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodPrice");
            result.Errors[0].Message.Should().Be("Wood price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Wood_Without_WoodPrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "0", "5", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.WoodPrice");
            result.Errors[0].Message.Should().Be("Wood price is required when selling wood");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Stone() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "A", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Stone");
            result.Errors[0].Message.Should().Be("Stone must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_Stone() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "-1", "5", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Stone");
            result.Errors[0].Message.Should().Be("Stone must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_StonePrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "A", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StonePrice");
            result.Errors[0].Message.Should().Be("Stone price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_StonePrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "-1", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StonePrice");
            result.Errors[0].Message.Should().Be("Stone price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Stone_Without_StonePrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "0", "5", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.StonePrice");
            result.Errors[0].Message.Should().Be("Stone price is required when selling stone");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_Ore() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "A", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Ore");
            result.Errors[0].Message.Should().Be("Ore must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_Ore() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "-1", "5");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.Ore");
            result.Errors[0].Message.Should().Be("Ore must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Alphanumeric_OrePrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "5", "A");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OrePrice");
            result.Errors[0].Message.Should().Be("Ore price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Negative_OrePrice() {
            throw new System.NotImplementedException();
            
            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "5", "-1");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OrePrice");
            result.Errors[0].Message.Should().Be("Ore price must be a valid number");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }

        [TestMethod]
        public void SellResourcesCommandHandler_Fails_For_Ore_Without_OrePrice() {
            throw new System.NotImplementedException();

            /*
            var handler = new SellResourcesCommandHandler(_repository, _formatter);
            var command = new SellResourcesCommand("test@test.com", "5", "5", "5", "5", "5", "5", "5", "0");

            var result = handler.Execute(command);

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Expression.ToString().Should().Be("c => c.OrePrice");
            result.Errors[0].Message.Should().Be("Ore price is required when selling ore");
            _context.CallsToSaveChanges.Should().Be(0);
            */
        }
    }
}