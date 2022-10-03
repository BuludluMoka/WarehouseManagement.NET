namespace Warehouse.Core.Services.EmailService
{
    interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
