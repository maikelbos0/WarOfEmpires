using System.Net;
using WarOfEmpires.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WarOfEmpires.Filters {
    public sealed class AdminAuthorizeFilter : IAuthorizationFilter {
        private readonly IAuthorizationService _authorizationService;

        public AdminAuthorizeFilter(IAuthorizationService authorizationService) {
            _authorizationService = authorizationService;
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            if (!_authorizationService.IsAdmin()) {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }
    }
}