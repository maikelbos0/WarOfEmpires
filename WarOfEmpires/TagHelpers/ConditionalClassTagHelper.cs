using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WarOfEmpires.TagHelpers {
    [HtmlTargetElement("*")]
    public class ConditionalClassTagHelper : TagHelper {
        public bool Condition { get; set; }
        public string ClassName { get; set; }
        public string NegativeClassName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            var className = Condition ? ClassName : NegativeClassName;

            if (!string.IsNullOrWhiteSpace(className)) {
                if (output.Attributes.TryGetAttribute("class", out var attribute)) {
                    output.Attributes.SetAttribute("class", $"{attribute.Value} {className}");
                }
                else {
                    output.Attributes.SetAttribute("class", className);
                }
            }
        }
    }
}
