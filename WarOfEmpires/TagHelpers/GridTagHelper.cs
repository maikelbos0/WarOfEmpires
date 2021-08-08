using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Reflection;
using WarOfEmpires.Extensions;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.TagHelpers {
    public class GridTagHelper : TagHelper {
        private readonly IUrlHelper urlHelper;

        public string Id { get; set; }
        public Type ItemType { get; set; }
        public string ItemController { get; set; }
        public string ItemAction { get; set; }
        public string DetailController { get; set; }
        public string DetailAction { get; set; }
        public string SearchFormId { get; set; }

        public GridTagHelper(IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory) {
            urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            var sortOrder = ItemType.GetCustomAttribute<GridSortingAttribute>();
            var columns = ItemType.GetProperties()
                .Select(p => new {
                    Property = p,
                    Attribute = p.GetCustomAttribute<GridColumnAttribute>()
                })
                .Where(c => c.Attribute != null)
                .OrderBy(c => c.Attribute.Index)
                .Select(c => new GridColumnViewModel() {
                    Width = c.Attribute.Width,
                    Header = c.Attribute.Header,
                    Data = c.Property.Name.ToCamelCase(),
                    SortData = (c.Attribute.SortData ?? c.Property.Name).ToCamelCase(),
                    ResponsiveDisplayBehaviour = c.Attribute.ResponsiveDisplayBehaviour
                })
                .ToList();

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";

            output.Content.AppendHtml($@"
                <div id=""{Id}""></div>
                <script type=""text/javascript"">
                    $(function () {{
                        var grid = new Grid('{Id}', '{urlHelper.Action(ItemAction, ItemController)}');
            ");

            if (!string.IsNullOrEmpty(DetailAction)) {
                output.Content.AppendHtml($@"
                        grid.detailUrl = '{urlHelper.Action(DetailAction, ItemController)}';
                ");
            }

            if (!string.IsNullOrEmpty(SearchFormId)) {
                output.Content.AppendHtml($@"
                        grid.searchFormId = '{SearchFormId}';
                ");
            }

            if (sortOrder != null) {
                output.Content.AppendHtml($@"
                        grid.sortColumn = '{sortOrder.Column.ToCamelCase()}';
                        grid.sortDescending = {sortOrder.Descending.ToString().ToLower()};
                ");
            }

            foreach (var column in columns) {
                output.Content.AppendHtml($@"
                        grid.addColumn({column.Width}, '{column.Data}', '{column.Header}', '{column.SortData}', '{column.ResponsiveDisplayBehaviour}');
                ");
            }

            output.Content.AppendHtml(@"
                        grid.initialize();
                    });
                </script>
            ");

            base.Process(context, output);
        }
    }
}
