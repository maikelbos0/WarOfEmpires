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
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new { data_success_message = "Your resources were stored in their banks." })) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Empire\Banking.cshtml	13	14
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Empire\Siege.cshtml	20	14
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new { data_success_message = "Your tax rate was changed." })) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Empire\Tax.cshtml	13	14
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new { data_success_message = $"Your {Model.Name} was upgraded." })) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Empire\_Building.cshtml	12	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\ChangeEmail.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\ChangePassword.cshtml	9	14
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new { @class = "html-only" })) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\Deactivate.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\ForgotPassword.cshtml	9	14
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new { @class = "html-only" })) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\LogIn.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\Register.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\ResetPassword.cshtml	9	14
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Home\SendActivation.cshtml	9	14
         * TODO @using (Html.BeginForm("LogOut", "Home", FormMethod.Post, new { @class = "html-only d-inline" })) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Layout\_Menu.cshtml	113	42
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Market\Buy.cshtml	13	14
         * TODO @using (Html.BeginForm(null, null, FormMethod.Post, new {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Market\Sell.cshtml	21	14
         * TODO @using (Html.BeginForm("WithdrawCaravan", null, FormMethod.Post, new {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Market\_Caravans.cshtml	32	38
         * TODO @using (Html.BeginForm()) {	C:\Users\Gebruiker\source\repos\WarOfEmpires\WarOfEmpires\Views\Message\Send.cshtml	13	14
        */
    }
}
 