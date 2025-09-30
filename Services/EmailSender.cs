using AuthServices.Services.Interfaces;

namespace AuthServices.Services;

public class EmailSender(IHttpClientFactory httpClientFactory) : IEmailSender
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task SendResetLinkAsync(string email, string url)
    {
        var client = _httpClientFactory.CreateClient("EmailServiceProvider");

        var payload = new { Email = email, Url = url};

        var resp = await client.PostAsJsonAsync("api/PasswordReset/send-reset-email", payload);
        resp.EnsureSuccessStatusCode();
    }


    public async Task SendResetSuccessAsync(string email)
    {
        var client = _httpClientFactory.CreateClient("EmailServiceProvider");

        var resp = await client.PostAsJsonAsync("api/PasswordReset/send-confirmation-email", email);
        resp.EnsureSuccessStatusCode();
    }
}