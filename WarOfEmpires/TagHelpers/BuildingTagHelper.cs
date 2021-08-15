using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WarOfEmpires.Controllers;

namespace WarOfEmpires.TagHelpers {
    public class BuildingTagHelper : PartialTagHelper {
        private readonly IUrlHelper _urlHelper;

        public string Type { get; set; }

        public BuildingTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            Url = _urlHelper.Action(nameof(EmpireController._Building), EmpireController.Route, new { BuildingType = Type });

            base.Process(context, output);
        }
    }
}
