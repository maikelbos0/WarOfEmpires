using WarOfEmpires.Attributes;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [Route("Player")]
    public sealed class PlayerController : BaseController {
        public PlayerController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route("Index")]
        [HttpGet]
        public ViewResult Index() {
            return View(new PlayerSearchModel());
        }

        [Route("GetPlayers")]
        [HttpPost]
        public JsonResult GetPlayers(DataGridViewMetaData metaData, PlayerSearchModel search) {
            return GridJson(new GetPlayersQuery(_authenticationService.Identity, search.DisplayName), metaData);
        }

        [Route("Details")]
        [HttpGet]
        public ViewResult Details(int id) {
            return View(_messageService.Dispatch(new GetPlayerDetailsQuery(_authenticationService.Identity, id)));
        }

        [Route("GetNotifications")]
        [HttpPost]
        public JsonResult GetNotifications() {
            return Json(_messageService.Dispatch(new GetNotificationsQuery(_authenticationService.Identity)));
        }
    }
}