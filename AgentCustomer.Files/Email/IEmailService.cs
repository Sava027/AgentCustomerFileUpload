namespace AgentCustomer.Files
{
    public interface IEmailService {
        void SendNotification(string to, string subject, string html, string from = null);
    }
}
