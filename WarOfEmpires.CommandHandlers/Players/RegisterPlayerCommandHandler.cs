using System.Drawing;
using System.Drawing.Imaging;
using VDT.Core.DependencyInjection;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Players;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Storage;

namespace WarOfEmpires.CommandHandlers.Players {
    [TransientServiceImplementation(typeof(ICommandHandler<RegisterPlayerCommand>))]
    public sealed class RegisterPlayerCommandHandler : ICommandHandler<RegisterPlayerCommand> {
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _repository;
        private readonly IStorageClient _storageClient;

        public RegisterPlayerCommandHandler(IUserRepository userRepository, IPlayerRepository repository, IStorageClient storageClient) {
            _userRepository = userRepository;
            _repository = repository;
            _storageClient = storageClient;
        }

        public CommandResult<RegisterPlayerCommand> Execute(RegisterPlayerCommand command) {
            var result = new CommandResult<RegisterPlayerCommand>();
            var user = _userRepository.TryGetByEmail(command.Email);
            string imageFileExtension = null;

            if (command.Avatar != null) {
                try {
                    using (var stream = command.Avatar()) {
                        var image = Image.FromStream(command.Avatar());

                        if (image.RawFormat.Equals(ImageFormat.Jpeg)) {
                            imageFileExtension = ".jpeg";
                        }
                        else if (image.RawFormat.Equals(ImageFormat.Png)) {
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
                var player = new Player(user.Id, command.DisplayName);
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