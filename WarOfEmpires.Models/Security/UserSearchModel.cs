using System.ComponentModel;

namespace WarOfEmpires.Models.Security {
    public sealed class UserSearchModel {
        [DisplayName("Display name")]
        public string DisplayName { get; set; }
    }
}