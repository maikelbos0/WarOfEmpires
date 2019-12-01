using System.Collections.Generic;
using System.Web.Mvc;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Models;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Message")]
    public class MessageController : BaseController {
        private readonly IDataGridViewService _dataGridViewService;

        public MessageController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) : base(messageService, authenticationService) {
            _dataGridViewService = dataGridViewService;
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [Route("GetReceivedMessages")]
        [HttpPost]
        public ActionResult GetReceivedMessages(DataGridViewMetaData metaData) {
            IEnumerable<ReceivedMessageViewModel> data = _messageService.Dispatch(new GetReceivedMessagesQuery(_authenticationService.Identity));

            data = _dataGridViewService.ApplyMetaData(data, ref metaData);

            return Json(new {
                metaData,
                data
            });
        }

        [Route("Send")]
        [HttpGet]
        public ActionResult Send(string recipientId) {
            return View(_messageService.Dispatch(new GetMessageRecipientQuery(recipientId)));
        }

        [Route("Send")]
        [HttpPost]
        public ActionResult Send(MessageModel model) {
            return ValidatedCommandResult(model,
                new SendMessageCommand(_authenticationService.Identity, model.RecipientId, model.Subject, model.Body),
                "Sent");
        }
    }
}