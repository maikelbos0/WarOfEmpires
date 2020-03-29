using System.Web.Mvc;
using WarOfEmpires.Models;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Player")]
    public sealed class PlayerController : BaseController {
        public PlayerController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [Route("GetPlayers")]
        [HttpPost]
        public ActionResult GetPlayers(DataGridViewMetaData metaData) {
            return GridJson(new GetPlayersQuery(), metaData);
        }

        [Route("Details")]
        [HttpGet]
        public ActionResult Details(string id) {
            return View(_messageService.Dispatch(new GetPlayerDetailsQuery(id)));
        }

        [Route("_Notifications")]
        [HttpPost]
        public ActionResult _Notifications() {
            return Json(_messageService.Dispatch(new GetNotificationsQuery(_authenticationService.Identity)));
        }
    }
}