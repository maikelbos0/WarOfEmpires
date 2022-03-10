using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Storage;

namespace WarOfEmpires.CommandHandlers.Players {
    public sealed class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IStorageClient _storageClient;

        public UpdateProfileCommandHandler(IPlayerRepository repository, IStorageClient storageClient) {
            _repository = repository;
            _storageClient = storageClient;
        }

        public CommandResult<UpdateProfileCommand> Execute(UpdateProfileCommand command) {
            var result = new CommandResult<UpdateProfileCommand>();
            var player = _repository.Get(command.Email);
            string imageFilePath = null;

            if (command.Avatar != null) {
                try {
                    using var stream = command.Avatar();
                    var imageFormat = Image.DetectFormat(command.Avatar());

                    if (imageFormat == JpegFormat.Instance) {
                        imageFilePath = $"{player.Id}.jpeg";
                    }
                    else if (imageFormat == PngFormat.Instance) {
                        imageFilePath = $"{player.Id}.png";
                    }
                }
                catch { }

                if (imageFilePath == null) {
                    result.AddError(c => c.Avatar, "New avatar has to be a jpeg or png image");
                }
            }

            if (result.Success) {
                player.Profile.FullName = command.FullName;
                player.Profile.Description = command.Description;

                if (imageFilePath != null) {
                    if (player.Profile.AvatarLocation != null) {
                        _storageClient.Delete(player.Profile.AvatarLocation);
                    }

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
