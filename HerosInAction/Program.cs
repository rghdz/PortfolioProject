using Twilio;
using DotNetEnv;
Console.Clear();
bool isRunning = true;
AvengersProfile currentHero = null;
while (isRunning)
{
    if (currentHero == null)
    {
        AvengersProfile.ShowMainMenu();
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AvengersProfile.CreateUsername();
                break;

            case "2":
                currentHero = AvengersProfile.LoggedIn();
                break;

            case "3":
                Console.WriteLine("Good Bye mighty hero!!!");
                break;

            default:
                Console.WriteLine("Invalid choice! Try again please!");
                break;
        }
    }
    else
    {
        MissionManagement.ShowLoggedInMenu(currentHero);
        string heroChoice = Console.ReadLine();

        switch (heroChoice)
        {
            case "1":
                MissionManagement.AddMission();
                break;

            case "2":
                MissionManagement.ShowAllMissions();
                break;

            case "3":
                MissionManagement.CompleteMission();
                break;

            case "4":
                MissionManagement.UpdateMission();
                break;

            case "5":
                Console.WriteLine($"{currentHero.Username} is now logged out.");
                currentHero = null;
                break;

            default:
                Console.WriteLine("Invalid choice! Try again please!");
                break;
        }
    }
    Console.WriteLine();
}
