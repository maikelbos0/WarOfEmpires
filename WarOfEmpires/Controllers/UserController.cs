using WarOfEmpires.Models;
using WarOfEmpires.Models.Security;
using WarOfEmpires.Queries.Security;
using WarOfEmpires.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("User")]
    public sealed class UserController : BaseController {
        private readonly IDataGridViewService _dataGridViewService;

        public UserController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) : base(messageService, authenticationService) {
            _dataGridViewService = dataGridViewService;
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return View();
        }
        
        [Route("GetUsers")]
        [HttpPost]
        public ActionResult GetUsers(DataGridViewMetaData metaData) {
            IEnumerable<UserViewModel> data = _messageService.Dispatch(new GetUsersQuery());

            data = _dataGridViewService.ApplyMetaData(data, ref metaData);

            return Json(new {
                metaData,
                data
            });
        }
    }
}