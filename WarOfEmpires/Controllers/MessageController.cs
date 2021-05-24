using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [Route("Message")]
    public class MessageController : BaseController {
        public MessageController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet("Index")]
        public ViewResult Index() {
            return View();
        }

        [HttpPost("GetReceivedMessages")]
        public JsonResult GetReceivedMessages(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedMessagesQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet("Send")]
        public ViewResult Send(int recipientId) {
            return View(_messageService.Dispatch(new GetMessageRecipientQuery(recipientId)));
        }

        [HttpGet("Reply")]
        public ViewResult Reply(int messageId) {
            return View("Send", _messageService.Dispatch(new GetReplyToMessageQuery(_authenticationService.Identity, messageId)));
        }

        [HttpPost("Reply")]
        [HttpPost("Send")]
        public ViewResult Send(MessageModel model) {
            return BuildViewResultFor(new SendMessageCommand(_authenticationService.Identity, model.RecipientId, model.Subject, model.Body))
                .OnSuccess(SentIndex)
                .OnFailure("Send", model);
        }

        [HttpGet("ReceivedDetails")]
        public ViewResult ReceivedDetails(int id) {
            var model = _messageService.Dispatch(new GetReceivedMessageQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadMessageCommand(_authenticationService.Identity, id));
            }

            return View(model);
        }

        [HttpGet("SentIndex")]
        public ViewResult SentIndex() {
            // Explicitly name view so it works from other actions
            return View("SentIndex");
        }

        [HttpPost("GetSentMessages")]
        public JsonResult GetSentMessages(DataGridViewMetaData metaData) {
            return GridJson(new GetSentMessagesQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet("SentDetails")]
        public ViewResult SentDetails(int id) {
            return View(_messageService.Dispatch(new GetSentMessageQuery(_authenticationService.Identity, id)));
        }
    }
}