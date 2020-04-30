using System.ComponentModel;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceSearchModel {
        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }
    }
}