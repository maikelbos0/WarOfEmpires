using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Commands.Messages;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Messages;
using WarOfEmpires.Queries.Messages;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [Route(Route)]
    public class MessageController : BaseController {
        public const string Route = "Message";

        public MessageController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet("", Order = -1)]
        [HttpGet(nameof(Index))]
        public ViewResult Index() {
            return View();
        }

        [HttpPost(nameof(GetReceivedMessages))]
        public JsonResult GetReceivedMessages(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedMessagesQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet(nameof(Send))]
        public ViewResult Send(int recipientId) {
            return View(_messageService.Dispatch(new GetMessageRecipientQuery(recipientId)));
        }

        [HttpGet(nameof(Reply))]
        public ViewResult Reply(int messageId) {
            return View(nameof(Send), _messageService.Dispatch(new GetReplyToMessageQuery(_authenticationService.Identity, messageId)));
        }

        [HttpPost(nameof(Send))]
        public ActionResult Send(MessageModel model) {
            return BuildViewResultFor(new SendMessageCommand(_authenticationService.Identity, model.RecipientId, model.Subject, model.Body))
                .OnSuccess(nameof(SentIndex))
                .OnFailure(model);
        }

        [HttpGet(nameof(ReceivedDetails))]
        public ViewResult ReceivedDetails(int id) {
            var model = _messageService.Dispatch(new GetReceivedMessageQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadMessageCommand(_authenticationService.Identity, id));
            }

            return View(model);
        }

        [HttpGet(nameof(SentIndex))]
        public ViewResult SentIndex() {
            return View();
        }

        [HttpPost(nameof(GetSentMessages))]
        public JsonResult GetSentMessages(DataGridViewMetaData metaData) {
            return GridJson(new GetSentMessagesQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet(nameof(SentDetails))]
        public ViewResult SentDetails(int id) {
            return View(_messageService.Dispatch(new GetSentMessageQuery(_authenticationService.Identity, id)));
        }
    }
}
