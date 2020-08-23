using System.Linq;
using System.Web.Mvc;
using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [RoutePrefix("Market")]
    public class MarketController : BaseController {
        public MarketController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [Route("Sell")]
        [HttpGet]
        public ActionResult Sell() {
            return View(_messageService.Dispatch(new GetMarketQuery(_authenticationService.Identity)));
        }

        [Route("Sell")]
        [HttpPost]
        public ActionResult Sell(MarketModel model) {
            return ValidatedCommandResult2(model,
                new SellResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new MerchandiseInfo(m.Type, m.Quantity, m.Price))),
                Sell);
        }

        [Route("Caravans")]
        public ActionResult Caravans() {
            return PartialView("_Caravans", _messageService.Dispatch(new GetCaravansQuery(_authenticationService.Identity)));
        }

        [Route("WithdrawCaravan")]
        [HttpPost]
        public ActionResult WithdrawCaravan(string id) {
            _messageService.Dispatch(new WithdrawCaravanCommand(_authenticationService.Identity, id));

            return RedirectToAction("Sell");
        }

        [Route("Buy")]
        [HttpGet]
        public ActionResult Buy() {
            return View(_messageService.Dispatch(new GetMarketQuery(_authenticationService.Identity)));
        }

        [Route("Buy")]
        [HttpPost]
        public ActionResult Buy(MarketModel model) {
            return ValidatedCommandResult2(model,
                new BuyResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new MerchandiseInfo(m.Type, m.Quantity, m.Price))),
                Buy);
        }

        [Route("SellTransactions")]
        [HttpGet]
        public ActionResult SellTransactions() {
            _messageService.Dispatch(new ReadTransactionsCommand(_authenticationService.Identity));

            return View();
        }

        [Route("GetSellTransactions")]
        [HttpPost]
        public ActionResult GetSellTransactions(DataGridViewMetaData metaData) {
            return GridJson(new GetSellTransactionsQuery(_authenticationService.Identity), metaData);
        }

        [Route("BuyTransactions")]
        [HttpGet]
        public ActionResult BuyTransactions() {
            return View();
        }

        [Route("GetBuyTransactions")]
        [HttpPost]
        public ActionResult GetBuyTransactions(DataGridViewMetaData metaData) {
            return GridJson(new GetBuyTransactionsQuery(_authenticationService.Identity), metaData);
        }
    }
}