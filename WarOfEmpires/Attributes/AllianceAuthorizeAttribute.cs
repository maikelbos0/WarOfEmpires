﻿using System.Net;
using System.Web.Mvc;
using Unity;
using WarOfEmpires.Services;

namespace WarOfEmpires.Attributes {
    public sealed class AllianceAuthorizeAttribute : ActionFilterAttribute, IAllianceAuthorizeAttribute {
        public bool CanInvite { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (!UnityConfig.Container.Resolve<IAuthorizationService>().IsAuthorized(this)) {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }
    }
}