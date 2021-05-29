using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WarOfEmpires.TagHelpers {
    public class IconTagHelper : TagHelper {
        public IconType Type { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "span";
            output.Attributes.SetAttribute("class", $"si si-{Type.ToString().ToLower()}");
        }
    }
}
