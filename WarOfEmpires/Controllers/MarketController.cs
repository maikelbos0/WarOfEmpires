using System.Linq;
using WarOfEmpires.Attributes;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;

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
        public ViewResult Sell() {
            // Explicitly name view so it works from other actions
            return View("Sell", _messageService.Dispatch(new GetMarketQuery(_authenticationService.Identity)));
        }

        [Route("Sell")]
        [HttpPost]
        public ViewResult Sell(MarketModel model) {
            return BuildViewResultFor(new SellResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new MerchandiseInfo(m.Type, m.Quantity, m.Price))))
                .OnSuccess(Sell)
                .OnFailure("Sell", model);
        }

        [Route("_Caravans")]
        public PartialViewResult _Caravans() {
            return PartialView("_Caravans", _messageService.Dispatch(new GetCaravansQuery(_authenticationService.Identity)));
        }

        [Route("WithdrawCaravan")]
        [HttpPost]
        public ViewResult WithdrawCaravan(int id) {
            return BuildViewResultFor(new WithdrawCaravanCommand(_authenticationService.Identity, id))
                .OnSuccess(Sell)
                .ThrowOnFailure();
        }

        [Route("Buy")]
        [HttpGet]
        public ViewResult Buy() {
            return View(_messageService.Dispatch(new GetMarketQuery(_authenticationService.Identity)));
        }

        [Route("Buy")]
        [HttpPost]
        public ViewResult Buy(MarketModel model) {
            return BuildViewResultFor(new BuyResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new MerchandiseInfo(m.Type, m.Quantity, m.Price))))
                .OnSuccess(Buy)
                .OnFailure("Buy", model);
        }

        [Route("SellTransactions")]
        [HttpGet]
        public ViewResult SellTransactions() {
            _messageService.Dispatch(new ReadTransactionsCommand(_authenticationService.Identity));

            return View();
        }

        [Route("GetSellTransactions")]
        [HttpPost]
        public JsonResult GetSellTransactions(DataGridViewMetaData metaData) {
            return GridJson(new GetSellTransactionsQuery(_authenticationService.Identity), metaData);
        }

        [Route("BuyTransactions")]
        [HttpGet]
        public ViewResult BuyTransactions() {
            return View();
        }

        [Route("GetBuyTransactions")]
        [HttpPost]
        public JsonResult GetBuyTransactions(DataGridViewMetaData metaData) {
            return GridJson(new GetBuyTransactionsQuery(_authenticationService.Identity), metaData);
        }
    }
}