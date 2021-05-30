using Microsoft.AspNetCore.Mvc;

namespace WarOfEmpires.Filters {
    public class UserOnlineAttribute : TypeFilterAttribute {
        public UserOnlineAttribute() : base(typeof(UserOnlineFilter)) { }
    }
}
