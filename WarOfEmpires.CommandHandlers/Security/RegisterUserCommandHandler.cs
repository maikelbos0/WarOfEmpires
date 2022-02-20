using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Mail;

namespace WarOfEmpires.CommandHandlers.Security {
    public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand> {
        private readonly IUserRepository _repository;
        private readonly IMailClient _mailClient;
        private readonly IMailTemplate<ActivationMailTemplateParameters> _template;

        public RegisterUserCommandHandler(IUserRepository repository, IMailClient mailClient, IMailTemplate<ActivationMailTemplateParameters> template) {
            _repository = repository;
            _mailClient = mailClient;
            _template = template;
        }

        public CommandResult<RegisterUserCommand> Execute(RegisterUserCommand parameter) {
            var result = new CommandResult<RegisterUserCommand>();
            var user = new User(parameter.Email, parameter.Password);

            if (_repository.TryGetByEmail(user.Email) != null) {
                result.AddError(c => c.Email, "Email address already exists");
            }

            if (result.Success) {
                _repository.Add(user);

                _mailClient.Send(_template.GetMessage(new ActivationMailTemplateParameters(user.Email, user.ActivationCode.Value), user.Email));
            }

            return result;
        }
    }
}