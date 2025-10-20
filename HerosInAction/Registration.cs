using MailKit.Net.Smtp;
using System.Net;
using System.Net.Mail;
using dotenv.net;
public class AvengersProfile
{
    // Lista med alla registrerade användare
    public static List<AvengersProfile> registeredUsers = new List<AvengersProfile>()
    {
        new AvengersProfile("Iron Man", "Ironman22!", "+46720462003", " ")
    };

    public string Username;
    private string Password;
    public string TwoFactorAuthen;
    public string Email;

    // Konstruktor för att skapa en hjälte
    public AvengersProfile(string username, string password, string twoFactorAuthen, string email)
    {
        Username = username;
        Password = password;
        TwoFactorAuthen = twoFactorAuthen;
        Email = email;
    }

    // Registrering av ny hjälte med Jarvis
   public static async Task CreateUsernameAsync()
{
    Console.WriteLine("Before you choose an Avenger identity, Jarvis will analyze your personality...");

    // Jarvis föreslår en roll baserat på användarens svar
    string suggestedRole = JarvisRoleMatcher.SuggestRole();

    var jarvis = new JarvisChat();
    jarvis.PrintJarvis($"You are registering as {suggestedRole}!");

    string username = suggestedRole;

    // Jarvis svarar med en kort AI-genererad kommentar
    string aiResponse = await jarvis.AskJarvisAsync(
        $"User chose the role {username}. Give a short, funny, inspiring comment as Jarvis in English."
    );
    jarvis.PrintJarvis(aiResponse);

    // Skapar lösenord
    Console.WriteLine($"Welcome {username}, now we continue to password!");
    string password = CreatePassword();

    // Frågar efter e-postadress för verifiering och framtida notiser
    string email;
    do
    {
        Console.WriteLine("Enter your email address (for verification and mission updates):");
        email = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(email))
            Console.WriteLine("You must provide a valid email to continue.");
    } 
    while (string.IsNullOrWhiteSpace(email));

    // Jarvis skickar verifieringskod via e-post (2FA)
    bool verified = TwoFactorCheck(email);
    if (!verified)
    {
        Console.WriteLine("Email verification failed. Registration aborted.");
        return;
    }

    // Lägger till hjälten i listan med registrerade användare
    registeredUsers.Add(new AvengersProfile(username, password, null, email));
    Console.WriteLine($"Congrats! Avenger created successfully, {username}!");

    // Nollställer hjältepoäng i MissionManagement om de inte finns
    if (!MissionManagement.heroPoints.ContainsKey(username))
    {
        MissionManagement.heroPoints[username] = 0;
    }
}

    // Skapar och kontrollerar starkt lösenord
    private static string CreatePassword()
    {
        while (true)
        {
            Console.WriteLine("Choose a strong password to complete your registration: ");
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

    // Metod för att skicka en verifieringskod via e-post (ersätter gamla SMS-baserade 2FA)
    public static bool TwoFactorCheck(string email)
    {
        DotEnv.Load();

        // Hämtar inloggningsuppgifter från miljövariabler (samma som används för notiser)
        string fromEmail = Environment.GetEnvironmentVariable("JARVIS_EMAIL");
        string appPassword = Environment.GetEnvironmentVariable("JARVIS_APP_PASSWORD");

        // Skapar en slumpad 6-siffrig kod som används som verifieringskod
        var random = new Random();
        string verificationCode = random.Next(100000, 999999).ToString();

        // Skapar en SMTP-klient som kopplar upp sig mot Gmails e-postserver
        var smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromEmail, appPassword),
            EnableSsl = true
        };

        // Bygger själva mejlet som skickas till användaren
        var message = new MailMessage(fromEmail, email)
        {
            Subject = "Jarvis Two-Factor Verification",
            Body = $"Hello hero!\n\nYour verification code is: {verificationCode}\n\nStay safe.\n– Jarvis"
        };

        // Skickar verifieringsmejlet
        smtp.Send(message);

        // Jarvis meddelar användaren i röd text
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Jarvis: A verification code has been sent to {email}");
        Console.ResetColor();

        // Användaren skriver in koden som skickades till mejlen
        Console.Write("Enter the verification code: ");
        string inputCode = Console.ReadLine();

        // Om användaren skrev rätt kod, godkänns verifieringen
        if (inputCode == verificationCode)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Jarvis: Verification successful. Welcome back, hero!");
            Console.ResetColor();
            return true;
        }

        // Om koden inte stämmer
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Jarvis: Incorrect verification code. Access denied.");
        Console.ResetColor();
        return false;
    }

    // // Tvåfaktorsverifiering via Twilio
    // public static bool TwoFactorCheck(string phoneNumber)
    // {
    //     var config = new ConfigurationBuilder()
    //         .AddUserSecrets<Program>()
    //         .AddEnvironmentVariables()
    //         .Build();

    //     string accountSid = config["TWILIO_ACCOUNT_SID"];
    //     string authToken = config["TWILIO_AUTH_TOKEN"];
    //     string verifySid = config["TWILIO_VERIFY_SERVICE_SID"];

    //     if (string.IsNullOrWhiteSpace(accountSid) ||
    //         string.IsNullOrWhiteSpace(authToken) ||
    //         string.IsNullOrWhiteSpace(verifySid))
    //     {
    //         Console.ForegroundColor = ConsoleColor.Red;
    //         Console.WriteLine("Missing Twilio credentials! Make sure they're added via User Secrets or .env.");
    //         Console.ResetColor();
    //         return false;
    //     }

    //     TwilioClient.Init(accountSid, authToken);

    //     var verification = VerificationResource.Create(
    //         to: phoneNumber,
    //         channel: "sms",
    //         pathServiceSid: verifySid
    //     );

    //     Console.WriteLine($"Code sent to {phoneNumber}. Enter the code:");
    //     string code = Console.ReadLine();

    //     var check = VerificationCheckResource.Create(
    //         to: phoneNumber,
    //         code: code,
    //         pathServiceSid: verifySid
    //     );

    //     if (check.Status.Equals("approved", StringComparison.OrdinalIgnoreCase))
    //     {
    //         Console.WriteLine("Phone number verified!");
    //         return true;
    //     }
    //     else
    //     {
    //         Console.WriteLine("Wrong code!");
    //         return false;
    //     }
    // }

    // Logga in befintlig hjälte
    public static AvengersProfile LoggedIn()
    {
        Console.WriteLine("Login to your Avenger Profile: ");
        Console.WriteLine("Avenger: ");
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

    // Visar huvudmenyn
    public static void ShowMainMenu()
    {
        Console.WriteLine("AVENGER HERO SYSTEM");
        Console.WriteLine("1. Register as a new Avenger");
        Console.WriteLine("2. Log in");
        Console.WriteLine("3. Exit");
        Console.Write("Choose an option: ");
    }
}