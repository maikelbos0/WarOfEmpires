using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [Route(Route)]
    public class MarketController : BaseController {
        public const string Route = "Market";

        public MarketController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService) 
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet(nameof(Sell))]
        public ViewResult Sell() {
            return View(_messageService.Dispatch(new GetMarketQuery(_authenticationService.Identity)));
        }

        [HttpPost(nameof(Sell))]
        public ActionResult Sell(MarketModel model) {
            return BuildViewResultFor(new SellResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new MerchandiseInfo(m.Type, m.Quantity, m.Price))))
                .OnSuccess(nameof(Sell))
                .OnFailure(model);
        }

        [HttpPost(nameof(WithdrawCaravan))]
        public ActionResult WithdrawCaravan(int id) {
            return BuildViewResultFor(new WithdrawCaravanCommand(_authenticationService.Identity, id))
                .OnSuccess(nameof(Sell))
                .ThrowOnFailure();
        }

        [HttpGet(nameof(Buy))]
        public ViewResult Buy() {
            return View(_messageService.Dispatch(new GetMarketQuery(_authenticationService.Identity)));
        }

        [HttpPost(nameof(Buy))]
        public ActionResult Buy(MarketModel model) {
            return BuildViewResultFor(new BuyResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new MerchandiseInfo(m.Type, m.Quantity, m.Price))))
                .OnSuccess(nameof(Buy))
                .OnFailure(nameof(Buy), model);
        }

        [HttpGet(nameof(SellTransactions))]
        public ViewResult SellTransactions() {
            _messageService.Dispatch(new ReadTransactionsCommand(_authenticationService.Identity));

            return View();
        }

        [HttpPost(nameof(GetSellTransactions))]
        public JsonResult GetSellTransactions(DataGridViewMetaData metaData) {
            return GridJson(new GetSellTransactionsQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet(nameof(BuyTransactions))]
        public ViewResult BuyTransactions() {
            return View();
        }

        [HttpPost(nameof(GetBuyTransactions))]
        public JsonResult GetBuyTransactions(DataGridViewMetaData metaData) {
            return GridJson(new GetBuyTransactionsQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet(nameof(BlackMarket))]
        public ViewResult BlackMarket() {
            return View(_messageService.Dispatch(new GetBlackMarketQuery()));
        }

        [HttpPost(nameof(BlackMarket))]
        public ActionResult BlackMarket(BlackMarketModel model) {
            return model.Command switch {
                "buy" => BuildViewResultFor(new BuyBlackMarketResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new BlackMarketMerchandiseInfo(m.Type, m.Quantity))))
                    .OnSuccess(nameof(BlackMarket))
                    .OnFailure(model),
                "sell" => BuildViewResultFor(new SellBlackMarketResourcesCommand(_authenticationService.Identity, model.Merchandise.Select(m => new BlackMarketMerchandiseInfo(m.Type, m.Quantity))))
                    .OnSuccess(nameof(BlackMarket))
                    .OnFailure(model),
                _ => throw new InvalidOperationException($"Invalid operation '{model.Command}' found"),
            };
        }
    }
}
