using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.IO;
using System.Linq;
using WarOfEmpires.CommandHandlers.Players;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Test.Utilities;
using WarOfEmpires.Utilities.Storage;

namespace WarOfEmpires.CommandHandlers.Tests.Players {
    [TestClass]
    public sealed class CreatePlayerCommandHandlerTests {
        [TestMethod]
        public void CreatePlayerCommandHandler_Succeeds_Succeeds_For_Jpeg() {
            var file = new byte[] { 255, 216, 255, 224, 0, 16, 74, 70, 73, 70, 0, 1, 1, 1, 0, 96, 0, 96, 0, 0, 255, 225, 0, 104, 69, 120, 105, 102, 0, 0, 77, 77, 0, 42, 0, 0, 0, 8, 0, 4, 1, 26, 0, 5, 0, 0, 0, 1, 0, 0, 0, 62, 1, 27, 0, 5, 0, 0, 0, 1, 0, 0, 0, 70, 1, 40, 0, 3, 0, 0, 0, 1, 0, 2, 0, 0, 1, 49, 0, 2, 0, 0, 0, 17, 0, 0, 0, 78, 0, 0, 0, 0, 0, 0, 0, 96, 0, 0, 0, 1, 0, 0, 0, 96, 0, 0, 0, 1, 112, 97, 105, 110, 116, 46, 110, 101, 116, 32, 52, 46, 50, 46, 49, 51, 0, 0, 255, 219, 0, 67, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 255, 219, 0, 67, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 255, 192, 0, 17, 8, 0, 1, 0, 1, 3, 1, 33, 0, 2, 17, 1, 3, 17, 1, 255, 196, 0, 31, 0, 0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 255, 196, 0, 181, 16, 0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 125, 1, 2, 3, 0, 4, 17, 5, 18, 33, 49, 65, 6, 19, 81, 97, 7, 34, 113, 20, 50, 129, 145, 161, 8, 35, 66, 177, 193, 21, 82, 209, 240, 36, 51, 98, 114, 130, 9, 10, 22, 23, 24, 25, 26, 37, 38, 39, 40, 41, 42, 52, 53, 54, 55, 56, 57, 58, 67, 68, 69, 70, 71, 72, 73, 74, 83, 84, 85, 86, 87, 88, 89, 90, 99, 100, 101, 102, 103, 104, 105, 106, 115, 116, 117, 118, 119, 120, 121, 122, 131, 132, 133, 134, 135, 136, 137, 138, 146, 147, 148, 149, 150, 151, 152, 153, 154, 162, 163, 164, 165, 166, 167, 168, 169, 170, 178, 179, 180, 181, 182, 183, 184, 185, 186, 194, 195, 196, 197, 198, 199, 200, 201, 202, 210, 211, 212, 213, 214, 215, 216, 217, 218, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250, 255, 196, 0, 31, 1, 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 255, 196, 0, 181, 17, 0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 119, 0, 1, 2, 3, 17, 4, 5, 33, 49, 6, 18, 65, 81, 7, 97, 113, 19, 34, 50, 129, 8, 20, 66, 145, 161, 177, 193, 9, 35, 51, 82, 240, 21, 98, 114, 209, 10, 22, 36, 52, 225, 37, 241, 23, 24, 25, 26, 38, 39, 40, 41, 42, 53, 54, 55, 56, 57, 58, 67, 68, 69, 70, 71, 72, 73, 74, 83, 84, 85, 86, 87, 88, 89, 90, 99, 100, 101, 102, 103, 104, 105, 106, 115, 116, 117, 118, 119, 120, 121, 122, 130, 131, 132, 133, 134, 135, 136, 137, 138, 146, 147, 148, 149, 150, 151, 152, 153, 154, 162, 163, 164, 165, 166, 167, 168, 169, 170, 178, 179, 180, 181, 182, 183, 184, 185, 186, 194, 195, 196, 197, 198, 199, 200, 201, 202, 210, 211, 212, 213, 214, 215, 216, 217, 218, 226, 227, 228, 229, 230, 231, 232, 233, 234, 242, 243, 244, 245, 246, 247, 248, 249, 250, 255, 218, 0, 12, 3, 1, 0, 2, 17, 3, 17, 0, 63, 0, 254, 254, 40, 160, 15, 255, 217 };
            var builder = new FakeBuilder()
                .BuildUser(1);
            var storageClient = Substitute.For<IStorageClient>();

            var handler = new CreatePlayerCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context), storageClient);
            var command = new CreatePlayerCommand("test1@test.com", "My name", "Elves", "Name", "Description", () => new MemoryStream(file));
            
            var result = handler.Execute(command);
            var player = builder.Context.Players.SingleOrDefault();

