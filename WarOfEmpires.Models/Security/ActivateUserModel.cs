namespace WarOfEmpires.Models.Security {
    public sealed class ActivateUserModel {
        public string Email { get; set; }
        public string ActivationCode { get; set; }
    }
}