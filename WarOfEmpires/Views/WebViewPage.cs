using System;
using Unity;
using WarOfEmpires.Services;

namespace WarOfEmpires.Views {
    // TODO determine if this is still the way to add custom services
    public abstract class WebViewPage<TModel> { //: System.Web.Mvc.WebViewPage<TModel> {
        private Lazy<IAuthorizationService> _authorizationService = new Lazy<IAuthorizationService>(() => UnityConfig.Container.Resolve<IAuthorizationService>());

        public IAuthorizationService AuthorizationService => _authorizationService.Value;
    }
}