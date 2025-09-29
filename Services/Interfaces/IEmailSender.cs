namespace AuthServices.Services.Interfaces;

public interface IEmailSender
{
    Task SendResetLinkAsync(string email, string url);
    Task SendResetSuccessAsync(string email);
}