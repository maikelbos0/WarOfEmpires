using VDT.Core.DependencyInjection;
using WarOfEmpires.Commands.Security;
using WarOfEmpires.Domain.Security;
using WarOfEmpires.Repositories.Security;
using WarOfEmpires.Utilities.Mail;

namespace WarOfEmpires.CommandHandlers.Security {
    [ScopedServiceImplementation(typeof(ICommandHandler<ForgotUserPasswordCommand>))]
    public sealed class ForgotUserPasswordCommandHandler : ICommandHandler<ForgotUserPasswordCommand> {
        private readonly IUserRepository _repository;
        private readonly IMailClient _mailClient;
        private readonly IMailTemplate<PasswordResetMailTemplateParameters> _template;

        public ForgotUserPasswordCommandHandler(IUserRepository repository, IMailClient mailClient, IMailTemplate<PasswordResetMailTemplateParameters> template) {
            _repository = repository;
            _mailClient = mailClient;
            _template = template;
        }

        public CommandResult<ForgotUserPasswordCommand> Execute(ForgotUserPasswordCommand parameter) {
            var result = new CommandResult<ForgotUserPasswordCommand>();
            var user = _repository.TryGetByEmail(parameter.Email);

            // There is no error reporting to prevent information leaking
            if (user != null && user.Status == UserStatus.Active) {
                var token = user.GeneratePasswordResetToken();

                _mailClient.Send(_template.GetMessage(new PasswordResetMailTemplateParameters(user.Email, token), user.Email));
            }
            else {
                user?.PasswordResetRequestFailed();
            }
            
            _repository.SaveChanges();

            return result;
        }
    }
}
