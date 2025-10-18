public static class JarvisRoleMatcher
{
    public static string SuggestRole()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Jarvis: Greetings, new Avenger. Please answer the questions so we can determine the best matching role for you.");
        Console.ResetColor();

        Console.WriteLine("Are you more of a (1) Leader or (2) Lone Wolf?");
        string q1 = Console.ReadLine()?.Trim();

        Console.WriteLine("Do you prefer (1) Brains or (2) Brawn?");
        string q2 = Console.ReadLine()?.Trim();

        Console.WriteLine("Would you rather use (1) Magic, (2) Gadgets, or (3) Strength?");
        string q3 = Console.ReadLine()?.Trim();

        string suggestedRole = MatchRole(q1, q2, q3);

        if (suggestedRole == "Iron Man")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("🦾 Jarvis: Excellent choice... however, Iron Man is already taken.");
            Console.WriteLine("Let’s find another hero with similar brilliance...");
            suggestedRole = "Doctor Strange";
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✨ Jarvis: Based on your answers, you should be {suggestedRole}!");
        Console.ResetColor();

        return suggestedRole;
    }

    private static string MatchRole(string q1, string q2, string q3)
    {
        if (q1 == "1" && q2 == "1" && q3 == "2") return "Iron Man";
        if (q1 == "1" && q2 == "2") return "Captain America";
        if (q1 == "2" && q2 == "2" && q3 == "3") return "Hulk";
        if (q3 == "1") return "Doctor Strange";
        if (q3 == "2") return "Spider Man";
        if (q1 == "2" && q2 == "1") return "Black Widow";
        if (q1 == "1" && q3 == "3") return "Thor";
        return "Hawkeye";
    }
}