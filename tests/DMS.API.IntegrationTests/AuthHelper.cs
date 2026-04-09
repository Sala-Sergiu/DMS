using System.Net.Http.Json;

namespace DMS.API.IntegrationTests;

public static class AuthHelper
{
    private const string Email = "integration@dms.com";
    private const string Password = "Test@123456";

    public static async Task<string> GetTokenAsync(HttpClient client)
    {
        // Încearcă înregistrarea — ignoră 409 dacă userul există deja
        await client.PostAsJsonAsync("/api/auth/register", new
        {
            fullName = "Integration Test User",
            email = Email,
            password = Password,
            location = "Test City"
        });

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email = Email,
            password = Password
        });

        if (!loginResponse.IsSuccessStatusCode)
        {
            var error = await loginResponse.Content.ReadAsStringAsync();
            throw new InvalidOperationException(
                $"Login failed in test. Status: {loginResponse.StatusCode}. Body: {error}");
        }

        var result = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();
        return result?.Token ?? throw new InvalidOperationException("Login response did not contain a token.");
    }

    private sealed record LoginResult(string Token, DateTime ExpiresAt);
}