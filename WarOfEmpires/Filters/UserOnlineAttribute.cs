using Microsoft.AspNetCore.Mvc;

namespace WarOfEmpires.Filters {
    public sealed class UserOnlineAttribute : TypeFilterAttribute {
        public UserOnlineAttribute() : base(typeof(UserOnlineFilter)) { }
    }
}
