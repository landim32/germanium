using System;

namespace Example;

public class Program
{
    // Entry point
    public static void Main(string[] args)
    {
        var name = args.Length > 0 ? args[0] : "World";
        var greeting = $"Hello, {name}!";
        Console.WriteLine(greeting);

        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
                Console.WriteLine($"Even: {i}");
        }
    }
}
