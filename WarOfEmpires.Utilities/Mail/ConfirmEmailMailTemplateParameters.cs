namespace WarOfEmpires.Utilities.Mail {
    public sealed class ConfirmEmailMailTemplateParameters {
        public string Email { get; }
        public int ActivationCode { get; }

        public ConfirmEmailMailTemplateParameters(string email, int activationCode) {
            Email = email;
            ActivationCode = activationCode;
        }
    }
}