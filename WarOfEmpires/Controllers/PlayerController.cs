using System.Web.Mvc;
using WarOfEmpires.Attributes;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [RoutePrefix("Player")]
    public sealed class PlayerController : BaseController {
        public PlayerController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return View(new PlayerSearchModel());
        }

        [Route("GetPlayers")]
        [HttpPost]
        public ActionResult GetPlayers(DataGridViewMetaData metaData, PlayerSearchModel search) {
            return GridJson(new GetPlayersQuery(search.DisplayName), metaData);
        }

        [Route("Details")]
        [HttpGet]
        public ActionResult Details(string id) {
            // TODO add alliance rights
            return View(_messageService.Dispatch(new GetPlayerDetailsQuery(id)));
        }

        [Route("_Notifications")]
        [HttpPost]
        public ActionResult _Notifications() {
            return Json(_messageService.Dispatch(new GetNotificationsQuery(_authenticationService.Identity)));
        }
    }
}