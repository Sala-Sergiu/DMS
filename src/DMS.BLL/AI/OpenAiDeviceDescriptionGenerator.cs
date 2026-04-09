using DMS.BLL.DTOs.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace DMS.BLL.AI;

public sealed class OpenAiDeviceDescriptionGenerator : IDeviceDescriptionGenerator
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OpenAiDeviceDescriptionGenerator> _logger;

    public OpenAiDeviceDescriptionGenerator(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<OpenAiDeviceDescriptionGenerator> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GenerateAsync(
        GenerateDescriptionRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["OpenAi:ApiKey"]
            ?? throw new InvalidOperationException("OpenAI API key is not configured.");

        var model = _configuration["OpenAi:Model"] ?? "gpt-4o-mini";

        var prompt = BuildPrompt(request);

        var payload = new
        {
            model,
            messages = new[]
            {
                new { role = "system", content = "You are a technical writer for a corporate IT asset management system. Write concise, professional device descriptions." },
                new { role = "user", content = prompt }
            },
            max_tokens = 120,
            temperature = 0.4
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");
        httpRequest.Content = JsonContent.Create(payload);

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reach OpenAI API.");
            throw new ApplicationException("AI description service is currently unavailable.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("OpenAI returned {StatusCode}: {Error}", response.StatusCode, error);
            throw new ApplicationException("AI description generation failed. Please try again later.");
        }

        var result = await response.Content.ReadFromJsonAsync<OpenAiResponse>(
            cancellationToken: cancellationToken);

        var content = result?.Choices?.FirstOrDefault()?.Message?.Content;

        if (string.IsNullOrWhiteSpace(content))
        {
            _logger.LogWarning("OpenAI returned an empty description.");
            throw new ApplicationException("AI returned an empty description.");
        }

        return content.Trim();
    }

    private static string BuildPrompt(GenerateDescriptionRequestDto request)
    {
        var parts = new List<string>
        {
            $"Device: {request.Brand} {request.Model} ({request.Name})",
            $"Type: {request.Type}"
        };

        if (!string.IsNullOrWhiteSpace(request.OperatingSystem))
            parts.Add($"OS: {request.OperatingSystem}");

        if (!string.IsNullOrWhiteSpace(request.Processor))
            parts.Add($"Processor: {request.Processor}");

        if (request.RamGb.HasValue)
            parts.Add($"RAM: {request.RamGb} GB");

        parts.Add("Write a single concise business-friendly description sentence for this device, suitable for an IT asset catalog.");

        return string.Join("\n", parts);
    }

    // Internal OpenAI response shape — not exposed outside this class
    private sealed class OpenAiResponse
    {
        [JsonPropertyName("choices")]
        public List<Choice>? Choices { get; init; }

        public sealed class Choice
        {
            [JsonPropertyName("message")]
            public Message? Message { get; init; }
        }

        public sealed class Message
        {
            [JsonPropertyName("content")]
            public string? Content { get; init; }
        }
    }
}