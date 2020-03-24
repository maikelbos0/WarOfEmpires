using System.Web.Mvc;
using WarOfEmpires.Commands.Markets;
using WarOfEmpires.Models.Markets;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [RoutePrefix("Market")]
    public class MarketController : BaseController {
        public MarketController(IAuthenticationService authenticationService, IMessageService messageService) : base(messageService, authenticationService) {
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
    }
}