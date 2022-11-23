namespace WarOfEmpires.Models.Security {
    public sealed class ConfirmUserEmailChangeModel {
        public string Email { get; set; }
        public string ConfirmationCode { get; set; }
    }
}