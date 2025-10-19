using MailKit.Net.Smtp;
using MimeKit;
public class MissionManagement
{
    // Lista med alla mission i systemet
    public static List<MissionManagement> missions = new List<MissionManagement>()
    {
        new MissionManagement("Retrieve Infinity Stones before Thanos", "Tony Stark must Collect all Infinity stones from Hisingen, Vasa, Frölunda, Angered, Göteborg Centrum.", new DateTime(2025, 10, 20, 13, 0, 0), "High", "Iron Man"),
        new MissionManagement("Stop alien drones over Liseberg", "Thor must stop a fleet of small alien drones hovering over the amusement park Liseberg.", new DateTime(2025, 10, 18, 11, 0, 0), "High", "Thor"),
        new MissionManagement("Evacuate civillians from Nordstan", "Lead the evacuation of civilians from Nordstan during a alien invasion.", new DateTime(2025, 10, 5, 12, 0, 0), "High", "Captain America"),
        new MissionManagement("Crush Chitauri Leviathans at Slottskogen", "Hulk must deal with giant alien robots attacking on Slottskogen Park.", new DateTime(2025, 10, 8, 14, 0, 0), "High", "Hulk"),
        new MissionManagement("Protect NBI/Handelsakademin", "Defend NBI from the incoming enemy army.", new DateTime(2025, 10, 2, 15, 30, 0), "Medium", "Black Widow"),
        new MissionManagement("Investigate Strange Activity", "Investigate mysterious magical activity in Opera.", new DateTime(2025, 10, 15, 9, 0, 0), "Low", "Spider Man"),
        new MissionManagement("Escort VIPs", "Protect important civilians during evacuation.", new DateTime(2025, 10, 12, 14, 0, 0), "Medium", "Hawkeye"),
        new MissionManagement("Seal dimentional rift over Karla tower", "Strange must close a magical rift above the Karla tower before aliens come through.", new DateTime(2025, 1, 12, 1, 0, 0), "Medium", "Doctor Strange")
    };

    // Dictionary för att hålla poäng per hjälte
    public static Dictionary<string, int> heroPoints = new Dictionary<string, int>();

    // Hämtar uppdrag för en viss hjälte baserat på rollen (ex. Iron Man)
    // Hämtar första mission som matchar hjälten och som inte är completed
    public static MissionManagement? GetMissionForAvenger(string avengerRole)
    {
        if (string.IsNullOrWhiteSpace(avengerRole) || missions == null || missions.Count == 0)
            return null;

        string roleTrimmed = avengerRole.Trim();

        return missions.FirstOrDefault(m => !string.IsNullOrWhiteSpace(m.AvengerRole) &&
                                           m.AvengerRole.Trim().Equals(roleTrimmed, StringComparison.OrdinalIgnoreCase) &&
                                           !m.IsCompleted);
    }

    // Fält för ett mission
    public string Title;
    public string Description;
    public DateTime DueDate;
    public string Priority;
    public bool IsCompleted;
    public string AvengerRole;
    public string HeroNote;

