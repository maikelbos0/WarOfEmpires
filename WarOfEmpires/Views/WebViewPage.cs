using Unity;
using WarOfEmpires.Services;

namespace WarOfEmpires.Views {
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel> {
        public bool IsAuthorized(IAllianceAuthorizationRequest request) {
            return UnityConfig.Container.Resolve<IAuthorizationService>().IsAuthorized(request);
        }
    }
}