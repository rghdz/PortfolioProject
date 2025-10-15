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
                MissionManagement.CompleteMission(currentHero);
                break;

            case "4":
                MissionManagement.UpdateMission(currentHero);
                break;

            case "5":
                MissionManagement.LogOut(currentHero);
                currentHero = null;
                break;

            default:
                Console.WriteLine("Invalid choice! Try again please!");
                break;
        }
    }
    Console.WriteLine();
}
