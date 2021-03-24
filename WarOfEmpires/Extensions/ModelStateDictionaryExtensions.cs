using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;

namespace WarOfEmpires.Extensions {
    public static class ModelStateDictionaryExtensions {
        private static readonly ModelExpressionProvider modelExpressionProvider = new ModelExpressionProvider(new EmptyModelMetadataProvider());

        public static void Merge<TCommand>(this ModelStateDictionary modelState, CommandResult<TCommand> commandResult) where TCommand : ICommand {
            foreach (var commandError in commandResult.Errors) {
                string propertyName = null;

                if (commandError.Expression != null) {
                    // TODO test this
                    propertyName = modelExpressionProvider.GetExpressionText(commandError.Expression);
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