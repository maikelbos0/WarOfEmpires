using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Models.Empires;

namespace WarOfEmpires.ViewComponents {
    public class ResourcesViewComponent : ViewComponent {
        public IViewComponentResult Invoke(ResourcesViewModel model) {
            return View(model);
        }
    }
}
