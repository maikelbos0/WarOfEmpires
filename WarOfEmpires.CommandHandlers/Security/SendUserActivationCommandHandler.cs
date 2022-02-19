using VDT.Core.DependencyInjection.Attributes;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Mail;

namespace WarOfEmpires.CommandHandlers.Security {
    [TransientServiceImplementation(typeof(ICommandHandler<SendUserActivationCommand>))]
    public sealed class SendUserActivationCommandHandler : ICommandHandler<SendUserActivationCommand> {
        private readonly IUserRepository _repository;
        private readonly IMailClient _mailClient;
        private readonly IMailTemplate<ActivationMailTemplateParameters> _template;

        public SendUserActivationCommandHandler(IUserRepository repository, IMailClient mailClient, IMailTemplate<ActivationMailTemplateParameters> template) {
            _repository = repository;
            _mailClient = mailClient;
            _template = template;
        }

        public CommandResult<SendUserActivationCommand> Execute(SendUserActivationCommand parameter) {
            var result = new CommandResult<SendUserActivationCommand>();
            var user = _repository.TryGetByEmail(parameter.Email);

            // Don't return errors to prevent leaking information
            if (user != null && user.Status == UserStatus.New) {
                user.GenerateActivationCode();

                _repository.SaveChanges();

                _mailClient.Send(_template.GetMessage(new ActivationMailTemplateParameters(user.Email, user.ActivationCode.Value), user.Email));
            }

            return result;
        }
    }
}