using MailKit.Net.Smtp;
using MimeKit;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

// Jarvis logik för att matcha användare med rätt Avenger-roll
public static class JarvisRoleMatcher
{
    // Skriver ut meddelande från Jarvis med röd färg
    public static void JarvisWrite(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Jarvis: {message}");
        Console.ResetColor();
    }

    // Frågar användaren och föreslår en roll baserat på svar
    public static string SuggestRole()
    {
        JarvisWrite("Greetings, new Avenger. Please answer the questions so we can determine the best matching role for you.");

        Console.WriteLine("Are you more of a (1) Leader or (2) Lone Wolf?");
        string q1 = Console.ReadLine()?.Trim();

        Console.WriteLine("Do you prefer (1) Brains or (2) Brawn?");
        string q2 = Console.ReadLine()?.Trim();

        Console.WriteLine("Would you rather use (1) Magic, (2) Gadgets, or (3) Strength?");
        string q3 = Console.ReadLine()?.Trim();

        // Bestämmer roll baserat på svaren
        string suggestedRole = MatchRole(q1, q2, q3);

        // Om Iron Man redan är vald, föreslå Doctor Strange istället
        if (suggestedRole == "Iron Man")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Jarvis: Excellent choice... however, Iron Man is already taken.");
            Console.WriteLine("Let’s find another hero with similar brilliance...");
            suggestedRole = "Doctor Strange";
            Console.ResetColor();
        }

        JarvisWrite($"Based on your answers, you should be {suggestedRole}!");

        return suggestedRole;
    }

    // Matchar svaren till en specifik Avenger-roll
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

// Jarvis AI som genererar missions baserat på hjälte
public static class JarvisMissionFinder
{
    // Returnerar en lista med mission-förslag från Jarvis
    public static async Task<List<string>> GetMissionSuggestion(string heroName, int missionCount)
    {
        var jarvis = new JarvisChat();

        // Skapar prompt till AI
        string prompt = $"Generate {missionCount} short and creative missions for the Avenger {heroName}. " +
                        "Each mission should fit their abilities and personality, and be written in one or two sentences. " +
                        "List each mission on a new line starting with 1., 2., etc.";

        // Får svar från AI
        string result = await jarvis.AskJarvisAsync(prompt);

        // Delar upp svar i rader och tar bort tomma
        var lines = result.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(l => l.Trim())
                          .Where(l => !string.IsNullOrWhiteSpace(l))
                          .ToList();

        // Extraherar texten på missions som börjar med nummer
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

// Jarvis notifierar användaren via email om nya missions
public static class JarvisNotifier
{
    public static void SendEmailNotification(string toEmail, string subject, string body)
    {
        // Laddar miljövariabler för email
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        string fromEmail = config["JARVIS_EMAIL"];
        string appPassword = config["JARVIS_APP_PASSWORD"];

        // Kontrollerar att credentials finns
        if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(appPassword))
        {
            Console.WriteLine("Jarvis: Missing email credentials. Cannot send email!");
            return;
        }

        // Skapar email-meddelande
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Jarvis", fromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        // Skickar email via SMTP
        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        client.Authenticate(fromEmail, appPassword);
        client.Send(message);
        client.Disconnect(true);

        // Bekräftar skickat mail
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Jarvis: Notification email sent successfully!");
        Console.ResetColor();
    }
}