using System.Web.Mvc;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Empire")]
    public class MarketController : BaseController {
        public MarketController(IAuthenticationService authenticationService, IMessageService messageService) : base(messageService, authenticationService) {
        }

        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return new EmptyResult();
        }

        [Route("BuyIndex")]
        [HttpGet]
        public ActionResult BuyIndex() {
            return new EmptyResult();
        }
    }
}