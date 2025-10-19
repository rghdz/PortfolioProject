class Program
{
    static async Task Main(string[] args)
    {
        Console.Clear();

        bool isRunning = true;
        AvengersProfile currentHero = null;

        while (isRunning)
        {
            if (currentHero == null)
            {
                // Visar huvudmeny för användaren
                AvengersProfile.ShowMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Registrera ny hjälte med Jarvis
                        await AvengersProfile.CreateUsernameAsync();
                        break;

                    case "2":
                        // Logga in befintlig hjälte
                        currentHero = AvengersProfile.LoggedIn();
                        break;

                    case "3":
                        // Avslutar programmet
                        Console.WriteLine("Good Bye mighty hero!!!");
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice! Try again please!");
                        break;
                }
            }
            else
            {
                // Visar menyn för inloggad hjälte
                MissionManagement.ShowLoggedInMenu(currentHero);
                string heroChoice = Console.ReadLine();

                switch (heroChoice)
                {
                    case "1":
                        // Lägg till nytt mission manuellt
                        MissionManagement.AddMission();
                        break;

                    case "2":
                        // Visa alla missions
                        MissionManagement.ShowAllMissions();
                        break;

                    case "3":
                        // Complete mission, ger poäng och möjlighet till nya missions
                        await MissionManagement.CompleteMission(currentHero);
                        break;

                    case "4":
                        // Uppdatera mission / lägg till anteckning
                        MissionManagement.UpdateMission(currentHero);
                        break;

                    case "5":
                        // Visa missionrapport med status och poäng
                        MissionManagement.ShowMissionReport(currentHero);
                        break;

                    case "6":
                        // Logga ut hjälten
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
    }
}