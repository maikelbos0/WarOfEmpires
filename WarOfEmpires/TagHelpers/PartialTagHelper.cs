using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WarOfEmpires.TagHelpers {
    public abstract class PartialTagHelper : TagHelper {
        public string Url { get; set; }
        public bool AjaxRefresh { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "partial-content");
            output.Attributes.SetAttribute("data-partial-url", Url);

            if (AjaxRefresh) {
                output.Attributes.SetAttribute("data-partial-ajax-refresh", "true");
            }
        }
    }
}
