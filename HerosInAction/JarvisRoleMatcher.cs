using MailKit.Net.Smtp;
using MimeKit;
using dotenv.net;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


public static class JarvisRoleMatcher
{
    public static void JarvisWrite(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Jarvis: {message}");
        Console.ResetColor();
    }

    public static string SuggestRole()
    {
        JarvisWrite("Greetings, new Avenger. Please answer the questions so we can determine the best matching role for you.");

        Console.WriteLine("Are you more of a (1) Leader or (2) Lone Wolf?");
        string q1 = Console.ReadLine()?.Trim();

        Console.WriteLine("Do you prefer (1) Brains or (2) Brawn?");
        string q2 = Console.ReadLine()?.Trim();

        Console.WriteLine("Would you rather use (1) Magic, (2) Gadgets, or (3) Strength?");
        string q3 = Console.ReadLine()?.Trim();

        string suggestedRole = MatchRole(q1, q2, q3);

        if (suggestedRole == "Iron Man")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("🦾 Jarvis: Excellent choice... however, Iron Man is already taken.");
            Console.WriteLine("Let’s find another hero with similar brilliance...");
            suggestedRole = "Doctor Strange";
            Console.ResetColor();
        }

        JarvisWrite($"Based on your answers, you should be {suggestedRole}!");

        return suggestedRole;
    }

    private static string MatchRole(string q1, string q2, string q3)
    {
        if (q1 == "1" && q2 == "1" && q3 == "2") return "Iron Man";
        if (q1 == "1" && q2 == "2") return "Captain America";
        if (q1 == "2" && q2 == "2" && q3 == "3") return "Hulk";
        if (q3 == "1") return "Doctor Strange";
        if (q3 == "2") return "Spider Man";
        if (q1 == "2" && q2 == "1") return "Black Widow";
        if (q1 == "1" && q3 == "3") return "Thor";
        return "Hawkeye";
    }
}

public static class JarvisMissionFinder
{
    public static async Task<List<string>> GetMissionSuggestion(string heroName, int missionCount)
    {
        var jarvis = new JarvisChat();

        string prompt = $"Generate {missionCount} short and creative missions for the Avenger {heroName}. " +
                        "Each mission should fit their abilities and personality, and be written in one or two sentences. " +
                        "List each mission on a new line starting with 1., 2., etc.";

        string result = await jarvis.AskJarvisAsync(prompt);

        // Split lines och ta bort tomma rader
        var lines = result.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(l => l.Trim())
                          .Where(l => !string.IsNullOrWhiteSpace(l))
                          .ToList();

        // Extrahera de faktiska missions som börjar med nummer
        var missions = new List<string>();
        var regex = new Regex(@"^\d+\.\s*(.*)$"); // matchar "1. Mission text"
        foreach (var line in lines)
        {
            var match = regex.Match(line);
            if (match.Success)
            {
                missions.Add(match.Groups[1].Value.Trim());
            }
        }

        return missions;
    }
}

public static class JarvisNotifier
{
    public static void SendEmailNotification(string toEmail, string subject, string body)
    {
        // 🔹 Konfigurera User Secrets + miljövariabler
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()   // laddar user secrets
            .AddEnvironmentVariables()   // laddar systemets miljövariabler
            .Build();

        string fromEmail = config["JARVIS_EMAIL"];
        string appPassword = config["JARVIS_APP_PASSWORD"];

        Console.WriteLine($"fromEmail={fromEmail}");
        Console.WriteLine($"appPassword={(string.IsNullOrEmpty(appPassword) ? "NOT FOUND" : "FOUND")}");

        if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(appPassword))
        {
            Console.WriteLine("⚠️ Jarvis: Missing email credentials. Cannot send email!");
            return;
        }

        // Skapa mejlet
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Jarvis", fromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        // Anslut till SMTP-server
        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        client.Authenticate(fromEmail, appPassword);  

        client.Send(message);
        client.Disconnect(true);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Jarvis: Notification email sent successfully!");
        Console.ResetColor();
    }
}