using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Queries.Markets;
using WarOfEmpires.Services;

namespace WarOfEmpires.ViewComponents {
    public class CaravansViewComponent : ViewComponent {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMessageService _messageService;

        public CaravansViewComponent(IAuthenticationService authenticationService, IMessageService messageService) {
            _authenticationService = authenticationService;
            _messageService = messageService;
        }

        public IViewComponentResult Invoke() {
            return View(_messageService.Dispatch(new GetCaravansQuery(_authenticationService.Identity)));
        }
    }
}
