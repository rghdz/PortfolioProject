using Twilio;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;
using Microsoft.Extensions.Configuration;



public class AvengersProfile
{
    public static List<AvengersProfile> registeredUsers = new List<AvengersProfile>()
    {
        new AvengersProfile("Iron Man", "Ironman22!", "+46720462003", " ")
    };
    public string Username;
    private string Password;
    public string TwoFactorAuthen;
    public string Email;
    public AvengersProfile(string username, string password, string twoFactorAuthen, string email)
    {
        Username = username;
        Password = password;
        TwoFactorAuthen = twoFactorAuthen;
        Email = email;
    }

public static async Task CreateUsernameAsync()
{
    Console.WriteLine("Before you choose an Avenger identity, Jarvis will analyze your personality...");
    string suggestedRole = JarvisRoleMatcher.SuggestRole();

    var jarvis = new JarvisChat();
    jarvis.PrintJarvis($"You are registering as {suggestedRole}!");

    string username = suggestedRole;

    // Jarvis ger kort och kul kommentar
    string aiResponse = await jarvis.AskJarvisAsync($"User chose the role {username}. Give a short, funny, inspiring comment as Jarvis in English.");
    jarvis.PrintJarvis(aiResponse);

    // Lösenord
    Console.WriteLine($"Welcome {username}, now we continue to password!");
    string password = CreatePassword();

    // Telefonnummer + 2FA
    string phoneNumber;
    do
    {
        Console.WriteLine("Enter your phone number with +46 to get a verification code please:");
        phoneNumber = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(phoneNumber))
            Console.WriteLine("You need to write in your phone number to continue.");
    } while (string.IsNullOrWhiteSpace(phoneNumber));

    bool verified = TwoFactorCheck(phoneNumber);
    if (!verified)
    {
        Console.WriteLine("Phone number verification failed..");
        return;
    }

    // 🔹 Fråga efter e-postadress
    string email;
    do
    {
        Console.WriteLine("Enter your email address (for mission updates):");
        email = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(email))
            Console.WriteLine("You must provide a valid email to receive mission updates.");
    } while (string.IsNullOrWhiteSpace(email));

    // Lägg till användaren i listan
    registeredUsers.Add(new AvengersProfile(username, password, phoneNumber, email));
    Console.WriteLine($"Congrats! Avenger created successfully, {username}!");
}
private static string CreatePassword()
{
    while (true)
    {
        Console.WriteLine("Choose a strong password to compelete your registration: ");
        string passW = Console.ReadLine();

        bool valid = passW.Length >= 6 &&
                     passW.Any(char.IsUpper) &&
                     passW.Any(char.IsLower) &&
                     passW.Any(char.IsDigit) &&
                     passW.Any(c => "!@#$%^&*()_+-=[]{}.<>/?-".Contains(c));

        if (valid)
        {
            Console.WriteLine("Thank you, Your password is strong enough!");
            return passW;
        }

        Console.WriteLine("Not strong enough please try again!");
        }
    }

public static bool TwoFactorCheck(string phoneNumber)
{
    var config = new ConfigurationBuilder()
        .AddUserSecrets<Program>() // laddar från user secrets
        .AddEnvironmentVariables() // laddar systemets miljövariabler
        .Build();

        // Hämta värden
        string accountSid = config["TWILIO_ACCOUNT_SID"];
        string authToken = config["TWILIO_AUTH_TOKEN"];
        string verifySid = config["TWILIO_VERIFY_SERVICE_SID"];

        // Felskydd
        if (string.IsNullOrWhiteSpace(accountSid) ||
            string.IsNullOrWhiteSpace(authToken) ||
            string.IsNullOrWhiteSpace(verifySid))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Missing Twilio credentials! Make sure they're added via User Secrets or .env.");
            Console.ResetColor();
            return false;
        }

        // Initiera Twilio
        TwilioClient.Init(accountSid, authToken);

        // Skicka SMS
        var verification = VerificationResource.Create(
            to: phoneNumber,
            channel: "sms",
            pathServiceSid: verifySid
        );

        Console.WriteLine($"✅ Code sent to {phoneNumber}. Enter the code:");
        string code = Console.ReadLine();

        // Kontrollera koden
        var check = VerificationCheckResource.Create(
            to: phoneNumber,
            code: code,
            pathServiceSid: verifySid
        );

        if (check.Status.Equals("approved", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("🎉 Phone number verified!");
            return true;
        }
        else
        {
            Console.WriteLine("❌ Wrong code!");
            return false;
        }
    }

    public static AvengersProfile LoggedIn()
    {
        Console.WriteLine("Login to your Avenger Profile: ");
        Console.WriteLine("Avenegr: ");
        string username = Console.ReadLine();

        Console.WriteLine("Password: ");
        string password = Console.ReadLine();

        var user = registeredUsers.FirstOrDefault(findUser => findUser.Username == username && findUser.Password == password);
        
        if (user != null)
        {
            Console.WriteLine($"Welcome back, {username}!");
            return user;
        }
        Console.WriteLine("Avenger not found. Please register first.");
        return null;      
    }
    public static void ShowMainMenu()
    {
        Console.WriteLine("AVENGER HERO SYSTEM");
        Console.WriteLine("1. Register as a new Avenger");
        Console.WriteLine("2. Log in");
        Console.WriteLine("7. Exit");
        Console.Write("Choose an option: ");
    }
}