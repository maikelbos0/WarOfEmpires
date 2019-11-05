namespace WarOfEmpires.Utilities.Mail {
    public interface IMailClient {
        void Send(MailMessage message);
    }
}