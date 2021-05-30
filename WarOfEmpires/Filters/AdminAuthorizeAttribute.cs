using Microsoft.AspNetCore.Mvc;

namespace WarOfEmpires.Filters {
    public sealed class AdminAuthorizeAttribute : TypeFilterAttribute {
        public AdminAuthorizeAttribute() : base(typeof(AdminAuthorizeFilter)) { }
    }
}