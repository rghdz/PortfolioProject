using Twilio;
using Twilio.Rest.Verify.V2.Service;
using Microsoft.Extensions.Configuration;


public class AvengersProfile
{
    public static List<AvengersProfile> registeredUsers = new List<AvengersProfile>()
    {
        new AvengersProfile("Iron Man", "Ironman22!", "+46720462003")
    };
    public string Username;
    private string Password;
    public string TwoFactorAuthen;
    public AvengersProfile(string username, string password, string twoFactorAuthen)
    {
        Username = username;
        Password = password;
        TwoFactorAuthen = twoFactorAuthen;
    }

    public static async Task CreateUsernameAsync()
    {
        Console.WriteLine("Before you choose an Avenger identity, Jarvis will analyze your personality...");
        string suggestedRole = JarvisRoleMatcher.SuggestRole(); // <-- Ny metod (läggs till i separat fil)

        Console.WriteLine($"\nDo you want to register as {suggestedRole}? (y/n)");
        string answer = Console.ReadLine()?.ToLower();

        string username;

        if (answer == "y")
        {
            // Om användaren accepterar Jarvis förslag direkt
            username = suggestedRole;
        }
        else
        {
            // 🧠 Annars går vi vidare till din gamla kod för manuell rollval
            List<string> avengerRoles = new List<string>
            {
                "Iron Man",
                "Thor",
                "Captain America",
                "Hulk",
                "Black Widow",
                "Spider Man",
                "Hawkeye",
                "Doctor Strange"
            };

            username = null;
            while (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Available Avengers: ");
                for (int i = 0; i < avengerRoles.Count; i++)
                {
                    bool taken = registeredUsers.Any(u => u.Username.Equals(avengerRoles[i], StringComparison.OrdinalIgnoreCase));
                    Console.WriteLine($"{i + 1}. {avengerRoles[i]} {(taken ? "- Taken" : " ")}");
                }

                Console.WriteLine("Enter the number of which Avenger you want to be: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice) || choice < 1 || choice > avengerRoles.Count)
                {
                    Console.WriteLine("Invalid choice, try again!");
                    continue;
                }

                string chosenAvenger = avengerRoles[choice - 1];
                bool alreadyTaken = registeredUsers.Any(u => u.Username.Equals(chosenAvenger, StringComparison.OrdinalIgnoreCase));

                if (alreadyTaken)
                {
                    Console.WriteLine($"Sorry {chosenAvenger} is already taken! Choose another one!");
                }
                else
                {
                    username = chosenAvenger;
                }

            }
        }
        var jarvis = new JarvisChat();
        string aiResponse = await jarvis.AskJarvisAsync($"Användaren valde rollen {username}. Ge en kort, rolig och inspirerande kommentar som Jarvis, på svenska.");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Jarvis (AI): {aiResponse}");
        Console.ResetColor();
        
        
        Console.WriteLine($"Welcome {username}, now we continue to password!");
        string password = CreatePassword();

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

        registeredUsers.Add(new AvengersProfile(username, password, phoneNumber));
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