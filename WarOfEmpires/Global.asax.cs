using WarOfEmpires.App_Start;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WarOfEmpires {
    public class Global : System.Web.HttpApplication {
        protected void Application_Start(object sender, EventArgs e) {
            var instrumentationKey = ConfigurationManager.AppSettings["ApplicationInsights.InstrumentationKey"];

            if (string.IsNullOrEmpty(instrumentationKey)) {
                TelemetryConfiguration.Active.DisableTelemetry = true;
            }
            else {
                TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /*
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\ChangeEmail.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\ChangePassword.cshtml	9	14
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new { @class = "html-only" })) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\Deactivate.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\ForgotPassword.cshtml	9	14
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new { @class = "html-only" })) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\LogIn.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\Register.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\ResetPassword.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\SendActivation.cshtml	9	14
         * TODO @using (Html.BeginForm("LogOut", "Home", FormMethod.Post, new { @class = "html-only d-inline" })) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Layout\_Menu.cshtml	113	42
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Message\Send.cshtml	13	14
        */
    }
}
 