using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Queries;
using WarOfEmpires.QueryHandlers;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace WarOfEmpires.Services {
    public sealed class MessageService : IMessageService {
        private readonly IServiceProvider _serviceProvider;

        public MessageService(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public CommandResult<TCommand> Dispatch<TCommand>(TCommand command) where TCommand : ICommand {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

            return handler.Execute(command);
        }

        public TReturnValue Dispatch<TReturnValue>(IQuery<TReturnValue> query) {
            var type = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TReturnValue));
            dynamic handler = _serviceProvider.GetRequiredService(type);

            return handler.Execute((dynamic)query);
        }
    }
}