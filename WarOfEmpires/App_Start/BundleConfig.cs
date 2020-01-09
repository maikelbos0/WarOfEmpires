using System.Web.Optimization;

namespace WarOfEmpires.App_Start {
    public static class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/toastr.js",
                "~/Scripts/jquery-datagridview.js",
                "~/Scripts/jquery-rangeslider.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
                "~/Scripts/umd/popper.js",
                "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Scripts/app").Include(
                "~/Scripts/application/ajax-form.js",
                "~/Scripts/application/building-totals-manager.js",
                "~/Scripts/application/command.js",
                "~/Scripts/application/housing-totals-manager.js",
                "~/Scripts/application/html-form.js",
                "~/Scripts/application/notification-manager.js",
                "~/Scripts/application/password-strength.js",
                "~/Scripts/application/resource-manager.js",
                "~/Scripts/application/jquery-datagridview-defaults.js",
                "~/Scripts/application/toastr-defaults.js"));

            bundles.Add(new StyleBundle("~/Content/css-bootstrap").Include(
                "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/site-icons.css",
                "~/Content/toastr.css",
                "~/Content/jquery-datagridview.css",
                "~/Content/jquery-datagridview.style.css",
                "~/Content/jquery-datagridview.style-custom.css",
                "~/Content/jquery-rangeslider.css",
                "~/Content/jquery-rangeslider.style.css"));
        }
    }
}