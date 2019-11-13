using System.Web.Mvc;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Administration")]
    public class AdministrationController : BaseController {
        public AdministrationController(IAuthenticationService authenticationService, IMessageService messageService) : base(messageService, authenticationService) {
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return View();
        }
    }
}