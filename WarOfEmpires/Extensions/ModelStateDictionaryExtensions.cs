using Microsoft.AspNetCore.Mvc.ModelBinding;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using WarOfEmpires.Utilities.Extensions;

namespace WarOfEmpires.Extensions {
    public static class ModelStateDictionaryExtensions {
        public static void Merge<TCommand>(this ModelStateDictionary modelState, CommandResult<TCommand> commandResult) where TCommand : ICommand {
            foreach (var commandError in commandResult.Errors) {
                string propertyName = null;

                if (commandError.Expression != null) {
                    propertyName = commandError.Expression.GetExpressionText();
                }
                
                if (propertyName != null) {
                    modelState.AddModelError(propertyName, commandError.Message);
                }
                else {
                    modelState.AddModelError(string.Empty, commandError.Message);
                }
            }
        }
    }
}