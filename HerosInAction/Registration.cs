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
        Console.WriteLine("Please enter your avenger name: ");
        string username = Console.ReadLine();

        if (registeredUsers.Any(character => character.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine($"Sorry the avenger '{username}' you chose is taken right now! Try with another avenger!");
            return;
        }

        Console.WriteLine($"Welcome {username}, now we continue to password! ");
        string password = CreatePassword();

        Console.WriteLine("Enter you phone number to get a verification code please: ");
        string twoFacAu = Console.ReadLine();

        registeredUsers.Add(new AvengersProfile(username, password, twoFacAu));
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
    
    public void TwoFactorCheck()
    {
        Console.WriteLine("Please provide your email adress or phone number for two-factor authentication: ");
        TwoFactorAuthen = Console.ReadLine();
        Console.WriteLine($"Two-factor authentication set with {TwoFactorAuthen}.");
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