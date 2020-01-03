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
    }
}