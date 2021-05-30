using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using WarOfEmpires.Services;

namespace WarOfEmpires.Filters {
    public class AllianceAuthorizeFilter : IAuthorizationFilter {
        private readonly IAllianceAuthorizationRequest _allianceAuthorizationRequest;
        private readonly IAuthorizationService _authorizationService;

        public AllianceAuthorizeFilter(IAllianceAuthorizationRequest allianceAuthorizationRequest, IAuthorizationService authorizationService) {
            _allianceAuthorizationRequest = allianceAuthorizationRequest;
            _authorizationService = authorizationService;
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            if (!_authorizationService.IsAuthorized(_allianceAuthorizationRequest)) {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }
    }
}
