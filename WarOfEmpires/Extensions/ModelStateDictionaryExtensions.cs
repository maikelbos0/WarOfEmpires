using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;

namespace WarOfEmpires.Extensions {
    public static class ModelStateDictionaryExtensions {
        public static void Merge<TCommand>(this ModelStateDictionary modelState, CommandResult<TCommand> commandResult) where TCommand : ICommand {
            foreach (var commandError in commandResult.Errors) {
                string propertyName = null;

                if (commandError.Expression != null) {
                    propertyName = ExpressionHelper.GetExpressionText(commandError.Expression);
                }
                
                if (propertyName != null && modelState.ContainsKey(propertyName)) {
                    modelState.AddModelError(propertyName, commandError.Message);
                }
                else {
                    modelState.AddModelError(string.Empty, commandError.Message);
                }
            }
        }
    }
}