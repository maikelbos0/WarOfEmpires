using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WarOfEmpires.TagHelpers {
    public abstract class PartialTagHelper : TagHelper {
        private readonly string _id;
        private readonly string _url;

        public PartialTagHelper(string id, string url) {
            _id = id;
            _url = url;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "partial-content");
            output.Attributes.SetAttribute("data-partial-url", _url);
            output.Attributes.SetAttribute("data-partial-ajax-refresh", "true");
            output.Attributes.SetAttribute("id", _id);
        }
    }
}
