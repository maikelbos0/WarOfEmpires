using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Storage;

namespace WarOfEmpires.CommandHandlers.Players {
    public sealed class CreatePlayerCommandHandler : ICommandHandler<CreatePlayerCommand> {
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _repository;
        private readonly IStorageClient _storageClient;

        public CreatePlayerCommandHandler(IUserRepository userRepository, IPlayerRepository repository, IStorageClient storageClient) {
            _userRepository = userRepository;
            _repository = repository;
            _storageClient = storageClient;
        }

        public CommandResult<CreatePlayerCommand> Execute(CreatePlayerCommand command) {
            var result = new CommandResult<CreatePlayerCommand>();
            var race = (Race)Enum.Parse(typeof(Race), command.Race);
            var user = _userRepository.TryGetByEmail(command.Email);
            string imageFileExtension = null;

            if (command.Avatar != null) {
                try {
                    using (var stream = command.Avatar()) {
                        var imageFormat = Image.DetectFormat(command.Avatar());

                        if (imageFormat == JpegFormat.Instance) {
                            imageFileExtension = ".jpeg";
                        }
                        else if (imageFormat == PngFormat.Instance) {
                            imageFileExtension = ".png";
                        }
                    }
                }
                catch { }

                if (imageFileExtension == null) {
                    result.AddError(c => c.Avatar, "Avatar has to be a jpeg or png image");
                }
            }

            if (result.Success) {
                var player = new Player(user.Id, command.DisplayName, race);
                _repository.Add(player);

                player.Profile.FullName = command.FullName;
                player.Profile.Description = command.Description;

                if (imageFileExtension != null) {
                    var imageFilePath = player.Id.ToString() + imageFileExtension;

                    using (var stream = command.Avatar()) {
                        _storageClient.Store(imageFilePath, stream);
                    }

                    player.Profile.AvatarLocation = imageFilePath;
                }

                _repository.SaveChanges();
            }


            return result;
        }
    }
}
