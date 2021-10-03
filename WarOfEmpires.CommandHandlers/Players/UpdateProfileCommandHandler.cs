using System.Drawing;
using System.Drawing.Imaging;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Utilities.Storage;

namespace WarOfEmpires.CommandHandlers.Players {
    [TransientServiceImplementation(typeof(ICommandHandler<UpdateProfileCommand>))]
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
                    using (var stream = command.Avatar()) {
                        var image = Image.FromStream(command.Avatar());

                        if (image.RawFormat.Equals(ImageFormat.Jpeg)) {
                            imageFilePath = $"{player.Id}.jpeg";
                        }
                        else if (image.RawFormat.Equals(ImageFormat.Png)) {
                            imageFilePath = $"{player.Id}.png";
                        }
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
