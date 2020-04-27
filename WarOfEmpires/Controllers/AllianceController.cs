using System.Web.Mvc;
using WarOfEmpires.Models.Alliances;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    public class AllianceController : BaseController {
        public AllianceController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet]
        public ActionResult Create() {
            return null;
        }

        [HttpPost]
        public ActionResult Create(CreateAllianceModel model) {
            return null;
        }
    }
}