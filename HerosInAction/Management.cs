using Microsoft.VisualBasic;

public class MissionManagement
{
    public static List<MissionManagement> missions = new List<MissionManagement>()
    {
        new MissionManagement("Retrieve Infinity Stones before Thanos", "Tony Stark must Collect all Infinity stones from Hisingen, Vasa, Frölunda, Angered, Göteborg Centrum.", new DateTime(2025, 10, 20, 13, 0, 0), "High"),
        new MissionManagement("Stop alien drones over Liseberg", "Thor must stop a fleet of small alien drones hovering over the amusement park Liseberg.", new DateTime(2025, 10, 18, 11, 0, 0), "High"),
        new MissionManagement("Evacuate civillians from Nordstan", "Lead the evacuation of civilians from Nordstan during a alien invasion.", new DateTime(2025, 10, 5, 12, 0, 0), "High"),
        new MissionManagement("Crush Chitauri Leviathans at Slottskogen", "Hulk must deal with giant alien robots attacking on Slottskogen Park.", new DateTime(2025, 10, 8, 14, 0, 0), "High"),
        new MissionManagement("Protect NBI/Handelsakademin", "Defend NBI from the incoming enemy army.", new DateTime(2025, 10, 20, 15, 30, 0), "Medium"),
        new MissionManagement("Investigate Strange Activity", "Investigate mysterious magical activity in Opera.", new DateTime(2025, 10, 15, 9, 0, 0), "Low"),
        new MissionManagement("Escort VIPs", "Protect important civilians during evacuation.", new DateTime(2025, 10, 12, 14, 0, 0), "Medium"),
        new MissionManagement("Seal dimentional rift over Karla tower", "Strange must close a magical rift above the Karla tower before aliens come through.", new DateTime(2025, 19, 12, 1, 0, 0), "Medium")
    };

    public string Title;
    public string Description;
    public DateTime DueDate;
    public string Priority;
    public bool IsCompleted;

    public MissionManagement(string title, string description, DateTime dueDate, string priority, bool isCompleted = false)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        IsCompleted = isCompleted;
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

        MissionManagement newMission = new MissionManagement(title, description, dueDate, priority, false);
        missions.Add(newMission);

        Console.WriteLine($"Mission '{title}' added successfully!!");
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
            Console.WriteLine($"{index}. {mission.Title} | Due: {mission.DueDate:yyyy-MM-dd HH:mm} | Priority: {mission.Priority} | Completed: {(mission.IsCompleted ? "YES" : "NO")}");
            index++;
        }
        Console.WriteLine();
    }


    public static void CompleteMission()
    {
        if (missions.Count == 0)
        {
            Console.WriteLine("No missions to complete!");
            return;
        }

        Console.WriteLine("Select the mission number to mark as completed:");
        ShowAllMissions();

        int choice;
        if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= missions.Count)
        {
            missions[choice - 1].IsCompleted = true;
            Console.WriteLine($"Mission '{missions[choice - 1].Title}' is now completed!");
        }
        else
        {
            Console.WriteLine("Invalid selection!");
        }
    }

    public static void UpdateMission()
    {
        Console.WriteLine("");

    }
    public static void LogOut()
    {
        Console.WriteLine("");

    }

    public static void ShowLoggedInMenu(AvengersProfile currentHero)
    {
        Console.WriteLine($"Welcome, {currentHero.Username}");
        Console.WriteLine("1. Add mission: ");
        Console.WriteLine("2. Show all missions: ");
        Console.WriteLine("3. Complete mission: ");
        Console.WriteLine("4. Update mission: ");
        Console.WriteLine("5. Logout: ");
        Console.Write("Choose an option: ");
    }
}