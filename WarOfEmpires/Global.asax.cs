using WarOfEmpires.App_Start;
using System;
using System.Configuration;

namespace WarOfEmpires {
    public class Global : System.Web.HttpApplication {
        protected void Application_Start(object sender, EventArgs e) {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
 