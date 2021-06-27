using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Filters;
using WarOfEmpires.Queries.Game;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [Route("Game")]
    public class GameController : BaseController {
        public GameController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet("_GameStatus")]
        public PartialViewResult _GameStatus() {
            return PartialView(_messageService.Dispatch(new GetGameStatusQuery()));
        }
    }
}