namespace Insurance.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendCredentialsAsync(string toEmail, string password, string role);
    }
}