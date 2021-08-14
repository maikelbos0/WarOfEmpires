using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [Route("Player")]
    public sealed class PlayerController : BaseController {
        public PlayerController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet]
        [HttpGet(nameof(Index))]
        public ViewResult Index() {
            return View(new PlayerSearchModel());
        }

        [HttpPost(nameof(GetPlayers))]
        public JsonResult GetPlayers(DataGridViewMetaData metaData, PlayerSearchModel search) {
            return GridJson(new GetPlayersQuery(_authenticationService.Identity, search.DisplayName), metaData);
        }

        [HttpGet(nameof(Details))]
        public ViewResult Details(int id) {
            return View(_messageService.Dispatch(new GetPlayerDetailsQuery(_authenticationService.Identity, id)));
        }

        [HttpPost(nameof(GetNotifications))]
        public JsonResult GetNotifications() {
            return Json(_messageService.Dispatch(new GetNotificationsQuery(_authenticationService.Identity)));
        }

        [HttpGet(nameof(Blocked))]
        public ViewResult Blocked() {
            // Explicitly name view so it works from other actions
            return View(nameof(Blocked), _messageService.Dispatch(new GetBlockedPlayersQuery(_authenticationService.Identity)));
        }

        [HttpPost(nameof(Block))]
        public ViewResult Block(int id) {
            return BuildViewResultFor(new BlockPlayerCommand(_authenticationService.Identity, id))
                .OnSuccess(Blocked)
                .ThrowOnFailure();
        }

        [HttpPost(nameof(Unblock))]
        public ViewResult Unblock(int id) {
            return BuildViewResultFor(new UnblockPlayerCommand(_authenticationService.Identity, id))
                .OnSuccess(Blocked)
                .ThrowOnFailure();
        }

        [HttpGet(nameof(Home))]
        public ViewResult Home() {
            return View();
        }
    }
}