using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using dotenv.net;

public class JarvisChat
{
    private static readonly HttpClient client = new HttpClient();
    private readonly string apiKey;

    public JarvisChat()
    {
        DotEnv.Load();
        apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            ?? throw new Exception("OPENAI_API_KEY not found");
    }

    public async Task<string> AskJarvisAsync(string userMessage)
    {
        var requestData = new
        {
            model = "gpt-4o-mini", // eller "gpt-4o" om du vill ha full version
            messages = new[]
            {
                new { role = "system", content = "You are Jarvis, a witty and intelligent AI assistant who helps users discover their ideal Avenger role and find suitable missions. Always respond with charm and confidence." },
                new { role = "user", content = userMessage }
            }
        };

        var json = JsonSerializer.Serialize(requestData);
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        request.Headers.Add("Authorization", $"Bearer {apiKey}");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseBody);
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return content;
    }
    public void PrintJarvis(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Jarvis: {message}");
        Console.ResetColor();
    }
}