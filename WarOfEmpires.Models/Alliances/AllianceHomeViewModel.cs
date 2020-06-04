using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarOfEmpires.Models.Alliances {
    public sealed class AllianceHomeViewModel : EntityViewModel {
        public string Code { get; set; }
        public string Name { get; set; }
        public int LeaderId { get; set; }
        public string Leader { get; set; }
        public List<AllianceHomeMemberViewModel> Members { get; set; }
        public List<ChatMessageViewModel> ChatMessages { get; set; }
        [DisplayName("Message")]
        [Required(ErrorMessage = "Message is required")]
        public string ChatMessage { get; set; }
    }
}