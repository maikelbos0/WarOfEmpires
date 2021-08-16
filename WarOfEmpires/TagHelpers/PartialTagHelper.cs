using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WarOfEmpires.TagHelpers {
    public class PartialTagHelper : TagHelper {
        private readonly IUrlHelper _urlHelper;

        public string Controller { get; set; }
        public string Action { get; set; }
        public object RouteValues { get; set; }
        public bool AjaxRefresh { get; set; }

        public PartialTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor) {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "partial-content");
            output.Attributes.SetAttribute("data-partial-url", _urlHelper.Action(Action, Controller, RouteValues));

            if (AjaxRefresh) {
                output.Attributes.SetAttribute("data-partial-ajax-refresh", "true");
            }
        }
    }
}
