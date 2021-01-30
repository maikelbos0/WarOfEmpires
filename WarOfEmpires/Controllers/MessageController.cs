using System.Web.Mvc;
using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [RoutePrefix("Message")]
    public class MessageController : BaseController {
        public MessageController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route]
        [Route("Index")]
        [HttpGet]
        public ViewResult Index() {
            return View();
        }

        [Route("GetReceivedMessages")]
        [HttpPost]
        public JsonResult GetReceivedMessages(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedMessagesQuery(_authenticationService.Identity), metaData);
        }

        [Route("Send")]
        [HttpGet]
        public ViewResult Send(int recipientId) {
            return View(_messageService.Dispatch(new GetMessageRecipientQuery(recipientId)));
        }

        [Route("Reply")]
        [HttpGet]
        public ViewResult Reply(int messageId) {
            return View("Send", _messageService.Dispatch(new GetReplyToMessageQuery(_authenticationService.Identity, messageId)));
        }

        [Route("Reply")]
        [Route("Send")]
        [HttpPost]
        public ViewResult Send(MessageModel model) {
            return BuildViewResultFor(new SendMessageCommand(_authenticationService.Identity, model.RecipientId, model.Subject, model.Body))
                .OnSuccess(SentIndex)
                .OnFailure("Send", model);
        }

        [Route("ReceivedDetails")]
        [HttpGet]
        public ViewResult ReceivedDetails(int id) {
            var model = _messageService.Dispatch(new GetReceivedMessageQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadMessageCommand(_authenticationService.Identity, id));
            }

            return View(model);
        }

        [Route("SentIndex")]
        [HttpGet]
        public ViewResult SentIndex() {
            // Explicitly name view so it works from other actions
            return View("SentIndex");
        }

        [Route("GetSentMessages")]
        [HttpPost]
        public JsonResult GetSentMessages(DataGridViewMetaData metaData) {
            return GridJson(new GetSentMessagesQuery(_authenticationService.Identity), metaData);
        }

        [Route("SentDetails")]
        [HttpGet]
        public ViewResult SentDetails(int id) {
            return View(_messageService.Dispatch(new GetSentMessageQuery(_authenticationService.Identity, id)));
        }
    }
}