    // Konstruktor för att skapa ett nytt mission
    public MissionManagement(string title, string description, DateTime dueDate, string priority, string avengerRole, bool isCompleted = false, string heroNote = "")
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        AvengerRole = avengerRole;
        IsCompleted = isCompleted;
        HeroNote = heroNote;
    }

    // Lägger till nytt mission manuellt
    public static void AddMission()
    {
        Console.WriteLine("Which mission would you like to add?");

        Console.WriteLine("Mission title: ");
        string title = Console.ReadLine();

        Console.WriteLine("Mission description: ");
        string description = Console.ReadLine();

        Console.WriteLine("Mission due date and time (yyyy-MM-dd HH:mm): ");
        string dueDateStr = Console.ReadLine();
        DateTime dueDate = DateTime.Parse(dueDateStr);

        Console.WriteLine("Mission priority (Low, Medium, High): ");
        string priority = Console.ReadLine();

        Console.WriteLine("Assign this mission to which Avenger? ");
        string avengerRole = Console.ReadLine();

        MissionManagement newMission = new MissionManagement(title, description, dueDate, priority, avengerRole, false);
        missions.Add(newMission);

        Console.WriteLine($"Mission '{title}' added successfully!!");
    }

    // Visar första mission för en hjälte
    public static void ShowMissionforAvenger(AvengersProfile hero)
    {
        var mission = GetMissionForAvenger(hero.Username);
        if (mission != null)
        {
            Console.WriteLine($" {hero.Username}:");
            Console.WriteLine($" {mission.Title}");
            Console.WriteLine($" {mission.Description}");
            Console.WriteLine($" {mission.DueDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($" {mission.Priority}");
            Console.WriteLine($" {(mission.IsCompleted ? "YES" : "NO")}");
        }
        else
        {
            Console.WriteLine("No mission assigned.");
        }
    }

    // Visar alla mission i systemet
    public static void ShowAllMissions()
    {
        if (missions.Count == 0)
        {
            Console.WriteLine("No missions available right now, but if you want to add a new one, please choose that option!");
            return;
        }

        Console.WriteLine("ALL MISSIONS: ");
        int index = 1;
        foreach (var mission in missions)
        {
            Console.WriteLine($"{index}. {mission.Title} | Assigned to: {mission.AvengerRole} | Due: {mission.DueDate:yyyy-MM-dd HH:mm} | Priority: {mission.Priority} | Completed: {(mission.IsCompleted ? "YES" : "NO")}");
            index++;
        }
        Console.WriteLine();
    }

    // Markerar mission som completed och ger poäng
    public static async Task CompleteMission(AvengersProfile hero)
    {
        // Hämta alla missions för hjälten som inte är completed
        var heroMissions = missions
            .Where(m => m.AvengerRole.Equals(hero.Username, StringComparison.OrdinalIgnoreCase) && !m.IsCompleted)
            .ToList();

        if (heroMissions.Count == 0)
        {
            JarvisHelper.Write("You have no missions to complete!");
            return;
        }

        // Lista alla missions med index
        Console.WriteLine("Here are your missions:");
        for (int i = 0; i < heroMissions.Count; i++)
        {
            var m = heroMissions[i];
            Console.WriteLine($"{i + 1}. {m.Title} | Priority: {m.Priority} | Due: {m.DueDate:yyyy-MM-dd HH:mm}");
        }

        int selectedIndex = -1;
        while (true)
        {
            JarvisHelper.Write("Which mission do you want to mark as completed? Enter the number:");
            string input = Console.ReadLine();
            if (int.TryParse(input, out selectedIndex) && selectedIndex >= 1 && selectedIndex <= heroMissions.Count)
            {
                selectedIndex -= 1; // 0-baserad lista
                break;
            }
            JarvisHelper.Write("Invalid choice. Please enter a valid number.");
        }

        var missionToComplete = heroMissions[selectedIndex];
        missionToComplete.IsCompleted = true;

        // Ge poäng baserat på priority
        int pointsAwarded = missionToComplete.Priority switch
        {
            "High" => 10,
            "Medium" => 5,
            "Low" => 2,
            _ => 0
        };

        if (!heroPoints.ContainsKey(hero.Username))
            heroPoints[hero.Username] = 0;

        heroPoints[hero.Username] += pointsAwarded;

        JarvisHelper.Write($"Mission '{missionToComplete.Title}' completed! You earned {pointsAwarded} points. Total points: {heroPoints[hero.Username]}");

        // Fråga om nya missions
        JarvisHelper.Write("Do you want me to generate new missions for you? (yes/no)");
        string answer = Console.ReadLine()?.Trim().ToLower();
        if (answer == "yes")
        {
            int count = 0;
            while (true)
            {
                JarvisHelper.Write("How many new missions would you like? (1-5)");
                string numInput = Console.ReadLine();
                if (int.TryParse(numInput, out count) && count >= 1 && count <= 5)
                    break;
                JarvisHelper.Write("Invalid number, please enter 1-5.");
            }

            // Hämta nya missions från AI
            var newMissions = await JarvisMissionFinder.GetMissionSuggestion(hero.Username, count);

            // Lägg till nya missions i listan, alla startar som incomplete
            foreach (var m in newMissions)
            {
                missions.Add(new MissionManagement(
                    title: m,
                    description: $"Mission for {hero.Username}",
                    dueDate: DateTime.Now.AddDays(3),
                    priority: "Medium",
                    avengerRole: hero.Username
                ));
            }

            JarvisHelper.Write($"I've generated {newMissions.Count} new missions for {hero.Username}!");

            // Visa nya missions
            foreach (var m in newMissions)
            {
                Console.WriteLine($"- {m}");
            }

            // Skicka email om användaren har email
            if (!string.IsNullOrWhiteSpace(hero.Email))
            {
                JarvisNotifier.SendEmailNotification(
                    hero.Email,
                    "New Missions from S.H.I.E.L.D.",
                    $"You have {newMissions.Count} new missions to complete. Check your S.H.I.E.L.D. account!"
                );
            }
        }
    }
    public static class JarvisHelper
    {
        // Allt Jarvis-meddelande går via denna metod → alltid rött
        public static void Write(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Jarvis: {message}");
            Console.ResetColor();
        }
    }
    // Uppdaterar anteckning för mission
    public static void UpdateMission(AvengersProfile hero)
    {
        var mission = GetMissionForAvenger(hero.Username);
        if (mission == null)
        {
            Console.WriteLine("You have no mission to update!");
            return;
        }
        Console.WriteLine($"Your current mission: {mission.Title}");
        if (!string.IsNullOrWhiteSpace(mission.HeroNote))
        {
            Console.WriteLine($"Current note: {mission.HeroNote}");
        }
        Console.WriteLine("Write your note here or leave blank:");
        string newNote = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newNote))
        {
            mission.HeroNote = newNote;
            Console.WriteLine("Your note has been saved!");
        }
        else
        {
            Console.WriteLine("No note added!");
        }
    }

    // Loggar ut hjälten
    public static void LogOut(AvengersProfile currentHero)
    {
        Console.Clear();
        Console.WriteLine($"Avenger {currentHero.Username} has logged out!");
        Thread.Sleep(1500);
    }

    // Visar rapport med alla missions och status
    public static void ShowMissionReport(AvengersProfile hero)
    {
        Console.WriteLine($"Mission Report for {hero.Username}");
        Console.WriteLine("---------------------------------------------");

        var userMissions = missions.Where(m => m.AvengerRole == hero.Username).ToList();

        if (userMissions.Count == 0)
        {
            Console.WriteLine("No missions found for this hero.");
            return;
        }

        foreach (var m in userMissions)
        {
            int daysLeft = (m.DueDate - DateTime.Now).Days;
            string status = m.IsCompleted ? "Completed" : (daysLeft < 0 ? "Overdue" : "Active");

            Console.WriteLine($"Title: {m.Title}");
            Console.WriteLine($"Due: {m.DueDate:yyyy-MM-dd}");
            Console.WriteLine($"Days left: {daysLeft}");
            Console.WriteLine($"Status: {status}");
            int pointsForHero = heroPoints.ContainsKey(hero.Username) ? heroPoints[hero.Username] : 0;
            Console.WriteLine($"Hero total points: {pointsForHero}");
            Console.WriteLine("---------------------------------------------");
        }
    }

    // Visar meny för inloggad hjälte
    public static void ShowLoggedInMenu(AvengersProfile currentHero)
    {
        Console.WriteLine($"Welcome, {currentHero.Username}");
        ShowMissionforAvenger(currentHero);

        Console.WriteLine("1. Add mission: ");
        Console.WriteLine("2. Show all missions: ");
        Console.WriteLine("3. Complete mission: ");
        Console.WriteLine("4. Update mission / Add note: ");
        Console.WriteLine("5. Show mission report: ");
        Console.WriteLine("6. Logout: ");
        Console.Write("Choose an option: ");
    }
}