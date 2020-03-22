using System.Web.Mvc;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Market")]
    public class MarketController : BaseController {
        public MarketController(IAuthenticationService authenticationService, IMessageService messageService) : base(messageService, authenticationService) {
        }

        [Route("Caravans")]
        [HttpGet]
        public ActionResult Caravans() {
            return View(_messageService.Dispatch(new GetCaravansQuery(_authenticationService.Identity)));
        }

        [Route("Buy")]
        [HttpGet]
        public ActionResult Buy() {
            return View(_messageService.Dispatch(new GetAvailableMerchandiseQuery(_authenticationService.Identity)));
        }
    }
}