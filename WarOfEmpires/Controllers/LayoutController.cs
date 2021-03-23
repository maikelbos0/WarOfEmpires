using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;

namespace WarOfEmpires.Controllers {
    [Route("Layout")]
    public class LayoutController : BaseController {
        public LayoutController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route("_Menu")]
        public PartialViewResult _Menu() {
            if (_authenticationService.IsAuthenticated) {
                return PartialView(_messageService.Dispatch(new GetCurrentPlayerQuery(_authenticationService.Identity)));
            }
            else {
                return PartialView(new CurrentPlayerViewModel() {
                    IsAuthenticated = false
                });
            }
        }
    }
}