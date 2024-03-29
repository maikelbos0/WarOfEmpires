using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Commands.Players;
using WarOfEmpires.CompositeModels;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [Route(Route)]
    public sealed class PlayerController : BaseController {
        public const string Route = "Player";

        public PlayerController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet("", Order = -1)]
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

        [HttpGet(nameof(GetNotifications))]
        public JsonResult GetNotifications() {
            return Json(_messageService.Dispatch(new GetNotificationsQuery(_authenticationService.Identity)));
        }

        [HttpGet(nameof(Blocked))]
        public ViewResult Blocked() {
            return View(_messageService.Dispatch(new GetBlockedPlayersQuery(_authenticationService.Identity)));
        }

        [HttpPost(nameof(Block))]
        public ActionResult Block(int id) {
            return BuildViewResultFor(new BlockPlayerCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(Blocked))
                .ThrowOnFailure();
        }

        [HttpPost(nameof(Unblock))]
        public ActionResult Unblock(int id) {
            return BuildViewResultFor(new UnblockPlayerCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(Blocked))
                .ThrowOnFailure();
        }

        [HttpGet(nameof(Home))]
        public ViewResult Home() {
            return View(_messageService.Dispatch(new GetPlayerHomeQuery(_authenticationService.Identity)));
        }

        [HttpGet(nameof(EditProfile))]
        public ViewResult EditProfile() {
            return View(new AvatarCompositeModel<ProfileModel>() {
                Base = _messageService.Dispatch(new GetProfileQuery(_authenticationService.Identity))
            });
        }

        [HttpPost(nameof(EditProfile))]
        public ActionResult EditProfile(AvatarCompositeModel<ProfileModel> model) {
            return BuildViewResultFor(new UpdateProfileCommand(_authenticationService.Identity, model.Base.FullName, model.Base.Description, model.Avatar == null ? null : model.Avatar.OpenReadStream))
                .OnSuccess(nameof(EditProfile))
                .OnFailure(model);
        }

        [HttpGet(nameof(Create))]
        public ViewResult Create() {
            return View(new AvatarCompositeModel<CreatePlayerModel>() {
                Base = _messageService.Dispatch(new GetCreatePlayerQuery())
            });
        }

        [HttpPost(nameof(Create))]
        public ActionResult Create(AvatarCompositeModel<CreatePlayerModel> model) {
            return BuildViewResultFor(new CreatePlayerCommand(_authenticationService.Identity, model.Base.DisplayName, model.Base.Race, model.Base.FullName, model.Base.Description, model.Avatar == null ? null : model.Avatar.OpenReadStream))
                .OnSuccess(nameof(Home))
                .OnFailure(model);
        }
    }
}
