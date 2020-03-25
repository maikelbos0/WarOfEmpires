using System.Collections.Generic;
using System.Web.Mvc;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Models;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Market")]
    public class MarketController : BaseController {
        private readonly IDataGridViewService _dataGridViewService;

        public MarketController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) : base(messageService, authenticationService) {
            _dataGridViewService = dataGridViewService;
        }

        [Route("Sell")]
        [HttpGet]
        public ActionResult Sell() {
            return View(_messageService.Dispatch(new GetMarketQuery(_authenticationService.Identity)));
        }

        [Route("Sell")]
        [HttpPost]
        public ActionResult Sell(MarketModel model) {
            return ValidatedCommandResult(model,
                new SellResourcesCommand(_authenticationService.Identity, model.Food, model.FoodPrice, model.Wood, model.WoodPrice, model.Stone, model.StonePrice, model.Ore, model.OrePrice),
                () => Sell());
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
            return ValidatedCommandResult(model,
                new BuyResourcesCommand(_authenticationService.Identity, model.Food, model.FoodPrice, model.Wood, model.WoodPrice, model.Stone, model.StonePrice, model.Ore, model.OrePrice),
                () => Buy());
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
            IEnumerable<TransactionViewModel> data = _messageService.Dispatch(new GetSellTransactionsQuery(_authenticationService.Identity));

            data = _dataGridViewService.ApplyMetaData(data, ref metaData);

            return Json(new {
                metaData,
                data
            });
        }

        [Route("BuyTransactions")]
        [HttpGet]
        public ActionResult BuyTransactions() {
            return View();
        }

        [Route("GetBuyTransactions")]
        [HttpPost]
        public ActionResult GetBuyTransactions(DataGridViewMetaData metaData) {
            IEnumerable<TransactionViewModel> data = _messageService.Dispatch(new GetBuyTransactionsQuery(_authenticationService.Identity));

            data = _dataGridViewService.ApplyMetaData(data, ref metaData);

            return Json(new {
                metaData,
                data
            });
        }
    }
}