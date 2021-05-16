using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        private static readonly ModelExpressionProvider modelExpressionProvider = new ModelExpressionProvider(new EmptyModelMetadataProvider());

        // TODO fix warnings by switching to async
        public static IHtmlContent DisplayFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, ResourcesViewModel>> expression) {
            var model = expression.Compile().Invoke(html.ViewData.Model);

            return html.Partial("_DisplayResources", model);
        }

        // TODO fix warnings by switching to async
        public static IHtmlContent HiddenFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, ResourcesViewModel>> expression) {
            var model = expression.Compile().Invoke(html.ViewData.Model);
            var name = modelExpressionProvider.GetExpressionText(expression);
            var viewData = new ViewDataDictionary(html.ViewData) {
                TemplateInfo = {
                    HtmlFieldPrefix = name
                }
            };

            return html.Partial("_HiddenResources", model, viewData);
        }

        public static async Task<IHtmlContent> Icon(this IHtmlHelper html, IconType type) {
            if (!Enum.IsDefined<IconType>(type)) {
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