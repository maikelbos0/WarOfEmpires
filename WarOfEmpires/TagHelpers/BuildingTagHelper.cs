using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WarOfEmpires.Controllers;

namespace WarOfEmpires.TagHelpers {
    public class BuildingTagHelper : PartialTagHelper {
        public string Type { get; set; }

        public BuildingTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(urlHelperFactory, actionContextAccessor) {
            Controller = EmpireController.Route;
            Action = nameof(EmpireController._Building);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            RouteValues = new { BuildingType = Type };

            base.Process(context, output);
        }
    }
}
