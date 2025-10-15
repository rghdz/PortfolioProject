using Microsoft.VisualBasic;

public class MissionManagement
{
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

    public static MissionManagement? GetMissionForAvenger(string avengerRole)
    {
        if (string.IsNullOrWhiteSpace(avengerRole) || missions == null || missions.Count == 0)
            return null;

        string roleTrimmed = avengerRole.Trim();
        
        return missions.FirstOrDefault(m => !string.IsNullOrWhiteSpace(m.AvengerRole) && m.AvengerRole.Trim().Equals(avengerRole.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public string Title;
    public string Description;
    public DateTime DueDate;
    public string Priority;
    public bool IsCompleted;
    public string AvengerRole;
    public string HeroNote;

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


    public static void CompleteMission(AvengersProfile hero)
    {
        var mission = GetMissionForAvenger(hero.Username);
        if (missions == null)
        {
            Console.WriteLine("No missions to complete!");
            return;
        }

        if (mission.IsCompleted)
        {
            Console.WriteLine("Mission Already completed!");
            return;
        }

        mission.IsCompleted = true;
        Console.WriteLine($"Mission '{mission.Title}' compeleted successfully!");
    }
    

    public static void UpdateMission(AvengersProfile hero)
    {
        var mission = GetMissionForAvenger(hero.Username);
        if (mission == null)
        {
            Console.WriteLine("You have no mission to update!");
        }
        Console.WriteLine($"Your current mission: {mission.Title}");
        Console.WriteLine("Would you like to add or update a reminder/comment for this mission?");
        Console.WriteLine("Write your note here or leave blank if you don't feel like it:");
        string newNote = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newNote))
        {
            Console.WriteLine("No note added. Returning to menu...");
            return;
        }
        mission.HeroNote = newNote;
        Console.WriteLine("Your personal note has been saved!");
    }
    public static void LogOut(AvengersProfile currentHero)
    {
        Console.Clear();
        Console.WriteLine($"Avenger {currentHero.Username} has logged out!");
        Thread.Sleep(1500);

    }

    public static void ShowLoggedInMenu(AvengersProfile currentHero)
    {
        Console.WriteLine($"Welcome, {currentHero.Username}");
        ShowMissionforAvenger(currentHero);

        Console.WriteLine("1. Add mission: ");
        Console.WriteLine("2. Show all missions: ");
        Console.WriteLine("3. Complete mission: ");
        Console.WriteLine("4. Update mission: ");
        Console.WriteLine("5. Logout: ");
        Console.Write("Choose an option: ");
    }
}