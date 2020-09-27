using Unity;
using WarOfEmpires.Services;

namespace WarOfEmpires.Views {
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel> {
        // TODO add as extension method instead of overriding page        
        public bool IsAuthorized(IAllianceAuthorizationRequest request) {
            return UnityConfig.Container.Resolve<IAuthorizationService>().IsAuthorized(request);
        }
    }
}