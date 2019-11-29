using System.Collections.Generic;
using System.Web.Mvc;
using WarOfEmpires.Models;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Player")]
    public sealed class PlayerController : BaseController {
        private readonly IDataGridViewService _dataGridViewService;

        public PlayerController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) : base(messageService, authenticationService) {
            _dataGridViewService = dataGridViewService;
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
            IEnumerable<PlayerViewModel> data = _messageService.Dispatch(new GetPlayersQuery());

            data = _dataGridViewService.ApplyMetaData(data, ref metaData);

            return Json(new {
                metaData,
                data
            });
        }
    }
}