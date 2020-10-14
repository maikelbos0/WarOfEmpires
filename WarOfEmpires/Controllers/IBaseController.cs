using System.Web.Mvc;
using WarOfEmpires.CommandHandlers;
using WarOfEmpires.Commands;

namespace WarOfEmpires.Controllers {
    public interface IBaseController {
        ViewDataDictionary ViewData { get; }
        ViewEngineCollection ViewEngineCollection { get; }
        void AddResponseHeader(string name, string value);
        void ClearModelState();
        bool IsModelStateValid();
        void MergeModelState<TCommand>(CommandResult<TCommand> commandResult) where TCommand : ICommand;
    }
}