            result.Success.Should().BeTrue();
            player.Should().NotBeNull();
            player.DisplayName.Should().Be("My name");
            player.Race.Should().Be(Race.Elves);
            player.Profile.FullName.Should().Be("Name");
            player.Profile.Description.Should().Be("Description");
            player.Profile.AvatarLocation.Should().Be("1.jpeg");
            storageClient.Received().Store("1.jpeg", Arg.Any<MemoryStream>());
            builder.Context.CallsToSaveChanges.Should().Be(2);
        }

        [TestMethod]
        public void CreatePlayerCommandHandler_Succeeds_For_Png() {
            var file = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 1, 0, 0, 0, 1, 8, 2, 0, 0, 0, 144, 119, 83, 222, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4, 103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 14, 195, 0, 0, 14, 195, 1, 199, 111, 168, 100, 0, 0, 0, 12, 73, 68, 65, 84, 24, 87, 99, 248, 255, 255, 63, 0, 5, 254, 2, 254, 167, 53, 129, 132, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 };
            var builder = new FakeBuilder()
                .BuildUser(1);
            var storageClient = Substitute.For<IStorageClient>();

            var handler = new CreatePlayerCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context), storageClient);
            var command = new CreatePlayerCommand("test1@test.com", "My name", "Elves", "Name", "Description", () => new MemoryStream(file));

            var result = handler.Execute(command);
            var player = builder.Context.Players.SingleOrDefault();

            result.Success.Should().BeTrue();
            player.Should().NotBeNull();
            player.DisplayName.Should().Be("My name");
            player.Race.Should().Be(Race.Elves);
            player.Profile.FullName.Should().Be("Name");
            player.Profile.Description.Should().Be("Description");
            player.Profile.AvatarLocation.Should().Be("1.png");
            storageClient.Received().Store("1.png", Arg.Any<MemoryStream>());
            builder.Context.CallsToSaveChanges.Should().Be(2);
        }

        [TestMethod]
        public void CreatePlayerCommandHandler_Succeeds_Without_Avatar() {
            var builder = new FakeBuilder()
                .BuildUser(1);
            var storageClient = Substitute.For<IStorageClient>();

            var handler = new CreatePlayerCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context), storageClient);
            var command = new CreatePlayerCommand("test1@test.com", "My name", "Elves", "Name", "Description", null);

            var result = handler.Execute(command);
            var player = builder.Context.Players.SingleOrDefault();

            result.Success.Should().BeTrue();
            player.Should().NotBeNull();
            player.DisplayName.Should().Be("My name");
            player.Race.Should().Be(Race.Elves);
            player.Profile.FullName.Should().Be("Name");
            player.Profile.Description.Should().Be("Description");
            player.Profile.AvatarLocation.Should().BeNull();
            storageClient.DidNotReceiveWithAnyArgs().Store(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(2);
        }

        [TestMethod]
        public void CreatePlayerCommandHandler_Fails_For_Not_An_Image_File() {
            var builder = new FakeBuilder()
                .BuildUser(1);
            var storageClient = Substitute.For<IStorageClient>();

            var handler = new CreatePlayerCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context), storageClient);
            var command = new CreatePlayerCommand("test1@test.com", "My name", "Elves", "Name", "Description", () => new MemoryStream(new byte[] { 1, 2, 3, 4, 5 }));

            var result = handler.Execute(command);

            result.Should().HaveError(command => command.Avatar, "Avatar has to be a jpeg or png image");
            builder.Context.Players.Should().BeEmpty();
            storageClient.DidNotReceiveWithAnyArgs().Store(default, default);
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow("Humans", Race.Humans, DisplayName = "Humans")]
        [DataRow("Elves", Race.Elves, DisplayName = "Elves")]
        [DataRow("Dwarves", Race.Dwarves, DisplayName = "Dwarves")]
        public void CreatePlayerCommandHandler_Resolves_Race_Parameter_To_Correct_Race(string raceParameter, Race expectedRace) {
            var builder = new FakeBuilder()
                .BuildUser(1);
            var storageClient = Substitute.For<IStorageClient>();

            var handler = new CreatePlayerCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context), storageClient);
            var command = new CreatePlayerCommand("test1@test.com", "My name", raceParameter, null, null, null);

            var result = handler.Execute(command);
            var player = builder.Context.Players.SingleOrDefault();

            result.Success.Should().BeTrue();
            player.Should().NotBeNull();
            player.Race.Should().Be(expectedRace);
        }

        [TestMethod]
        public void CreatePlayerCommandHandler_Throws_Exception_For_Nonexistent_Race() {
            var builder = new FakeBuilder()
                .BuildUser(1);
            var storageClient = Substitute.For<IStorageClient>();

            var handler = new CreatePlayerCommandHandler(new UserRepository(builder.Context), new PlayerRepository(builder.Context), storageClient);
            var command = new CreatePlayerCommand("test1@test.com", "My name", "wrong", null, null, null);

            Action action = () => handler.Execute(command);

            action.Should().Throw<ArgumentException>();
            builder.Context.Players.Should().BeEmpty();
            builder.Context.CallsToSaveChanges.Should().Be(0);
        }
    }
}
