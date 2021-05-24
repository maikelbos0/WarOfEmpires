using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using WarOfEmpires.Models;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.Extensions {
    public static class HtmlHelperExtensions {
        private static readonly PropertyInfo[] resourceViewModelProperties = new[] {
            typeof(ResourcesViewModel).GetProperty(nameof(ResourcesViewModel.Gold)),
            typeof(ResourcesViewModel).GetProperty(nameof(ResourcesViewModel.Food)),
            typeof(ResourcesViewModel).GetProperty(nameof(ResourcesViewModel.Wood)),
            typeof(ResourcesViewModel).GetProperty(nameof(ResourcesViewModel.Stone)),
            typeof(ResourcesViewModel).GetProperty(nameof(ResourcesViewModel.Ore))
        };

        public static IHtmlContent HiddenFor<TModel>(this IHtmlHelper<TModel> html, Expression<Func<TModel, ResourcesViewModel>> expression) {
            var builder = new HtmlContentBuilder();

            foreach (var resourceViewNodelProperty in resourceViewModelProperties) {
                var fieldExpression = Expression.Property(expression.Body, resourceViewNodelProperty);
                var fieldLambda = Expression.Lambda<Func<TModel, long>>(fieldExpression, expression.Parameters);

                builder.AppendHtml(html.HiddenFor(fieldLambda));
            }

            return builder;
        }

        public static async Task<IHtmlContent> Icon(this IHtmlHelper html, IconType type) {
            if (!Enum.IsDefined(type)) {
                throw new ArgumentException($"Invalid icon type found: '{type}'");
            }

            return await html.PartialAsync("_Icon", type);
        }

        public static async Task<IHtmlContent> Grid<TGridItem>(this IHtmlHelper html, string id, string dataUrl, string detailUrl = null, string searchFormId = null) where TGridItem : EntityViewModel {
            var gridSorting = typeof(TGridItem).GetCustomAttribute<GridSortingAttribute>();
            var gridColumns = typeof(TGridItem).GetProperties()
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

            return await html.PartialAsync("_Grid", new GridViewModel() {
                Id = id,
                DataUrl = dataUrl,
                DetailUrl = detailUrl,
                SearchFormId = searchFormId,
                Columns = gridColumns,
                SortColumn = gridSorting?.Column,
                SortDescending = gridSorting?.Descending ?? false
            });
        }
    }
}