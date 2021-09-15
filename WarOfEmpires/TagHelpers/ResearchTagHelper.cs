using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WarOfEmpires.Controllers;

namespace WarOfEmpires.TagHelpers {
    public class ResearchTagHelper : PartialTagHelper {
        public string Type { get; set; }

        public ResearchTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) : base(urlHelperFactory, actionContextAccessor) {
            Controller = EmpireController.Route;
            Action = nameof(EmpireController._Research);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            RouteValues = new { ResearchType = Type };

            base.Process(context, output);
        }
    }
}
