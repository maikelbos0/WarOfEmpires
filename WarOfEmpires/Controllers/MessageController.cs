using System.Web.Mvc;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Message")]
    public class MessageController : BaseController {
        public MessageController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
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
            return GridJson(new GetReceivedMessagesQuery(_authenticationService.Identity), metaData);
        }

        [Route("Send")]
        [HttpGet]
        public ActionResult Send(string recipientId) {
            return View(_messageService.Dispatch(new GetMessageRecipientQuery(recipientId)));
        }

        [Route("Reply")]
        [HttpGet]
        public ActionResult Reply(string messageId) {
            return View("Send", _messageService.Dispatch(new GetReplyToMessageQuery(_authenticationService.Identity, messageId)));
        }

        [Route("Reply")]
        [Route("Send")]
        [HttpPost]
        public ActionResult Send(MessageModel model) {
            return ValidatedCommandResult(model,
                new SendMessageCommand(_authenticationService.Identity, model.RecipientId, model.Subject, model.Body),
                "Sent");
        }

        [Route("ReceivedDetails")]
        [HttpGet]
        public ActionResult ReceivedDetails(string id) {
            var model = _messageService.Dispatch(new GetReceivedMessageQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadMessageCommand(_authenticationService.Identity, id));
            }

            return View(model);
        }

        [Route("SentIndex")]
        [HttpGet]
        public ActionResult SentIndex() {
            return View();
        }

        [Route("GetSentMessages")]
        [HttpPost]
        public ActionResult GetSentMessages(DataGridViewMetaData metaData) {
            return GridJson(new GetSentMessagesQuery(_authenticationService.Identity), metaData);
        }

        [Route("SentDetails")]
        [HttpGet]
        public ActionResult SentDetails(string id) {
            return View(_messageService.Dispatch(new GetSentMessageQuery(_authenticationService.Identity, id)));
        }
    }
}