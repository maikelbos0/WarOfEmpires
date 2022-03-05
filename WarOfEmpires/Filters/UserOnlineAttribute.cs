using Microsoft.AspNetCore.Mvc;

namespace WarOfEmpires.Filters {
    // TODO move to middleware
    public sealed class UserOnlineAttribute : TypeFilterAttribute {
        public UserOnlineAttribute() : base(typeof(UserOnlineFilter)) { }
    }
}
