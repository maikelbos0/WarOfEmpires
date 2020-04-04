using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using WarOfEmpires.Models.Empires;

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
    }
}