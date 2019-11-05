using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;
using System.Web.Mvc;

namespace WarOfEmpires.Extensions {
    public static class ModelStateDictionaryExtensions {
        public static void Merge<TCommand>(this ModelStateDictionary modelState, object model, CommandResult<TCommand> commandResult) where TCommand : ICommand {
            foreach (var commandError in commandResult.Errors) {
                string propertyName = null;
                var modelType = model.GetType();

                if (commandError.Expression != null) {
                    propertyName = ExpressionHelper.GetExpressionText(commandError.Expression);
                }
                
                if (propertyName == null || modelType.GetProperty(propertyName) == null) {
                    modelState.AddModelError(string.Empty, commandError.Message);
                }
                else {
                    modelState.AddModelError(propertyName, commandError.Message);
                }
            }
        }
    }
}