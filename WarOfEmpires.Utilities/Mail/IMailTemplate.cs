namespace WarOfEmpires.Utilities.Mail {
    public interface IMailTemplate<TMailTemplateParameters> {
        MailMessage GetMessage(TMailTemplateParameters parameters, string to);
    }
}