using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Models.Players;
using WarOfEmpires.Queries.Players;
using WarOfEmpires.Services;

namespace WarOfEmpires.ViewComponents {
    public class MenuViewComponent : ViewComponent {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMessageService _messageService;

        public MenuViewComponent(IAuthenticationService authenticationService, IMessageService messageService) {
            _authenticationService = authenticationService;
            _messageService = messageService;
        }

        public IViewComponentResult Invoke() {
            if (_authenticationService.IsAuthenticated) {
                return View(_messageService.Dispatch(new GetCurrentPlayerQuery(_authenticationService.Identity)));
            }
            else {
                return View(new CurrentPlayerViewModel() {
                    IsAuthenticated = false
                });
            }
        }
    }
}
