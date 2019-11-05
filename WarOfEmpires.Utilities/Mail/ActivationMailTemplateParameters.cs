namespace WarOfEmpires.Utilities.Mail {
    public sealed class ActivationMailTemplateParameters {
        public string Email { get; }
        public int ActivationCode { get; }

        public ActivationMailTemplateParameters(string email, int activationCode) {
            Email = email;
            ActivationCode = activationCode;
        }
    }
}