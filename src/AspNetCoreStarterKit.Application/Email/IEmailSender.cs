using System.Threading.Tasks;

namespace AspNetCoreStarterKit.Application.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}