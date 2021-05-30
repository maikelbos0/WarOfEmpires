using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [Route("Market")]
    public class MarketController : BaseController {
        public MarketController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet("Sell")]
        public ViewResult Sell() {
            // Explicitly name view so it works from other actions
            return View("Sell", _messageService.Dispatch(new GetMarketQuery(_authenticationService.Identity)));
        }

        [HttpPost("Sell")]
        public ViewResult Sell(MarketModel model) {
            return BuildViewResultFor(new SellResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new MerchandiseInfo(m.Type, m.Quantity, m.Price))))
                .OnSuccess(Sell)
                .OnFailure("Sell", model);
        }

        [HttpPost("WithdrawCaravan")]
        public ViewResult WithdrawCaravan(int id) {
            return BuildViewResultFor(new WithdrawCaravanCommand(_authenticationService.Identity, id))
                .OnSuccess(Sell)
                .ThrowOnFailure();
        }

        [HttpGet("Buy")]
        public ViewResult Buy() {
            return View(_messageService.Dispatch(new GetMarketQuery(_authenticationService.Identity)));
        }

        [HttpPost("Buy")]
        public ViewResult Buy(MarketModel model) {
            return BuildViewResultFor(new BuyResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new MerchandiseInfo(m.Type, m.Quantity, m.Price))))
                .OnSuccess(Buy)
                .OnFailure("Buy", model);
        }

        [HttpGet("SellTransactions")]
        public ViewResult SellTransactions() {
            _messageService.Dispatch(new ReadTransactionsCommand(_authenticationService.Identity));

            return View();
        }

        [HttpPost("GetSellTransactions")]
        public JsonResult GetSellTransactions(DataGridViewMetaData metaData) {
            return GridJson(new GetSellTransactionsQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet("BuyTransactions")]
        public ViewResult BuyTransactions() {
            return View();
        }

        [HttpPost("GetBuyTransactions")]
        public JsonResult GetBuyTransactions(DataGridViewMetaData metaData) {
            return GridJson(new GetBuyTransactionsQuery(_authenticationService.Identity), metaData);
        }
    }
}