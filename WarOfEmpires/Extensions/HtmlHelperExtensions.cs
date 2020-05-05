using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WarOfEmpires.Models;
using WarOfEmpires.Models.Empires;
using WarOfEmpires.Models.Grids;

namespace WarOfEmpires.Extensions {
    public static class HtmlHelperExtensions {
        public static MvcHtmlString DisplayFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, ResourcesViewModel>> expression) {
            var model = expression.Compile().Invoke(html.ViewData.Model);

            return html.Partial("_DisplayResources", model);
        }

        public static MvcHtmlString HiddenFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, ResourcesViewModel>> expression) {
            var model = expression.Compile().Invoke(html.ViewData.Model);
            var name = ExpressionHelper.GetExpressionText(expression);
            var viewData = new ViewDataDictionary(html.ViewData) {
                TemplateInfo = new TemplateInfo {
                    HtmlFieldPrefix = name
                }
            };

            return html.Partial("_HiddenResources", model, viewData);
        }

        public static MvcHtmlString IconFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, string>> expression) {
            var model = expression.Compile().Invoke(html.ViewData.Model);

            return html.Icon(model);
        }

        public static MvcHtmlString Icon(this HtmlHelper html, string expression) {
            return html.Partial("_Icon", expression);
        }

        public static MvcHtmlString Grid<TGridItem>(this HtmlHelper html, string id, string dataUrl, string detailUrl = null, string searchFormId = null) where TGridItem : EntityViewModel {
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
                    Data = c.Property.Name,
                    SortData = c.Attribute.SortData ?? c.Property.Name
                })
                .ToList();

            return html.Partial("_Grid", new GridViewModel() {
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