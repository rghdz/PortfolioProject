using dotenv.net;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

public class AvengersProfile
{
    public static List<AvengersProfile> registeredUsers = new List<AvengersProfile>()
    {
        new AvengersProfile("Iron Man", "Ironman22!", "0948857842")
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

    public static void CreateUsername()
    {
        Console.WriteLine("Create your Avenger Profile to begin: ");
        Console.WriteLine("Choose which Avenger you want to be (e.g. Iron Man, Thor, Captain America, etc.) ");

        string username;
        do
        {
            Console.WriteLine("Please enter your avenger name: ");
             username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
                Console.WriteLine("Username cannot be empty! Try again!");

            else if (registeredUsers.Any(character => character.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Sorry the avenger '{username}' you chose is taken right now! Try with another avenger!");
                username = null;
            }

        } while (string.IsNullOrWhiteSpace(username));
        Console.WriteLine($"Welcome {username}, now we continue to password! ");
        string password = CreatePassword();

        string phoneNumber;
        do
        {
            Console.WriteLine("Enter you phone number with +46 to get a verification code please: ");
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
        Console.WriteLine($"Congrats! Avenger created succesfully, {username}!");
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
        DotEnv.Load();
    
        string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID")
            ?? throw new Exception("TWILIO_ACCOUNT_SID not set. Did you add it to User Secrets?");
        string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN")
            ?? throw new Exception("TWILIO_AUTH_TOKEN not set. Did you add it to User Secrets?");
        string verifySid = Environment.GetEnvironmentVariable("TWILIO_VERIFY_SERVICE_SID")
            ?? throw new Exception("TWILIO_VERIFY_SERVICE_SID not set. Did you add it to User Secrets?");

        if (string.IsNullOrWhiteSpace(accountSid) ||
            string.IsNullOrWhiteSpace(authToken) ||
            string.IsNullOrWhiteSpace(verifySid))
        {
            Console.WriteLine("Twilio credentials are missing. Please check your .env file.");
            return false;
        }

        TwilioClient.Init(accountSid, authToken);

        var verification = VerificationResource.Create(
            to: phoneNumber,
            channel: "sms",
            pathServiceSid: verifySid
        );
        Console.WriteLine($"A code was sent to your phone number {phoneNumber}!");
        Console.WriteLine("Write in the code here please!");
        string code = Console.ReadLine();

        var check = VerificationCheckResource.Create(
            to: phoneNumber,
            code: code,
            pathServiceSid: verifySid
        );

        if (check.Status == "approved" || check.Status == "Approved")
        {
            Console.WriteLine("Phone number was verified!");
            return true;
        }
        else
        {
            Console.WriteLine("Wrong code!");
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